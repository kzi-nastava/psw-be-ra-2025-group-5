using Explorer.Stakeholders.Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace Explorer.Stakeholders.Infrastructure.Database;

public class StakeholdersContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Person> People { get; set; }
    public DbSet<TourProblem> TourProblems { get; set; }

    public StakeholdersContext(DbContextOptions<StakeholdersContext> options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("stakeholders");

        modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();

        ConfigureStakeholder(modelBuilder);
        ConfigureTourProblem(modelBuilder); 

    }

    private static void ConfigureStakeholder(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Person>()
            .HasOne<User>()
            .WithOne()
            .HasForeignKey<Person>(s => s.UserId);
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
}