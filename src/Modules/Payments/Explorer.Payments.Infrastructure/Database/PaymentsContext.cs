using Explorer.Payments.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Text.Json;

namespace Explorer.Payments.Infrastructure.Database;

public class PaymentsContext: DbContext
{
    public DbSet<ShoppingCart> ShoppingCarts { get; set; }
    public DbSet<TourPurchaseToken> TourPurchaseTokens { get; set; }
    public DbSet<Wallet> Wallets { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<TourSale> TourSales { get; set; }

    public PaymentsContext(DbContextOptions<PaymentsContext> options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("payments");

        ConfigureShoppingCart(modelBuilder);
        ConfigureWallet(modelBuilder);
        ConfigureTourSale(modelBuilder);
    }

    private static void ConfigureShoppingCart(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ShoppingCart>()
        .Property(s => s.Items)
        .HasColumnType("jsonb")
        .HasConversion(
            items => JsonSerializer.Serialize(items, new JsonSerializerOptions { WriteIndented = false }),
            json => JsonSerializer.Deserialize<List<OrderItem>>(json, new JsonSerializerOptions()) ?? new List<OrderItem>(),
            new ValueComparer<List<OrderItem>>(
                (c1, c2) => c1.SequenceEqual(c2),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c.ToList()
            )
        );
    }

    private static void ConfigureWallet(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Wallet>(builder =>
        {
            builder.ToTable("Wallets");
            builder.HasKey(w => w.Id);

            builder.Property(w => w.TouristId).IsRequired();
            builder.Property(w => w.Balance).IsRequired();

            builder.HasIndex(w => w.TouristId).IsUnique(); 
        });
    }

    private static void ConfigureTourSale(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TourSale>()
        .Property(s => s.TourIds)
        .HasColumnType("integer[]");

        modelBuilder.Entity<TourSale>()
        .Property(ts => ts.CreationDate)
        .ValueGeneratedOnAdd()
        .HasDefaultValueSql("NOW()");
    }
}
