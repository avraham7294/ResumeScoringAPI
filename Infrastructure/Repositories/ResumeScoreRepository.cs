using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Infrastructure.Repositories
{
    public class ResumeScoreRepository : Repository<ResumeScore>, IResumeScoreRepository
    {
        public ResumeScoreRepository(ResumeDbContext context) : base(context) { }

        public async Task<ResumeScore?> GetScoreForResumeAsync(int resumeId, int jobDescriptionId)
        {
            return await _context.ResumeScores
                .FirstOrDefaultAsync(rs => rs.ResumeId == resumeId && rs.JobDescriptionId == jobDescriptionId);
        }
    }
}
