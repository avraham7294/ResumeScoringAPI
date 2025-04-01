namespace Application.DTOs
{
    /// <summary>
    /// Data Transfer Object (DTO) that represents the result of scoring a resume against a job description.
    /// </summary>
    public class ResumeScoreResult
    {
        /// <summary>
        /// The score calculated for the resume against the job description.
        /// Score is typically between 0 and 100, indicating how well the resume matches the job.
        /// </summary>
        public double Score { get; set; }

        /// <summary>
        /// A textual explanation providing the reason behind the assigned score.
        /// This may include strengths, weaknesses, or areas of improvement.
        /// </summary>
        public string Reason { get; set; } = string.Empty;
    }
}
