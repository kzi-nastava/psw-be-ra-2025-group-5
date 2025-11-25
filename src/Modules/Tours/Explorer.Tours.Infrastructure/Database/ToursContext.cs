using Explorer.Tours.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Text.Json;

namespace Explorer.Tours.Infrastructure.Database;

public class ToursContext : DbContext
{
    public DbSet<Equipment> Equipment { get; set; }
    public DbSet<Tour> Tours { get; set; }
    public DbSet<Monument> Monument { get; set; }
    public DbSet<TouristEquipment> TouristEquipment { get; set; }

    public DbSet<Facility> Facilities { get; set; }
    public DbSet<TouristPreferences> TouristPreferences { get; set; }

    public ToursContext(DbContextOptions<ToursContext> options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("tours");
        modelBuilder.Entity<Monument>().OwnsOne(m => m.Location);
        modelBuilder.Entity<TouristEquipment>().ToTable("TouristEquipment");
        
        ConfigureTouristPreferences(modelBuilder);
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
}