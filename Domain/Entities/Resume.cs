namespace Domain.Entities
{
    /// <summary>
    /// Represents a candidate's resume information.
    /// </summary>
    public class Resume
    {
        /// <summary>
        /// Primary key. Unique identifier for the resume.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Full name of the candidate.
        /// </summary>
        public string CandidateName { get; set; } = string.Empty;

        /// <summary>
        /// Email address of the candidate.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// URL of the uploaded resume file stored in cloud storage.
        /// </summary>
        public string FileUrl { get; set; } = string.Empty;

        /// <summary>
        /// UTC timestamp of when the resume was uploaded.
        /// </summary>
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Collection of resume scores associated with this resume.
        /// </summary>
        public ICollection<ResumeScore>? ResumeScores { get; set; }
    }
}
