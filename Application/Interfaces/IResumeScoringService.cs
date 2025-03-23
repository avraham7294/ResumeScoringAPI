using Application.DTOs;

namespace Application.Interfaces
{
    public interface IResumeScoringService
    {
        Task<ResumeScoreResult> ScoreResumeAgainstJobAsync(int resumeId, int jobId);
    }
}
