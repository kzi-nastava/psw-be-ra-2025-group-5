using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Explorer.API.Controllers;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Infrastructure.Database;
using Explorer.Stakeholders.Core.Domain;
using Explorer.API.Controllers.Administration;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace Explorer.Stakeholders.Tests.Integration.AppRatings;

[Collection("Sequential")]
public class AppRatingsTests : BaseStakeholdersIntegrationTest
{
    public AppRatingsTests(StakeholdersTestFactory factory) : base(factory) { }


    [Fact]
    public void Tourist_can_create_rating()
    {
        using var scope = Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();

        var controller = CreateController(scope, "-23", "tourist");

        var dto = new CreateAppRatingDto
        {
            Rating = 5,
            Comment = "Sjajna aplikacija!"
        };

        var result = ((ObjectResult)controller.Create(dto).Result)?.Value as AppRatingDto;

        result.ShouldNotBeNull();
        result.Id.ShouldNotBe(0);
        result.Rating.ShouldBe(5);
        result.Comment.ShouldBe("Sjajna aplikacija!");
        result.UserId.ShouldBe(-23);

        var stored = db.AppRatings.FirstOrDefault(r => r.Id == result.Id);
        stored.ShouldNotBeNull();
        stored.UserId.ShouldBe(-23);
        stored.Comment.ShouldBe("Sjajna aplikacija!");
    }

    [Fact]
    public void Author_can_update_own_rating()
    {
        using var scope = Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();

        var controller = CreateController(scope, "-21", "author");

        var updateDto = new UpdateAppRatingDto
        {
            Rating = 3,
            Comment = "Izmenjena ocena"
        };

        var result = ((ObjectResult)controller.Update(-1, updateDto).Result)?.Value as AppRatingDto;

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
        using var scope = Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();

        var controller = CreateController(scope, "-22", "tourist");

        var result = controller.Delete(-2);

        result.ShouldBeOfType<NoContentResult>();

        var stored = db.AppRatings.FirstOrDefault(r => r.Id == -2);
        stored.ShouldBeNull();
    }


    private static AdminAppRatingsController CreateAdminController(IServiceScope scope)
    {
        return new AdminAppRatingsController(
            scope.ServiceProvider.GetRequiredService<IAppRatingService>());
    }

    private static AppRatingsController CreateController(IServiceScope scope, string userId, string role)
    {
        var controller = new AppRatingsController(
            scope.ServiceProvider.GetRequiredService<IAppRatingService>());

        controller.ControllerContext = BuildContext(userId, role);
        return controller;
    }

    private static ControllerContext BuildContext(string userId, string role)
    {
        var claims = new[]
        {
            new Claim("id", userId),
            new Claim("role", role)
        };

        var identity = new ClaimsIdentity(claims, "TestAuth");
        var user = new ClaimsPrincipal(identity);

        return new ControllerContext
        {
            HttpContext = new Microsoft.AspNetCore.Http.DefaultHttpContext
            {
                User = user
            }
        };
    }
}
