using Explorer.BuildingBlocks.Tests;
using Explorer.Encounters.API.Internal;
using Explorer.Encounters.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Explorer.Encounters.Tests;

public class EncountersTestFactory : BaseTestFactory<EncountersContext>
{
    protected override IServiceCollection ReplaceNeededDbContexts(IServiceCollection services)
    {
        var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<EncountersContext>));
        services.Remove(descriptor!);
        services.AddDbContext<EncountersContext>(SetupTestContext());

        var experienceDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IInternalPersonExperienceService));
        if (experienceDescriptor != null)
        {
            services.Remove(experienceDescriptor);
        }
        services.AddScoped<IInternalPersonExperienceService, StubPersonExperienceService>();

        return services;
    }
}

public class StubPersonExperienceService : IInternalPersonExperienceService
{
    public void AddExperience(long userId, int xpAmount)
    {
        Console.WriteLine($"[STUB] Would add {xpAmount} XP to user {userId}");
    }
}

