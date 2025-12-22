using Explorer.Stakeholders.Core.Domain.AppRatings;
using Explorer.Stakeholders.Core.Domain.Clubs;
using Explorer.Stakeholders.Core.Domain.Comments;
using Explorer.Stakeholders.Core.Domain.Diaries;
using Explorer.Stakeholders.Core.Domain.Notifications;
using Explorer.Stakeholders.Core.Domain.Positions;
using Explorer.Stakeholders.Core.Domain.TourProblems;
using Explorer.Stakeholders.Core.Domain.Users;
using Explorer.Stakeholders.Core.Domain.Users.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

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
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Diary> Diaries { get; set; }

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
            ConfigureNotification(modelBuilder);
            ConfigureDiary(modelBuilder);
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

                builder.Property(tp => tp.Deadline)
                    .IsRequired(false);

                builder.Property(tp => tp.IsResolved)
                    .IsRequired()
                    .HasDefaultValue(false);

                // Foreign key ka User entitetu
                builder.HasOne<User>()
                    .WithMany()
                    .HasForeignKey(tp => tp.ReporterId)
                    .OnDelete(DeleteBehavior.Restrict);

                builder.Property(tp => tp.Comments)
                    .HasColumnType("bigint[]");

                // Index za brže pretraživanje po reporteru
                builder.HasIndex(tp => tp.ReporterId);

                // Index za brže pretraživanje po TourId
                builder.HasIndex(tp => tp.TourId);

                modelBuilder.Entity<Comment>(cb =>
                {
                    cb.HasKey(c => c.CommentId);
                    cb.Property(c => c.CommentId).ValueGeneratedOnAdd();
                    cb.Property(c => c.AuthorId).IsRequired();
                    cb.Property(c => c.Content).IsRequired().HasMaxLength(2000);
                    cb.Property(c => c.CreatedAt).IsRequired().HasDefaultValueSql("NOW()");
                    cb.Property(c => c.UpdatedAt).IsRequired(false);

                    cb.HasIndex(c => c.AuthorId);
                });
            });
        }

        private static void ConfigurePosition(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Position>()
                .HasOne<User>()
                .WithOne()
                .HasForeignKey<Position>(l => l.TouristId);
        }

        private static void ConfigureNotification(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Notification>(builder =>
            {
                builder.HasKey(n => n.Id);

                builder.Property(n => n.UserId)
                    .IsRequired();

                builder.Property(n => n.Type)
                    .HasConversion<int>()
                    .IsRequired();

                builder.Property(n => n.Title)
                    .IsRequired()
                    .HasMaxLength(200);

                builder.Property(n => n.Message)
                    .IsRequired()
                    .HasMaxLength(1000);

                builder.Property(n => n.IsRead)
                    .IsRequired()
                    .HasDefaultValue(false);

                builder.Property(n => n.CreatedAt)
                    .IsRequired();

                builder.Property(n => n.TourProblemId)
                    .IsRequired(false);

                builder.Property(n => n.TourId)
                    .IsRequired(false);

                builder.Property(n => n.ActionUrl)
                    .HasMaxLength(500)
                    .IsRequired(false);

                builder.HasIndex(n => n.UserId);
                builder.HasIndex(n => n.IsRead);
                builder.HasIndex(n => n.CreatedAt);
                builder.HasIndex(n => n.TourProblemId);
                builder.HasIndex(n => n.TourId);
            });
        }

        private static void ConfigureDiary(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Diary>(builder =>
            {
                builder.HasKey(d => d.Id);

                builder.Property(d => d.Name)
                    .IsRequired()
                    .HasMaxLength(300);

                builder.Property(d => d.CreatedAt)
                    .IsRequired()
                    .HasConversion(
                        v => DateTime.SpecifyKind(v, DateTimeKind.Utc),
                        v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
                    );

                builder.Property(d => d.Status)
                    .HasConversion<int>()
                    .IsRequired();

                builder.Property(d => d.Country)
                    .IsRequired()
                    .HasMaxLength(100);

                builder.Property(d => d.City)
                    .HasMaxLength(100)
                    .IsRequired(false);

                builder.Property(d => d.TouristId)
                    .IsRequired();

                builder.HasIndex(d => d.TouristId);
            });
        }

    }
}