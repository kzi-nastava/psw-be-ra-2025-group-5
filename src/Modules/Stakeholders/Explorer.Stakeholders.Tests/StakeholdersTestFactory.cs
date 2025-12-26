using Explorer.BuildingBlocks.Core.FileStorage;
using Explorer.BuildingBlocks.Tests;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using Explorer.Stakeholders.Core.UseCases;
using Explorer.Stakeholders.Infrastructure.Database;
using Explorer.Stakeholders.Infrastructure.Database.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Explorer.Stakeholders.Tests;

public class StakeholdersTestFactory : BaseTestFactory<StakeholdersContext>
{
    protected override IServiceCollection ReplaceNeededDbContexts(IServiceCollection services)
    {
        var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<StakeholdersContext>));
        services.Remove(descriptor!);
        services.AddDbContext<StakeholdersContext>(SetupTestContext());

        var storageDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IImageStorage));
        if (storageDescriptor != null)
        {
            services.Remove(storageDescriptor);
        }
        services.AddSingleton<IImageStorage, InMemoryImageStorage>();

        return services;
    }
}