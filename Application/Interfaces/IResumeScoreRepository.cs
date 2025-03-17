using Domain.Entities;

namespace Application.Interfaces
{
    public interface IResumeScoreRepository : IRepository<ResumeScore>
    {
        Task<ResumeScore?> GetScoreForResumeAsync(int resumeId, int jobDescriptionId);
    }
}
