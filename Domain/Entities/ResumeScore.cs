namespace Domain.Entities
{
    /// <summary>
    /// Represents the score of a candidate's resume against a specific job description.
    /// </summary>
    public class ResumeScore
    {
        /// <summary>
        /// Unique identifier for the ResumeScore record.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Foreign key reference to the Resume entity.
        /// </summary>
        public int ResumeId { get; set; }

        /// <summary>
        /// Foreign key reference to the JobDescription entity.
        /// </summary>
        public int JobDescriptionId { get; set; }

        /// <summary>
        /// AI-evaluated score indicating how well the resume matches the job description.
        /// </summary>
        public double Score { get; set; }

        /// <summary>
        /// Explanation or reasoning behind the assigned score.
        /// </summary>
        public string Reason { get; set; } = string.Empty;

        /// <summary>
        /// Date and time when the resume was scored.
        /// </summary>
        public DateTime ScoredAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Navigation property for the associated Resume.
        /// </summary>
        public Resume? Resume { get; set; }

        /// <summary>
        /// Navigation property for the associated JobDescription.
        /// </summary>
        public JobDescription? JobDescription { get; set; }
    }
}
