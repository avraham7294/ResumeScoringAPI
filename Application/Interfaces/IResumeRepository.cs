using Domain.Entities;

namespace Application.Interfaces
{
    /// <summary>
    /// Interface for performing data operations specifically related to Resumes.
    /// Extends the generic repository interface to include resume-specific queries.
    /// </summary>
    public interface IResumeRepository : IRepository<Resume>
    {
        /// <summary>
        /// Retrieves all resumes that match a specific candidate name.
        /// </summary>
        /// <param name="candidateName">Name of the candidate to search for.</param>
        /// <returns>A collection of resumes matching the candidate name.</returns>
        Task<IEnumerable<Resume>> GetResumesByCandidateAsync(string candidateName);
    }
}
