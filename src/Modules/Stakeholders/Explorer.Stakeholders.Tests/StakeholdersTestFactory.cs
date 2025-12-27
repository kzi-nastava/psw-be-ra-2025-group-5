using Explorer.BuildingBlocks.Core.FileStorage;
using Explorer.BuildingBlocks.Tests;
using Explorer.Encounters.API.Dtos;
using Explorer.Encounters.API.Public.Tourist;
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

        var challengeServiceDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IChallengeExecutionService));
        if (challengeServiceDescriptor != null)
        {
            services.Remove(challengeServiceDescriptor);
        }
        services.AddScoped<IChallengeExecutionService, StubChallengeExecutionService>();

        return services;
    }
}

public class StubChallengeExecutionService : IChallengeExecutionService
{
    public ChallengeExecutionDto StartChallenge(long challengeId, long touristId)
    {
        return new ChallengeExecutionDto
        {
            Id = 1,
            ChallengeId = challengeId,
            TouristId = touristId,
            Status = "InProgress",
            StartedAt = DateTime.UtcNow
        };
    }

    public ChallengeExecutionDto CompleteChallenge(long executionId, long touristId)
    {
        return new ChallengeExecutionDto
        {
            Id = executionId,
            ChallengeId = -1,
            TouristId = touristId,
            Status = "Completed",
            StartedAt = DateTime.UtcNow.AddMinutes(-10),
            CompletedAt = DateTime.UtcNow
        };
    }

    public ChallengeExecutionDto AbandonChallenge(long executionId, long touristId)
    {
        return new ChallengeExecutionDto
        {
            Id = executionId,
            ChallengeId = -1,
            TouristId = touristId,
            Status = "Abandoned",
            StartedAt = DateTime.UtcNow.AddMinutes(-5),
            AbandonedAt = DateTime.UtcNow
        };
    }

    public ChallengeExecutionDto GetById(long id)
    {
        return new ChallengeExecutionDto
        {
            Id = id,
            ChallengeId = -1,
            TouristId = -21,
            Status = "InProgress",
            StartedAt = DateTime.UtcNow
        };
    }

    public List<ChallengeExecutionDto> GetByTourist(long touristId)
    {
        return new List<ChallengeExecutionDto>();
    }
}
