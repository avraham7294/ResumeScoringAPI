using AIResumeScoringAPI.Infrastructure.Services;
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

        [HttpPost("upload")]
        public async Task<IActionResult> UploadResume(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Invalid file.");

            var fileName = $"{Guid.NewGuid()}_{file.FileName}";

            using (var stream = file.OpenReadStream())
            {
                var fileUrl = await _blobStorageService.UploadFileAsync(stream, fileName);
                return Ok(new { FileUrl = fileUrl });
            }
        }

        [HttpGet("download/{fileName}")]
        public async Task<IActionResult> DownloadResume(string fileName)
        {
            var fileStream = await _blobStorageService.DownloadFileAsync(fileName);
            if (fileStream == null)
                return NotFound("File not found.");

            return File(fileStream, "application/pdf", fileName);
        }

        [HttpDelete("delete/{fileName}")]
        public async Task<IActionResult> DeleteResume(string fileName)
        {
            var isDeleted = await _blobStorageService.DeleteFileAsync(fileName);
            if (!isDeleted)
                return NotFound("File not found.");

            return Ok(new { Message = "File deleted successfully." });
        }

        [HttpPost("{resumeId}/score/{jobId}")]
        public async Task<IActionResult> ScoreResume(int resumeId, int jobId, [FromServices] ResumeScoringService scorer)
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
