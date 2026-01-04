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
    public DbSet<Coupon> Coupons { get; set; }


    public PaymentsContext(DbContextOptions<PaymentsContext> options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("payments");

        ConfigureShoppingCart(modelBuilder);
        ConfigureWallet(modelBuilder);
        ConfigureCoupon(modelBuilder);
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

    private static void ConfigureCoupon(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Coupon>(builder =>
        {
            builder.ToTable("Coupons");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Code)
                .IsRequired()
                .HasMaxLength(8);

            builder.Property(c => c.Percentage)
                .IsRequired();

            builder.Property(c => c.ExpirationDate)
                .IsRequired(false);

            builder.Property(c => c.AuthorId)
                .IsRequired();

            builder.Property(c => c.TourId)
                .IsRequired(false);

            builder.HasIndex(c => c.Code)
                .IsUnique();
        });
    }
}
