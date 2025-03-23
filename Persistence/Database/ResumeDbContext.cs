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
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ResumeScore>()
                .HasOne(rs => rs.Resume)
                .WithMany(r => r.ResumeScores) // Make sure Resume has this navigation property
                .HasForeignKey(rs => rs.ResumeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ResumeScore>()
                .HasOne(rs => rs.JobDescription)
                .WithMany(j => j.ResumeScores) // Make sure JobDescription has this too
                .HasForeignKey(rs => rs.JobDescriptionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
