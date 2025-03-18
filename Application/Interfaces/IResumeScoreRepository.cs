using Domain.Entities;
// # Resume Score Repository Interface
namespace Application.Interfaces
{
    public interface IResumeScoreRepository : IRepository<ResumeScore>
    {
        Task<ResumeScore?> GetScoreForResumeAsync(int resumeId, int jobDescriptionId);
    }
}
