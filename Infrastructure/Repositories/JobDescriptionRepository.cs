using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Infrastructure.Repositories
{
    public class JobDescriptionRepository : Repository<JobDescription>, IJobDescriptionRepository
    {
        public JobDescriptionRepository(ResumeDbContext context) : base(context) { }

        public async Task<IEnumerable<JobDescription>> GetRecentJobPostingsAsync(int days)
        {
            return await _context.JobDescriptions
                .Where(j => j.PostedAt >= DateTime.UtcNow.AddDays(-days))
                .ToListAsync();
        }
    }
}
