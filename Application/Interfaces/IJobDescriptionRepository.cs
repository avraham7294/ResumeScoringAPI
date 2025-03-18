using Domain.Entities;
//   # Job Description Repository Interface


namespace Application.Interfaces
{
    public interface IJobDescriptionRepository : IRepository<JobDescription>
    {
        Task<IEnumerable<JobDescription>> GetRecentJobPostingsAsync(int days);
    }
}
