using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace AIResumeScoringAPI.Infrastructure.Services
{
    public class ResumeScoringService
    {
        private readonly IConfiguration _configuration;
        private readonly IResumeRepository _resumeRepo;
        private readonly IJobDescriptionRepository _jobRepo;
        private readonly IResumeScoreRepository _scoreRepo;
        private readonly BlobStorageService _blobService;

        private readonly HttpClient _httpClient;

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

        public async Task<ResumeScoreResult> ScoreResumeAgainstJobAsync(int resumeId, int jobId)
        {
            var resume = await _resumeRepo.GetByIdAsync(resumeId);
            var job = await _jobRepo.GetByIdAsync(jobId);

            if (resume == null || job == null)
                throw new Exception("Resume or Job not found");

            // Download and extract text
            var stream = await _blobService.DownloadFileAsync(Path.GetFileName(resume.FileUrl));

            using var memoryStream = new MemoryStream();
            await stream!.CopyToAsync(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin); // Reset position to start

            var resumeText = Infrastructure.Utilities.PdfParser.ExtractTextFromPdf(memoryStream);


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

            var response = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content);
            var responseJson = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"OpenAI API Error: {response.StatusCode} - {responseJson}");
            }

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

                // Store score
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
