using Explorer.Encounters.Core.Domain;
using Explorer.Encounters.Core.Domain.Social;
using Microsoft.EntityFrameworkCore;

namespace Explorer.Encounters.Infrastructure.Database
{
    public class EncountersContext : DbContext
    {
        public DbSet<Challenge> Challenges { get; set; }
        public DbSet<ChallengeExecution> ChallengeExecutions { get; set; }
        public DbSet<ChallengeParticipation> ChallengeParticipations { get; set; }
        public DbSet<KeyPointChallenge> KeyPointChallenges { get; set; }

        public EncountersContext(DbContextOptions<EncountersContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("encounters");
            modelBuilder.Entity<ChallengeParticipation>(entity =>
            {
                entity.ToTable("ChallengeParticipations");
                entity.HasIndex(e => new { e.ChallengeId, e.TouristId }).IsUnique();


                entity.Property(e => e.LastSeenAt)
                      .IsRequired()
                      .HasDefaultValueSql("NOW()");
            });
        }
    }
}
