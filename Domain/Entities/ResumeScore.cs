namespace Domain.Entities
{
    public class ResumeScore
    {
        public int Id { get; set; }
        public int ResumeId { get; set; }
        public int JobDescriptionId { get; set; }
        public double Score { get; set; }
        public string Reason { get; set; } = string.Empty;
        public DateTime ScoredAt { get; set; } = DateTime.UtcNow;

        public Resume? Resume { get; set; }
        public JobDescription? JobDescription { get; set; }
    }

}
