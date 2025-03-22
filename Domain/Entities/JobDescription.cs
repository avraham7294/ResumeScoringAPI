namespace Domain.Entities
{
    public class JobDescription
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime PostedAt { get; set; } = DateTime.UtcNow;
        public ICollection<ResumeScore>? ResumeScores { get; set; }
    }
}
