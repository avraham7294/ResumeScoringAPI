using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Database
{
    public class ResumeDbContext : DbContext
    {
        public ResumeDbContext(DbContextOptions<ResumeDbContext> options) : base(options) { }

        public DbSet<Resume> Resumes { get; set; }
        public DbSet<JobDescription> JobDescriptions { get; set; }
        public DbSet<ResumeScore> ResumeScores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure Relationships
            modelBuilder.Entity<ResumeScore>()
                .HasOne(rs => rs.Resume)
                .WithMany()
                .HasForeignKey(rs => rs.ResumeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ResumeScore>()
                .HasOne(rs => rs.JobDescription)
                .WithMany()
                .HasForeignKey(rs => rs.JobDescriptionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
