using Explorer.API.Controllers.Tourist;
using Explorer.Encounters.API.Dtos;
using Explorer.Encounters.API.Public.Administration;
using Explorer.Encounters.API.Public.Tourist;
using Explorer.Encounters.Infrastructure.Database;
using Explorer.Stakeholders.API.Public.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Explorer.Encounters.Tests.Integration.Tourist;

[Collection("Sequential")]
public class TouristChallengeControllerTests : BaseEncountersIntegrationTest
{
    private readonly IServiceScope _scope;
    private readonly EncountersContext _dbContext;

    public TouristChallengeControllerTests(EncountersTestFactory factory) : base(factory)
    {
        _scope = Factory.Services.CreateScope();
        _dbContext = _scope.ServiceProvider.GetRequiredService<EncountersContext>();
    }

    [Fact]
    public void Create_fails_when_level_less_than_10()
    {
        // Arrange
        var controller = CreateController(_scope, "-21"); 
        var newChallenge = new CreateTouristChallengeDto
        {
            Name = "Neuspešan izazov",
            Description = "Nema levela",
            Latitude = 44.82,
            Longitude = 20.45,
            ExperiencePoints = 50,
            Type = "Social"
        };

        // Act & Assert
        var result = controller.Create(newChallenge).Result;
        result.ShouldBeOfType<ForbidResult>();
        
    }

    private static TouristChallengeController CreateController(IServiceScope scope, string? touristId = "-23")
    {
        var challengeService = scope.ServiceProvider.GetRequiredService<IChallengeService>();
        var challengeExecutionService = scope.ServiceProvider.GetRequiredService<IChallengeExecutionService>();
        var profileService = scope.ServiceProvider.GetRequiredService<IProfileService>();
        var challengeCreationService = scope.ServiceProvider.GetRequiredService<IChallengeCreationService>();

        var controller = new TouristChallengeController(
            challengeService,
            challengeExecutionService,
            profileService,
            challengeCreationService
        );

        controller.ControllerContext = BuildContext(touristId);
        return controller;
    }
}
