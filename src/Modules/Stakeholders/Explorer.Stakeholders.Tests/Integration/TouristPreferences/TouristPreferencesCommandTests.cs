using Explorer.API.Controllers.Tourist;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace Explorer.Stakeholders.Tests.Integration.TouristPreferences;

[Collection("Sequential")]
public class TouristPreferencesCommandTests : BaseStakeholdersIntegrationTest
{
    public TouristPreferencesCommandTests(StakeholdersTestFactory factory) : base(factory) { }

    [Fact]
    public void Creates()
    {
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope, "-22");
        var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();
        var newDto = new TouristPreferencesDto
        {
            UserId = -22,
            PreferredDifficulty = TourDifficulty.Medium,
            TransportationRatings = new Dictionary<TransportationType, int>
            {
                { TransportationType.Walking, 3 },
                { TransportationType.Bicycle, 2 },
                { TransportationType.Car, 1 },
                { TransportationType.Boat, 0 }
            },
            PreferredTags = new List<string> { "culture", "history" }
        };

        var result = ((ObjectResult)controller.Create(newDto).Result)?.Value as TouristPreferencesDto;

        result.ShouldNotBeNull();
        result.UserId.ShouldBe(newDto.UserId);
        result.PreferredDifficulty.ShouldBe(newDto.PreferredDifficulty);

        var stored = dbContext.TouristPreferences.FirstOrDefault(tp => tp.UserId == newDto.UserId);
        stored.ShouldNotBeNull();
        ((TourDifficulty)stored.PreferredDifficulty).ShouldBe(newDto.PreferredDifficulty);
    }
    
    /*[Fact]
    public void Updates()
    {
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope, "-23");
        var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();
        var updateDto = new TouristPreferencesDto
        {
            UserId = -23,
            PreferredDifficulty = TourDifficulty.Hard,
            TransportationRatings = new Dictionary<TransportationType, int>
            {
                { TransportationType.Walking, 1 },
                { TransportationType.Bicycle, 1 },
                { TransportationType.Car, 1 },
                { TransportationType.Boat, 1 }
            },
            PreferredTags = new List<string> { "extreme" }
        };

        var result = ((ObjectResult)controller.Update(updateDto).Result)?.Value as TouristPreferencesDto;

        result.ShouldNotBeNull();
        result.PreferredDifficulty.ShouldBe(updateDto.PreferredDifficulty);

        var stored = dbContext.TouristPreferences.FirstOrDefault(tp => tp.UserId == updateDto.UserId);
        stored.ShouldNotBeNull();
        ((TourDifficulty)stored.PreferredDifficulty).ShouldBe(updateDto.PreferredDifficulty);
    }

    [Fact]
    public void Deletes()
    {
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope, "-21");
        var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();

        var result = (OkResult)controller.Delete();

        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(200);

        var stored = dbContext.TouristPreferences.FirstOrDefault(tp => tp.UserId == -21);
        stored.ShouldBeNull();
    }*/

    private static TouristPreferencesController CreateController(IServiceScope scope, string userId)
    {
        return new TouristPreferencesController(scope.ServiceProvider.GetRequiredService<ITouristPreferencesService>())
        {
            ControllerContext = BuildContext(userId)
        };
    }
}