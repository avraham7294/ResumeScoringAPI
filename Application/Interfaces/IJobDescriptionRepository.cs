using Domain.Entities;

namespace Application.Interfaces
{
    public interface IJobDescriptionRepository : IRepository<JobDescription>
    {
        Task<IEnumerable<JobDescription>> GetRecentJobPostingsAsync(int days);
    }
}
