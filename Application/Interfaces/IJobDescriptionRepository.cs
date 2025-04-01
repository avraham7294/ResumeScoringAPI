using Domain.Entities;

namespace Application.Interfaces
{
    /// <summary>
    /// Repository interface for performing operations related to <see cref="JobDescription"/> entities.
    /// Inherits from the generic <see cref="IRepository{T}"/> interface.
    /// </summary>
    public interface IJobDescriptionRepository : IRepository<JobDescription>
    {
        /// <summary>
        /// Retrieves recent job postings that were posted within the specified number of days.
        /// </summary>
        /// <param name="days">The number of past days to consider when fetching recent job postings.</param>
        /// <returns>A collection of recent <see cref="JobDescription"/> entities.</returns>
        Task<IEnumerable<JobDescription>> GetRecentJobPostingsAsync(int days);
    }
}
