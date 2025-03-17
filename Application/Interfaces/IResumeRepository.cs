using Domain.Entities;

namespace Application.Interfaces
{
    public interface IResumeRepository : IRepository<Resume>
    {
        Task<IEnumerable<Resume>> GetResumesByCandidateAsync(string candidateName);
    }
}
