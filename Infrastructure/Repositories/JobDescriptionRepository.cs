using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Infrastructure.Repositories
{
    /// <summary>
    /// Repository implementation for handling Job Description data operations.
    /// </summary>
    public class JobDescriptionRepository : Repository<JobDescription>, IJobDescriptionRepository
    {
        /// <summary>
        /// Initializes a new instance of <see cref="JobDescriptionRepository"/>.
        /// </summary>
        /// <param name="context">Database context for Resume application.</param>
        public JobDescriptionRepository(ResumeDbContext context) : base(context) { }

        /// <summary>
        /// Retrieves recent job postings added within the specified number of days.
        /// </summary>
        /// <param name="days">Number of days to look back.</param>
        /// <returns>List of recent job descriptions.</returns>
        public async Task<IEnumerable<JobDescription>> GetRecentJobPostingsAsync(int days)
        {
            return await _context.JobDescriptions
                .Where(j => j.PostedAt >= DateTime.UtcNow.AddDays(-days))
                .ToListAsync();
        }
    }
}
