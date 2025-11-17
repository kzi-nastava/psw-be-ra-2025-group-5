using Microsoft.EntityFrameworkCore;
using Explorer.Blog.Core.Domain;

namespace Explorer.Blog.Infrastructure.Database;

public class BlogContext : DbContext
{
    public DbSet<BlogPost> BlogPosts { get; set; }
    public DbSet<BlogImage> BlogImages { get; set; }
    public BlogContext(DbContextOptions<BlogContext> options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("blog");

        modelBuilder.Entity<BlogPost>(b =>
        {
            b.Property(x => x.Title).IsRequired().HasMaxLength(300);
            b.Property(x => x.Description).IsRequired().HasColumnType("text");
            b.Property(x => x.CreatedAt).IsRequired();
            b.HasMany(x => x.Images).WithOne().HasForeignKey(i => i.BlogPostId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<BlogImage>(i =>
        {
            i.Property(x => x.Data).IsRequired().HasColumnType("bytea"); 
            i.Property(x => x.ContentType).IsRequired().HasMaxLength(100);
            i.Property(x => x.FileName).HasMaxLength(255);
            i.Property(x => x.Order).IsRequired();
        });
    }
}