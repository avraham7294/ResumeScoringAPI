using Domain.Entities;
//   # Resume-specific Repository Interface

namespace Application.Interfaces
{
    public interface IResumeRepository : IRepository<Resume>
    {
        Task<IEnumerable<Resume>> GetResumesByCandidateAsync(string candidateName);
    }
}
