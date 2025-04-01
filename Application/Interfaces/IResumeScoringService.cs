using Application.DTOs;

namespace Application.Interfaces
{
    /// <summary>
    /// Interface for resume scoring service.
    /// Provides functionality to evaluate and score a candidate's resume against a job description.
    /// </summary>
    public interface IResumeScoringService
    {
        /// <summary>
        /// Scores a resume against a specific job description.
        /// </summary>
        /// <param name="resumeId">ID of the resume to evaluate.</param>
        /// <param name="jobId">ID of the job description to evaluate against.</param>
        /// <returns>A <see cref="ResumeScoreResult"/> object containing the score and reason.</returns>
        Task<ResumeScoreResult> ScoreResumeAgainstJobAsync(int resumeId, int jobId);
    }
}
