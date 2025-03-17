using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Infrastructure.Repositories
{
    public class ResumeRepository : Repository<Resume>, IResumeRepository
    {
        public ResumeRepository(ResumeDbContext context) : base(context) { }

        public async Task<IEnumerable<Resume>> GetResumesByCandidateAsync(string candidateName)
        {
            return await _context.Resumes
                .Where(r => r.CandidateName.Contains(candidateName))
                .ToListAsync();
        }
    }
}
