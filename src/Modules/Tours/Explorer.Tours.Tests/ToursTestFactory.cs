using Explorer.BuildingBlocks.Core.FileStorage;
using Explorer.BuildingBlocks.Tests;
using Explorer.Payments.Infrastructure.Database;
using Explorer.Tours.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Explorer.Tours.Tests;

public class ToursTestFactory : BaseTestFactory<ToursContext>
{
    protected override IServiceCollection ReplaceNeededDbContexts(IServiceCollection services)
    {
        var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ToursContext>));
        services.Remove(descriptor!);
        services.AddDbContext<ToursContext>(SetupTestContext());

        descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<PaymentsContext>));
        services.Remove(descriptor!);
        services.AddDbContext<PaymentsContext>(SetupTestContext());

        var storageDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IImageStorage));
        if (storageDescriptor != null)
        {
            services.Remove(storageDescriptor);
        }
        services.AddSingleton<IImageStorage, InMemoryImageStorage>();

        return services;
    }
}
