using Explorer.Tours.Core.Domain.Equipments;
using Explorer.Tours.Core.Domain.Equipments.Entities;
using Explorer.Tours.Core.Domain.Facilities;
using Explorer.Tours.Core.Domain.Monuments;
using Explorer.Tours.Core.Domain.TourExecutions;
using Explorer.Tours.Core.Domain.Preferences;
using Explorer.Tours.Core.Domain.Tours;
using Explorer.Tours.Core.Domain.Tours.Entities;
using Explorer.Tours.Core.Domain.Tours.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Text.Json;
using TransportationType = Explorer.Tours.Core.Domain.Preferences.TransportationType;
using Explorer.Tours.API.Dtos.Locations;

namespace Explorer.Tours.Infrastructure.Database;

public class ToursContext : DbContext
{
    public DbSet<Equipment> Equipment { get; set; }
    public DbSet<Tour> Tours { get; set; }
    public DbSet<Monument> Monument { get; set; }
    public DbSet<TouristEquipment> TouristEquipment { get; set; }
    public DbSet<Facility> Facilities { get; set; }
    public DbSet<TouristPreferences> TouristPreferences { get; set; }
    public DbSet<TourExecution> TourExecutions { get; set; }
    public DbSet<TourReview> TourReviews { get; set; }
    public DbSet<ReviewImage> ReviewImages { get; set; }
    public DbSet<RequiredEquipment> RequiredEquipment { get; set; }
    public DbSet<TourManualProgress> TourManualProgress { get; set; }

    public ToursContext(DbContextOptions<ToursContext> options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("tours");
        modelBuilder.Entity<Monument>().OwnsOne(m => m.Location);
        modelBuilder.Entity<TouristEquipment>().ToTable("TouristEquipment");
        
        ConfigureTour(modelBuilder);
        ConfigureTouristPreferences(modelBuilder);
        ConfigureTourExecution(modelBuilder);
        ConfigureReview(modelBuilder);
        ConfigureTourManual(modelBuilder); 
    }

    private static void ConfigureTour(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Tour>()
                    .OwnsMany(t => t.Durations, d =>
                    {
                        d.WithOwner().HasForeignKey("TourId");
                    });

        modelBuilder.Entity<Tour>()
            .HasMany(t => t.KeyPoints)
            .WithOne()
            .HasForeignKey("TourId")
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(false); // TourId može biti null privremeno

        modelBuilder.Entity<Tour>()
            .Property(t => t.Tags)
            .HasColumnType("text[]");

        // Konfiguracija KeyPoint-a
        modelBuilder.Entity<KeyPoint>()
            .ToTable("KeyPoints");

        // Konfiguracija Location kao JSON vrednosnog objekta
        modelBuilder.Entity<KeyPoint>()
            .Property(kp => kp.Location)
            .HasColumnType("jsonb")
            .HasConversion(
                v => JsonSerializer.Serialize(new { v.Latitude, v.Longitude }, (JsonSerializerOptions)null),
                v => JsonSerializer.Deserialize<LocationDto>(v, (JsonSerializerOptions)null) != null 
                    ? new Location(JsonSerializer.Deserialize<LocationDto>(v, (JsonSerializerOptions)null).Latitude,
                                   JsonSerializer.Deserialize<LocationDto>(v, (JsonSerializerOptions)null).Longitude)
                    : null
            )
            .Metadata.SetValueComparer(new ValueComparer<Location>(
                (l1, l2) => l1 != null && l2 != null && l1.Latitude == l2.Latitude && l1.Longitude == l2.Longitude,
                l => HashCode.Combine(l.Latitude, l.Longitude),
                l => new Location(l.Latitude, l.Longitude)
            ));

        modelBuilder.Entity<KeyPoint>()
            .Property(kp => kp.ImagePath)
            .HasColumnType("text");



        modelBuilder.Entity<Tour>()
        .HasMany(t => t.RequiredEquipment)
        .WithOne()
        .HasForeignKey("TourId")
        .OnDelete(DeleteBehavior.Cascade)
        .IsRequired();

        modelBuilder.Entity<RequiredEquipment>()
            .ToTable("TourRequiredEquipment");
    }

