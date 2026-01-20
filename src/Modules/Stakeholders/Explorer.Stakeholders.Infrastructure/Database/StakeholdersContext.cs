using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.Domain.AppRatings;
using Explorer.Stakeholders.Core.Domain.Clubs;
using Explorer.Stakeholders.Core.Domain.ClubMessages;
using Explorer.Stakeholders.Core.Domain.Comments;
using Explorer.Stakeholders.Core.Domain.Diaries;
using Explorer.Stakeholders.Core.Domain.Notifications;
using Explorer.Stakeholders.Core.Domain.Positions;
using Explorer.Stakeholders.Core.Domain.Social;
using Explorer.Stakeholders.Core.Domain.TourProblems;
using Explorer.Stakeholders.Core.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Explorer.Stakeholders.Core.Domain.ProfileMessages;
using Explorer.Stakeholders.Core.Domain.Streaks;


namespace Explorer.Stakeholders.Infrastructure.Database
{
    public class StakeholdersContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<Club> Clubs { get; set; }
        public DbSet<ClubMessage> ClubMessages { get; set; }
        public DbSet<AppRating> AppRatings { get; set; }
        public DbSet<TourProblem> TourProblems { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Diary> Diaries { get; set; }
        public DbSet<ClubInvite> ClubInvites { get; set; }
        public DbSet<ClubMember> ClubMembers { get; set; }
        public DbSet<ClubJoinRequest> ClubJoinRequests { get; set; }
        public DbSet<ProfileFollow> ProfileFollows { get; set; }
        public DbSet<ProfileMessage> ProfileMessages { get; set; }
        public DbSet<Streak> Streaks { get; set; }

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
            ConfigureClub(modelBuilder);
            ConfigureClubMessage(modelBuilder);
            ConfigureClubInvite(modelBuilder);
            ConfigureClubMember(modelBuilder);
            ConfigureClubJoinRequest(modelBuilder);
            ConfigureFollow(modelBuilder);
            ConfigureProfileMessage(modelBuilder);
            ConfigureStreak(modelBuilder);
        }

        private static void ConfigureClubJoinRequest(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClubJoinRequest>(builder =>
            {
                builder.HasKey(cjr => cjr.Id);

                builder.Property(cjr => cjr.ClubId)
                       .IsRequired();

                builder.Property(cjr => cjr.TouristId)
                       .IsRequired();

                builder.Property(cjr => cjr.CreatedAt)
                       .IsRequired();

                builder.HasOne<Club>()
                       .WithMany()
                       .HasForeignKey(cjr => cjr.ClubId)
                       .OnDelete(DeleteBehavior.Cascade);

                builder.HasOne<Person>()
                       .WithMany()
                       .HasForeignKey(cjr => cjr.TouristId)
                       .OnDelete(DeleteBehavior.Cascade);

                builder.HasIndex(cjr => cjr.ClubId);
                builder.HasIndex(cjr => cjr.TouristId);
            });
        }

        private static void ConfigureClub(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Club>(builder =>
            {
                builder.HasKey(c => c.Id);

                builder.Property(c => c.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                builder.Property(c => c.Description)
                    .IsRequired()
                    .HasMaxLength(2000);

                builder.Property(c => c.CreatorId)
                    .IsRequired();

                builder.Property(c => c.ImagePaths)
                    .HasConversion(
                        v => string.Join(";", v),  
                        v => v.Split(';', StringSplitOptions.RemoveEmptyEntries).ToList()  
                    )
                    .Metadata.SetValueComparer(
                        new ValueComparer<List<string>>(
                            (c1, c2) => c1.SequenceEqual(c2),  
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),  
                            c => c.ToList()  
                        )
                    );

                builder.Property(c => c.ImagePaths)
                        .HasColumnType("text")
                        .IsRequired();

                builder.Property(c => c.Status)
                        .HasConversion<int>() 
                        .IsRequired()
                        .HasDefaultValue(Club.ClubStatus.Active);
                builder.Property(c => c.Status)
                        .HasConversion<int>()   
                        .IsRequired();

            });

        }
        private static void ConfigureClubInvite(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClubInvite>(builder =>
            {
                builder.HasKey(ci => ci.Id);
                builder.Property(ci => ci.ClubId).IsRequired();
                builder.Property(ci => ci.TouristId).IsRequired();
                builder.Property(ci => ci.CreatedAt).IsRequired();
                builder.Property(ci => ci.NotificationId).IsRequired();
            });
        }
        private static void ConfigureClubMember(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClubMember>(builder =>
            {
                builder.HasKey(cm => new { cm.ClubId, cm.TouristId });

                builder.Property(cm => cm.ClubId)
                       .IsRequired();

                builder.Property(cm => cm.TouristId)
                       .IsRequired();

                builder.Property(cm => cm.JoinedAt)
                       .IsRequired();

                builder.HasOne<Club>()
                       .WithMany(c => c.Members)
                       .HasForeignKey(cm => cm.ClubId)
                       .OnDelete(DeleteBehavior.Cascade);

                builder.HasOne<Person>()
                       .WithMany()
                       .HasForeignKey(cm => cm.TouristId)
                       .OnDelete(DeleteBehavior.Cascade);

                builder.ToTable("ClubMembers");
            });
        }

