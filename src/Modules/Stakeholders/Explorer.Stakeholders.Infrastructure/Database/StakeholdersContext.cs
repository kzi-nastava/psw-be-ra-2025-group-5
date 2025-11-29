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
        public DbSet<TourProblem> TourProblems { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Comment> Comments { get; set; }

        public StakeholdersContext(DbContextOptions<StakeholdersContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("stakeholders");

            // Unique constraint na Username
            modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();

            // Konfiguracije
            ConfigureStakeholder(modelBuilder);
            ConfigureAppRating(modelBuilder);
            ConfigureTourProblem(modelBuilder);
            ConfigurePosition(modelBuilder);
            ConfigureComment(modelBuilder);
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

        private static void ConfigureTourProblem(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TourProblem>(builder =>
            {
                builder.HasKey(tp => tp.Id);

                builder.Property(tp => tp.TourId)
                    .IsRequired();

                builder.Property(tp => tp.ReporterId)
                    .IsRequired();

                // Enumi se čuvaju kao integer vrednosti u bazi (0, 1, 2, 3...)
                builder.Property(tp => tp.Category)
                    .HasConversion<int>()
                    .IsRequired();

                builder.Property(tp => tp.Priority)
                    .HasConversion<int>()
                    .IsRequired();

                builder.Property(tp => tp.Description)
                    .IsRequired()
                    .HasMaxLength(2000);

                builder.Property(tp => tp.OccurredAt)
                    .IsRequired();

                builder.Property(tp => tp.CreatedAt)
                    .IsRequired()
                    .HasDefaultValueSql("NOW()"); // PostgreSQL funkcija za trenutno vreme

                // Foreign key ka User entitetu
                builder.HasOne<User>()
                    .WithMany()
                    .HasForeignKey(tp => tp.ReporterId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Index za brže pretraživanje po reporteru
                builder.HasIndex(tp => tp.ReporterId);

                // Index za brže pretraživanje po TourId
                builder.HasIndex(tp => tp.TourId);

            });
        }

        private static void ConfigurePosition(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Position>()
                .HasOne<User>()
                .WithOne()
                .HasForeignKey<Position>(l => l.TouristId);
        }

        private static void ConfigureComment(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Comment>(builder =>
            {
                builder.HasKey(c => c.CommentId);
                builder.Property(c => c.AuthorId).IsRequired();
                builder.Property(c => c.Content).IsRequired().HasMaxLength(2000);
                builder.Property(c => c.CreatedAt).IsRequired().HasDefaultValueSql("NOW()");
                builder.Property(c => c.UpdatedAt).IsRequired(false);
                builder.HasOne<User>().WithMany().HasForeignKey(c => c.AuthorId).OnDelete(DeleteBehavior.Restrict);
                builder.HasIndex(c => c.AuthorId);

            });
        }
    }
}