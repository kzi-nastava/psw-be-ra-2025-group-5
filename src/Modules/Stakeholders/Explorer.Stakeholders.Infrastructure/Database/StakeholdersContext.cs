using Explorer.Stakeholders.Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace Explorer.Stakeholders.Infrastructure.Database
{
    public class StakeholdersContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<Club> Clubs { get; set; }
        public DbSet<AppRating> AppRatings { get; set; }

        public StakeholdersContext(DbContextOptions<StakeholdersContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("stakeholders");

            // Unique constraint na Username
            modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();

            // Konfiguracije
            ConfigureStakeholder(modelBuilder);
            ConfigureAppRating(modelBuilder);
        }

        private static void ConfigureStakeholder(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>()
                .HasOne<User>()
                .WithOne()
                .HasForeignKey<Person>(p => p.UserId);
        }

        private static void ConfigureAppRating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppRating>()
                .Property(x => x.CreatedAt)
                .HasConversion(
                    v => DateTime.SpecifyKind(v, DateTimeKind.Utc),
                    v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
                );

            modelBuilder.Entity<AppRating>()
                .Property(x => x.Rating)
                .IsRequired();

            modelBuilder.Entity<AppRating>()
                .Property(x => x.UserId)
                .IsRequired();
        }
    }
}
