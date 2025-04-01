using AIResumeScoringAPI.Infrastructure.Utilities;
using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace AIResumeScoringAPI.Infrastructure.Services
{
    /// <summary>
    /// Service responsible for scoring resumes against job descriptions using AI evaluation.
    /// </summary>
    public class ResumeScoringService : IResumeScoringService
    {
        private readonly IConfiguration _configuration;
        private readonly IResumeRepository _resumeRepo;
        private readonly IJobDescriptionRepository _jobRepo;
        private readonly IResumeScoreRepository _scoreRepo;
        private readonly BlobStorageService _blobService;
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResumeScoringService"/> class.
        /// </summary>
        public ResumeScoringService(
            IConfiguration config,
            IResumeRepository resumeRepo,
            IJobDescriptionRepository jobRepo,
            IResumeScoreRepository scoreRepo,
            BlobStorageService blobService)
        {
            _configuration = config;
            _resumeRepo = resumeRepo;
            _jobRepo = jobRepo;
            _scoreRepo = scoreRepo;
            _blobService = blobService;
            _httpClient = new HttpClient();
        }

        /// <summary>
        /// Scores a resume against a specific job description.
        /// </summary>
        /// <param name="resumeId">The ID of the resume.</param>
        /// <param name="jobId">The ID of the job description.</param>
        /// <returns>A <see cref="ResumeScoreResult"/> containing the score and reason.</returns>
        /// <exception cref="Exception">Thrown when resume, job, or AI evaluation fails.</exception>
        public async Task<ResumeScoreResult> ScoreResumeAgainstJobAsync(int resumeId, int jobId)
        {
            // 🔸 Retrieve Resume and Job Description from database
            var resume = await _resumeRepo.GetByIdAsync(resumeId);
            var job = await _jobRepo.GetByIdAsync(jobId);

            if (resume == null || job == null)
                throw new Exception("Resume or Job not found");

            // 🔸 Download resume PDF file from Blob Storage
            var stream = await _blobService.DownloadFileAsync(Path.GetFileName(resume.FileUrl));
            if (stream == null)
                throw new Exception("Failed to download resume file from storage.");

            using var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin); // Reset stream position

            // 🔸 Extract text content from PDF
            var resumeText = PdfParser.ExtractTextFromPdf(memoryStream);

            // 🔸 Prepare AI Prompt for evaluation
            var prompt = $$"""
            Act as an experienced HR evaluator.

            You're given:
            - A job description
            - A candidate resume

            Evaluate how well the resume matches the job.

            Respond with only this JSON object on a single line:
            { "score": 85, "reason": "Strong match in required skills and experience." }

            --- Job Description ---
            {{job.Description}}

            --- Resume ---
            {{resumeText}}
            """;

            // 🔸 Prepare request to OpenAI API
            var request = new
            {
                model = "gpt-4o-mini-2024-07-18",
                messages = new[] {
                    new { role = "user", content = prompt }
                }
            };

            var apiKey = _configuration["OpenAI:ApiKey"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

            var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

            // 🔸 Send request to OpenAI API
            var response = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content);
            var responseJson = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"OpenAI API Error: {response.StatusCode} - {responseJson}");
            }

            // 🔸 Parse AI API Response
            using var doc = JsonDocument.Parse(responseJson);
            var contents = doc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

            try
            {
                var wrappedJson = contents;
                var json = JsonDocument.Parse(wrappedJson);

                double score = json.RootElement.GetProperty("score").GetDouble();
                string reason = json.RootElement.GetProperty("reason").GetString();

                // 🔸 Save score to database
                var resumeScore = new ResumeScore
                {
                    ResumeId = resume.Id,
                    JobDescriptionId = job.Id,
                    Score = score,
                    Reason = reason
                };

                await _scoreRepo.AddAsync(resumeScore);
                await _scoreRepo.SaveChangesAsync();

                return new ResumeScoreResult
                {
                    Score = score,
                    Reason = reason
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to parse AI response: {ex.Message}");
            }
        }
    }
}