        private static void ConfigureClubMessage(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClubMessage>(builder =>
            {
                builder.HasKey(cm => cm.Id);

                builder.Property(cm => cm.ClubId)
                    .IsRequired();

                builder.Property(cm => cm.AuthorId)
                    .IsRequired();

                builder.Property(cm => cm.Content)
                    .IsRequired()
                    .HasMaxLength(280);

                builder.Property(cm => cm.AttachedResourceType)
                    .HasConversion<int>()
                    .IsRequired();

                builder.Property(cm => cm.AttachedResourceId)
                    .IsRequired(false);

                builder.Property(cm => cm.CreatedAt)
                    .IsRequired();

                builder.Property(cm => cm.UpdatedAt)
                    .IsRequired(false);

                builder.HasOne<Club>()
                    .WithMany()
                    .HasForeignKey(cm => cm.ClubId)
                    .OnDelete(DeleteBehavior.Cascade);

                builder.HasIndex(cm => cm.ClubId);
                builder.HasIndex(cm => cm.AuthorId);

                builder.ToTable("ClubMessages");
            });
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
                builder.HasIndex(n => n.ClubId);
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

        private static void ConfigureFollow(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProfileFollow>(entity =>
            {
                entity.HasKey(f => new { f.FollowerId, f.FollowingId });

                entity.HasOne(f => f.Follower)
                      .WithMany(p => p.Following)
                      .HasForeignKey(f => f.FollowerId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(f => f.Following)
                      .WithMany(p => p.Followers)
                      .HasForeignKey(f => f.FollowingId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }

        private static void ConfigureProfileMessage(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProfileMessage>(builder =>
            {
                builder.HasKey(cm => cm.Id);

                builder.Property(cm => cm.Id)
                    .ValueGeneratedOnAdd();

                builder.Property(cm => cm.ReceiverId)
                    .IsRequired();

                builder.Property(cm => cm.AuthorId)
                    .IsRequired();

                builder.Property(cm => cm.Content)
                    .IsRequired()
                    .HasMaxLength(280);

                builder.Property(cm => cm.AttachedResourceType)
                    .HasConversion<int>()
                    .IsRequired();

                builder.Property(cm => cm.AttachedResourceId)
                    .IsRequired(false);

                builder.Property(cm => cm.CreatedAt)
                    .IsRequired();

                builder.Property(cm => cm.UpdatedAt)
                    .IsRequired(false);

                builder.HasIndex(cm => cm.AuthorId);
                builder.HasIndex(cm => cm.ReceiverId);

                builder.ToTable("ProfileMessages");
            });
        }

        private static void ConfigureStreak(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Streak>(builder =>
            {
                builder.HasKey(s => s.Id);
                builder.Property(s => s.UserId)
                    .IsRequired();
                builder.Property(s => s.StartDate)
                    .IsRequired();
                builder.Property(s => s.LastActivity)
                    .IsRequired();
                builder.Property(s => s.LongestStreak)
                    .IsRequired();
                builder.HasIndex(s => s.UserId)
                    .IsUnique();
            });
        }
    }
}