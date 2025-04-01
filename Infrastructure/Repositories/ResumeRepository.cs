using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Infrastructure.Repositories
{
    /// <summary>
    /// Repository implementation for performing data operations related to resumes.
    /// </summary>
    public class ResumeRepository : Repository<Resume>, IResumeRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResumeRepository"/> class.
        /// </summary>
        /// <param name="context">Database context instance.</param>
        public ResumeRepository(ResumeDbContext context) : base(context) { }

        /// <summary>
        /// Retrieves a list of resumes based on the candidate's name.
        /// </summary>
        /// <param name="candidateName">Candidate's name to filter resumes.</param>
        /// <returns>A list of resumes matching the candidate name.</returns>
        public async Task<IEnumerable<Resume>> GetResumesByCandidateAsync(string candidateName)
        {
            return await _context.Resumes
                .Where(r => r.CandidateName.Contains(candidateName))
                .ToListAsync();
        }
    }
}
