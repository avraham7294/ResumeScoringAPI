using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Infrastructure.Repositories
{
    /// <summary>
    /// Repository implementation for handling ResumeScore-related database operations.
    /// </summary>
    public class ResumeScoreRepository : Repository<ResumeScore>, IResumeScoreRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResumeScoreRepository"/> class.
        /// </summary>
        /// <param name="context">The database context used to access ResumeScore entities.</param>
        public ResumeScoreRepository(ResumeDbContext context) : base(context) { }

        /// <summary>
        /// Retrieves the score of a specific resume against a specific job description.
        /// </summary>
        /// <param name="resumeId">The ID of the resume.</param>
        /// <param name="jobDescriptionId">The ID of the job description.</param>
        /// <returns>The matching <see cref="ResumeScore"/> record, or null if not found.</returns>
        public async Task<ResumeScore?> GetScoreForResumeAsync(int resumeId, int jobDescriptionId)
        {
            return await _context.ResumeScores
                .FirstOrDefaultAsync(rs => rs.ResumeId == resumeId && rs.JobDescriptionId == jobDescriptionId);
        }
    }
}
