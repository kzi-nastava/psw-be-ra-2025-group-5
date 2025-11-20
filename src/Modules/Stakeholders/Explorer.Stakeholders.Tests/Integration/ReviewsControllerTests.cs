using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Explorer.API.Controllers;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Infrastructure.Database;
using Explorer.Stakeholders.Core.Domain;
using Explorer.API.Controllers.Administration;

namespace Explorer.Stakeholders.Tests.Integration.AppRatings;

[Collection("Sequential")]
public class AppRatingsTests : BaseStakeholdersIntegrationTest
{
    public AppRatingsTests(StakeholdersTestFactory factory) : base(factory) { }

    [Fact]
    public void Tourist_can_create_rating()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope, "-23");
        var db = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();

        var dto = new CreateAppRatingDto
        {
            Rating = 5,
            Comment = "Sjajna aplikacija!"
        };

        // Act
        var result = ((ObjectResult)controller.Create(dto).Result)?.Value as AppRatingDto;

        // Assert - Response
        result.ShouldNotBeNull();
        result.Id.ShouldNotBe(0);
        result.Rating.ShouldBe(5);
        result.Comment.ShouldBe("Sjajna aplikacija!");
        result.UserId.ShouldBe(-23);

        // Assert - Database
        var stored = db.AppRatings.FirstOrDefault(r => r.Id == result.Id);
        stored.ShouldNotBeNull();
        stored.UserId.ShouldBe(-23);
        stored.Comment.ShouldBe("Sjajna aplikacija!");
    }

    [Fact]
    public void Author_can_update_own_rating()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope, "-21"); 
        var db = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();

        var updateDto = new UpdateAppRatingDto
        {
            Rating = 3,
            Comment = "Izmenjena ocena"
        };

        // Act
        var result = ((ObjectResult)controller.Update(-1, updateDto).Result)?.Value as AppRatingDto;

        // Assert
        result.ShouldNotBeNull();
        result.Id.ShouldBe(-1);
        result.Rating.ShouldBe(3);
        result.Comment.ShouldBe("Izmenjena ocena");

        var stored = db.AppRatings.First(r => r.Id == -1);
        stored.Rating.ShouldBe(3);
        stored.Comment.ShouldBe("Izmenjena ocena");
    }

    [Fact]
    public void User_can_delete_own_rating()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope, "-22"); 
        var db = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();

        // Act
        var result = controller.Delete(-2);

        // Assert
        result.ShouldBeOfType<NoContentResult>();

        var stored = db.AppRatings.FirstOrDefault(r => r.Id == -2);
        stored.ShouldBeNull();
    }

    [Fact]
    public void Admin_can_get_all_ratings()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateAdminController(scope);

        // Act
        var actionResult = controller.GetAll(); 
        var result = actionResult.Result as OkObjectResult; 
        result.ShouldNotBeNull();

        var ratings = result.Value as List<AppRatingDto>;
        ratings.ShouldNotBeNull();
        ratings.Count.ShouldBeGreaterThan(0);

        var first = ratings.First();
        first.UserId.ShouldBe(-21); 
        first.Rating.ShouldBe(5);

        var second = ratings[1];
        second.UserId.ShouldBe(-22); 
        second.Rating.ShouldBe(4);
    }

    private static AdminAppRatingsController CreateAdminController(IServiceScope scope)
    {
        return new AdminAppRatingsController(
            scope.ServiceProvider.GetRequiredService<IAppRatingService>());
    }

    private static AppRatingsController CreateController(IServiceScope scope, string userId)
    {
        return new AppRatingsController(
            scope.ServiceProvider.GetRequiredService<IAppRatingService>())
        {
            ControllerContext = BuildContext(userId)
        };
    }
}
