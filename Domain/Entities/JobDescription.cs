namespace Domain.Entities
{
    /// <summary>
    /// Represents a Job Description entity.
    /// Contains information about the job title, description, and the date it was posted.
    /// </summary>
    public class JobDescription
    {
        /// <summary>
        /// Unique identifier for the job description.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Title of the job position.
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Full textual description of the job.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Date and time when the job was posted.
        /// Defaults to current UTC time.
        /// </summary>
        public DateTime PostedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Collection of ResumeScores associated with this job description.
        /// Represents the scoring results of different resumes against this job.
        /// </summary>
        public ICollection<ResumeScore>? ResumeScores { get; set; }
    }
}
