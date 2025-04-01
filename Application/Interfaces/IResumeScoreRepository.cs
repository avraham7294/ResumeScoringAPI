using Domain.Entities;

namespace Application.Interfaces
{
    /// <summary>
    /// Interface for Resume Score repository operations.
    /// Inherits from the generic repository interface and adds resume score-specific operations.
    /// </summary>
    public interface IResumeScoreRepository : IRepository<ResumeScore>
    {
        /// <summary>
        /// Retrieves the score of a specific resume against a specific job description, if available.
        /// </summary>
        /// <param name="resumeId">ID of the resume.</param>
        /// <param name="jobDescriptionId">ID of the job description.</param>
        /// <returns>Resume score entity if found; otherwise, null.</returns>
        Task<ResumeScore?> GetScoreForResumeAsync(int resumeId, int jobDescriptionId);
    }
}
