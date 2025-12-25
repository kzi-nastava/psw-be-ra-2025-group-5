using Explorer.Blog.Infrastructure.Database;
using Explorer.BuildingBlocks.Tests;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Explorer.Blog.Core.Mappers;
using Explorer.BuildingBlocks.Core.FileStorage;

namespace Explorer.Blog.Tests;

public class BlogTestFactory : BaseTestFactory<BlogContext>
{
    protected override IServiceCollection ReplaceNeededDbContexts(IServiceCollection services)
    {
        var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<BlogContext>));
        services.Remove(descriptor!);
        services.AddDbContext<BlogContext>(SetupTestContext());
        services.AddAutoMapper(typeof(BlogProfile));

        var storageDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IImageStorage));
        if (storageDescriptor != null)
        {
            services.Remove(storageDescriptor);
        }
        services.AddSingleton<IImageStorage, InMemoryImageStorage>();

        return services;
    }
}
