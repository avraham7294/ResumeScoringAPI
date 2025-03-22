namespace Domain.Entities
{
    public class Resume
    {
        public int Id { get; set; }
        public string CandidateName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FileUrl { get; set; } = string.Empty; // Cloud Storage URL
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
        public ICollection<ResumeScore>? ResumeScores { get; set; }

    }
}
