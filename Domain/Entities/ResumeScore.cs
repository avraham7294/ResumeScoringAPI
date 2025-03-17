namespace Domain.Entities
{
    public class ResumeScore
    {
        public int Id { get; set; }
        public int ResumeId { get; set; }
        public int JobDescriptionId { get; set; }
        public double Score { get; set; } // AI-generated score
        public DateTime ScoredAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public Resume? Resume { get; set; }
        public JobDescription? JobDescription { get; set; }
    }
}
