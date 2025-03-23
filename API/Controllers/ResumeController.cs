using AIResumeScoringAPI.Infrastructure.Services;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;


namespace AIResumeScoringAPI.API.Controllers
{
    [ApiController]
    [Route("api/resumes")]
    public class ResumeController : ControllerBase
    {
        private readonly BlobStorageService _blobStorageService;

        public ResumeController(BlobStorageService blobStorageService)
        {
            _blobStorageService = blobStorageService;
        }

        [Authorize]
        [HttpPost("upload")]
        public async Task<IActionResult> UploadResume(
    IFormFile file,
    [FromForm] string candidateName,
    [FromForm] string email,
    [FromServices] IResumeRepository resumeRepository)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Invalid file.");

            var fileName = $"{Guid.NewGuid()}_{file.FileName}";

            using (var stream = file.OpenReadStream())
            {
                var fileUrl = await _blobStorageService.UploadFileAsync(stream, fileName);

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

        [Authorize]
        [HttpGet("download/{fileName}")]
        public async Task<IActionResult> DownloadResume(string fileName)
        {
            var fileStream = await _blobStorageService.DownloadFileAsync(fileName);
            if (fileStream == null)
                return NotFound("File not found.");

            return File(fileStream, "application/pdf", fileName);
        }

        [Authorize]
        [HttpDelete("delete/{fileName}")]
        public async Task<IActionResult> DeleteResume(string fileName)
        {
            var isDeleted = await _blobStorageService.DeleteFileAsync(fileName);
            if (!isDeleted)
                return NotFound("File not found.");

            return Ok(new { Message = "File deleted successfully." });
        }

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


    }
}
