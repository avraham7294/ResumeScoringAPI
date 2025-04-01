using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Database
{
    /// <summary>
    /// Database context class for Resume Scoring API.
    /// Manages entity sets and database configurations.
    /// </summary>
    public class ResumeDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResumeDbContext"/> class.
        /// </summary>
        /// <param name="options">Options to configure the context.</param>
        public ResumeDbContext(DbContextOptions<ResumeDbContext> options) : base(options) { }

        /// <summary>
        /// Gets or sets the Resumes table.
        /// </summary>
        public DbSet<Resume> Resumes { get; set; }

        /// <summary>
        /// Gets or sets the JobDescriptions table.
        /// </summary>
        public DbSet<JobDescription> JobDescriptions { get; set; }

        /// <summary>
        /// Gets or sets the ResumeScores table.
        /// </summary>
        public DbSet<ResumeScore> ResumeScores { get; set; }

        /// <summary>
        /// Gets or sets the Users table.
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// Configures the entity relationships and behaviors.
        /// </summary>
        /// <param name="modelBuilder">Model builder for configuring entities.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure ResumeScore -> Resume relationship (many-to-one)
            modelBuilder.Entity<ResumeScore>()
                .HasOne(rs => rs.Resume)
                .WithMany(r => r.ResumeScores)
                .HasForeignKey(rs => rs.ResumeId)
                .OnDelete(DeleteBehavior.Cascade); // Delete ResumeScore if Resume is deleted

            // Configure ResumeScore -> JobDescription relationship (many-to-one)
            modelBuilder.Entity<ResumeScore>()
                .HasOne(rs => rs.JobDescription)
                .WithMany(j => j.ResumeScores)
                .HasForeignKey(rs => rs.JobDescriptionId)
                .OnDelete(DeleteBehavior.Cascade); // Delete ResumeScore if JobDescription is deleted
        }
    }
}
