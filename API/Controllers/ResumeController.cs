using AIResumeScoringAPI.Infrastructure.Services;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace AIResumeScoringAPI.API.Controllers
{
    /// <summary>
    /// Controller responsible for handling resume-related operations such as upload, download, delete, and scoring.
    /// </summary>
    [ApiController]
    [Route("api/resumes")]
    public class ResumeController : ControllerBase
    {
        private readonly BlobStorageService _blobStorageService;

        /// <summary>
        /// Constructor to initialize the ResumeController with required services.
        /// </summary>
        /// <param name="blobStorageService">Service for interacting with Azure Blob Storage.</param>
        public ResumeController(BlobStorageService blobStorageService)
        {
            _blobStorageService = blobStorageService;
        }

        /// <summary>
        /// Uploads a candidate's resume in PDF format.
        /// </summary>
        /// <param name="file">PDF file to upload.</param>
        /// <param name="candidateName">Candidate's name.</param>
        /// <param name="email">Candidate's email address.</param>
        /// <param name="resumeRepository">Resume repository service.</param>
        /// <returns>Result of the upload operation.</returns>
        [Authorize]
        [HttpPost("upload")]
        public async Task<IActionResult> UploadResume(
            IFormFile file,
            [FromForm] string candidateName,
            [FromForm] string email,
            [FromServices] IResumeRepository resumeRepository)
        {
            // 🔸 Basic Input Validation
            if (file == null || file.Length == 0)
                return BadRequest("Resume file is required.");

            if (!file.FileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
                return BadRequest("Only PDF files are allowed.");

            if (file.Length > 5 * 1024 * 1024) // 5 MB limit
                return BadRequest("File size must be under 5MB.");

            if (string.IsNullOrWhiteSpace(candidateName))
                return BadRequest("Candidate name is required.");

            if (string.IsNullOrWhiteSpace(email) || !IsValidEmail(email))
                return BadRequest("A valid email address is required.");

            // 🔸 Generate a secure and unique file name
            var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";

            using (var stream = file.OpenReadStream())
            {
                // 🔸 Upload file to Azure Blob Storage
                var fileUrl = await _blobStorageService.UploadFileAsync(stream, fileName);

                // 🔸 Save resume metadata to the database
                var resume = new Resume
                {
                    CandidateName = candidateName,
                    Email = email,
                    FileUrl = fileUrl,
                    UploadedAt = DateTime.UtcNow
                };

                await resumeRepository.AddAsync(resume);
                await resumeRepository.SaveChangesAsync();

                return Ok(new
                {
                    Message = "Resume uploaded and saved successfully.",
                    ResumeId = resume.Id,
                    FileUrl = fileUrl
                });
            }
        }

        /// <summary>
        /// Downloads a previously uploaded resume by file name.
        /// </summary>
        /// <param name="fileName">Name of the file to download.</param>
        /// <returns>PDF file stream.</returns>
        [Authorize]
        [HttpGet("download/{fileName}")]
        public async Task<IActionResult> DownloadResume(string fileName)
        {
            var fileStream = await _blobStorageService.DownloadFileAsync(fileName);

            if (fileStream == null)
                return NotFound("File not found.");

            return File(fileStream, "application/pdf", fileName);
        }

        /// <summary>
        /// Deletes a previously uploaded resume by file name.
        /// </summary>
        /// <param name="fileName">Name of the file to delete.</param>
        /// <returns>Result of the delete operation.</returns>
        [Authorize]
        [HttpDelete("delete/{fileName}")]
        public async Task<IActionResult> DeleteResume(string fileName)
        {
            var isDeleted = await _blobStorageService.DeleteFileAsync(fileName);

            if (!isDeleted)
                return NotFound("File not found.");

            return Ok(new { Message = "File deleted successfully." });
        }

        /// <summary>
        /// Scores a candidate's resume against a job description using AI evaluation.
        /// </summary>
        /// <param name="resumeId">ID of the resume to score.</param>
        /// <param name="jobId">ID of the job description to compare against.</param>
        /// <param name="scorer">Resume scoring service.</param>
        /// <returns>Score result with reason.</returns>
        [Authorize]
        [HttpPost("{resumeId}/score/{jobId}")]
        public async Task<IActionResult> ScoreResume(int resumeId, int jobId, [FromServices] IResumeScoringService scorer)
        {
            try
            {
                var result = await scorer.ScoreResumeAgainstJobAsync(resumeId, jobId);
                return Ok(new
                {
                    ResumeId = resumeId,
                    JobId = jobId,
                    Score = result.Score,
                    Reason = result.Reason
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Validates if an email address is in correct format.
        /// </summary>
        /// <param name="email">Email address to validate.</param>
        /// <returns>True if valid, otherwise false.</returns>
        private bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }
    }
}
