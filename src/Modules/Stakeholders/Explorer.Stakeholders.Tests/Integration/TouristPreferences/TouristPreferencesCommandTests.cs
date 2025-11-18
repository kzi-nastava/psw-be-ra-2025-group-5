using Explorer.API.Controllers.Tourist;
using Explorer.BuildingBlocks.Core.Exceptions;
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
    public void Gets()
    {
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope, "-23");
        var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();

        var result = ((ObjectResult)controller.Get().Result)?.Value as TouristPreferencesDto;

        result.ShouldNotBeNull();
        result.UserId.ShouldBe(-23);
        result.PreferredDifficulty.ShouldBe(TourDifficulty.Medium);
        result.TransportationRatings.ShouldContainKey(TransportationType.Walking);
        result.TransportationRatings[TransportationType.Walking].ShouldBe(2);
        result.TransportationRatings.ShouldContainKey(TransportationType.Bicycle);
        result.TransportationRatings[TransportationType.Bicycle].ShouldBe(3);
        result.TransportationRatings.ShouldContainKey(TransportationType.Car);
        result.TransportationRatings[TransportationType.Car].ShouldBe(1);
        result.TransportationRatings.ShouldContainKey(TransportationType.Boat);
        result.TransportationRatings[TransportationType.Boat].ShouldBe(0);
        result.PreferredTags.ShouldContain("adventure");
        result.PreferredTags.ShouldContain("nature");
    }

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

    [Fact]
    public void Create_fails_already_exists()
    {
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope, "-21");
        var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();
        var newDto = new TouristPreferencesDto
        {
            UserId = -21,
            PreferredDifficulty = TourDifficulty.Easy,
            TransportationRatings = new Dictionary<TransportationType, int>
            {
                { TransportationType.Walking, 1 },
                { TransportationType.Bicycle, 1 },
                { TransportationType.Car, 1 },
                { TransportationType.Boat, 1 }
            },
            PreferredTags = new List<string> { "test" }
        };

        Should.Throw<InvalidOperationException>(() => controller.Create(newDto));

        var countBefore = dbContext.TouristPreferences.Count(tp => tp.UserId == -21);
    }

    [Fact]
    public void Create_fails_invalid_user_id()
    {
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope, "invalid"); // Nevalidan userId
        var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();
        var newDto = new TouristPreferencesDto
        {
            UserId = 0,
            PreferredDifficulty = TourDifficulty.Medium,
            TransportationRatings = new Dictionary<TransportationType, int>
            {
                { TransportationType.Walking, 2 },
                { TransportationType.Bicycle, 2 },
                { TransportationType.Car, 2 },
                { TransportationType.Boat, 2 }
            },
            PreferredTags = new List<string> { "invalid" }
        };

        Should.Throw<Exception>(() => controller.Create(newDto));
    }

    [Fact]
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
    public void Update_fails_invalid_id()
    {
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope, "-999");
        var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();
        var updateDto = new TouristPreferencesDto
        {
            UserId = -999,
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

        Should.Throw<NotFoundException>(() => controller.Update(updateDto));
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

        var stored = dbContext.TouristPreferences.FirstOrDefault(tp => tp.UserId == -2);
        stored.ShouldBeNull();
    }

    [Fact]
    public void Delete_fails_invalid_id()
    {
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope, "-999");
        var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();

        Should.Throw<NotFoundException>(() => controller.Delete());
    }

    private static TouristPreferencesController CreateController(IServiceScope scope, string userId)
    {
        return new TouristPreferencesController(scope.ServiceProvider.GetRequiredService<ITouristPreferencesService>())
        {
            ControllerContext = BuildContext(userId)
        };
    }
}