    private static void ConfigureTouristPreferences(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TouristPreferences>()
            .Property(tp => tp.TransportationRatings)
            .HasColumnType("jsonb")
            .HasConversion(
                v => JsonSerializer.Serialize(v, new JsonSerializerOptions { WriteIndented = false }),
                v => JsonSerializer.Deserialize<Dictionary<TransportationType, int>>(v, new JsonSerializerOptions()) ?? new Dictionary<TransportationType, int>()
            )
            .Metadata.SetValueComparer(new ValueComparer<Dictionary<TransportationType, int>>(
                (d1, d2) => d1.SequenceEqual(d2),
                d => d.Aggregate(0, (a, v) => HashCode.Combine(a, v.Key.GetHashCode(), v.Value.GetHashCode())),
                d => new Dictionary<TransportationType, int>(d)));

        modelBuilder.Entity<TouristPreferences>()
            .Property(tp => tp.PreferredTags)
            .HasColumnType("jsonb")
            .HasConversion(
                v => JsonSerializer.Serialize(v, new JsonSerializerOptions { WriteIndented = false }),
                v => JsonSerializer.Deserialize<List<string>>(v, new JsonSerializerOptions()) ?? new List<string>()
            )
            .Metadata.SetValueComparer(ValueComparer.CreateDefault<List<string>>(true));
    }

    private static void ConfigureTourExecution(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TourExecution>()
            .ToTable("TourExecutions");

        modelBuilder.Entity<TourExecution>()
            .OwnsMany(e => e.CompletedKeyPoints, cb =>
            {
                cb.ToTable("KeyPointCompletions");

                cb.WithOwner().HasForeignKey("TourExecutionId");

                cb.Property<long>("Id");
                cb.HasKey("Id");

                cb.Property(kp => kp.KeyPointId).IsRequired();
                cb.Property(kp => kp.CompletedAt).IsRequired();
                cb.Property(kp => kp.DistanceTravelled).IsRequired();
            });
    }

    private static void ConfigureReview(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ReviewImage>(b =>
        {
            b.ToTable("ReviewImages");

            b.HasKey(ri => ri.Id);

            b.Property(ri => ri.ReviewId)
                .IsRequired();

            b.Property(ri => ri.Data)
                .HasColumnType("bytea")
                .IsRequired();

            b.Property(ri => ri.ContentType)
                .HasMaxLength(100)
                .IsRequired();

            b.Property(ri => ri.Order)
                .IsRequired();

            b.HasIndex(ri => new { ri.ReviewId, ri.Order })
                .IsUnique();
        });

        modelBuilder.Entity<TourReview>(b =>
        {
            b.ToTable("TourReviews");

            b.OwnsOne(tr => tr.Progress, p =>
            {
                p.Property(pp => pp.Percentage)
                    .HasColumnName("Progress")
                    .IsRequired();
            });

            b.HasMany(tr => tr.Images)
                .WithOne()
                .HasForeignKey(ri => ri.ReviewId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            b.HasOne<Tour>()
                .WithMany(t => t.Reviews)
                .HasForeignKey(tr => tr.TourID)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            b.Property(tr => tr.TouristUsername)
                .HasMaxLength(100)
                .HasDefaultValue("")
                .IsRequired(false);
        });
    }

    private static void ConfigureTourManual(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TourManualProgress>(b =>
        {
            b.ToTable("TourManualProgress");

            b.HasKey(x => x.Id);

            b.Property(x => x.PageKey)
                .HasMaxLength(100)
                .IsRequired();

            b.HasIndex(x => new { x.UserId, x.PageKey })
                .IsUnique();
        });

    }
}
