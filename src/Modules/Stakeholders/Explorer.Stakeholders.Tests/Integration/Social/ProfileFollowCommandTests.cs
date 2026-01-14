using Explorer.API.Controllers.Social;
using Explorer.Stakeholders.API.Dtos.Social;
using Explorer.Stakeholders.API.Public.Social;
using Explorer.Stakeholders.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Explorer.Stakeholders.Tests.Integration.Social;

[Collection("Sequential")]
public class ProfileFollowCommandTests : BaseStakeholdersIntegrationTest
{
    public ProfileFollowCommandTests(StakeholdersTestFactory factory) : base(factory) { }

    [Fact]
    public void Follow()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();

        // Act
        var result = ((ObjectResult)controller.Follow(-11, -12).Result)?.Value as ProfileFollowDto;

        // Assert - Response
        result.ShouldNotBeNull();
        result.FollowerId.ShouldBe(-11);
        result.FollowingId.ShouldBe(-12);

        // Assert - Database
        var storedFollow = dbContext.ProfileFollows.FirstOrDefault(f => f.FollowerId == -11 && f.FollowingId == -12);
        storedFollow.ShouldNotBeNull();
    }

    [Fact]
    public void Follow_fails_self_follow()
    {
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        var result = controller.Follow(-11, -11).Result;

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        badRequest.Value.ShouldBe("You cannot follow yourself.");
    }

    [Fact]
    public void Follow_fails_relationship_exists()
    {
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        var result = controller.Follow(-21, -11).Result;

        var badRequest = Assert.IsType<ConflictObjectResult>(result);
        badRequest.Value.ShouldBe("This follow relationship already exists");
    }

    [Fact]
    public void Follow_fails_profile_doesnt_exists()
    {
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        var result = controller.Follow(-999, -11).Result;

        var badRequest = Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void Unfollow()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();

        // Act
        var result = controller.Unfollow(-21, -12);

        // Assert - Response
        var request = Assert.IsType<OkResult>(result);

        // Assert - Database
        var storedFollow = dbContext.ProfileFollows.FirstOrDefault(f => f.FollowerId == -21 && f.FollowingId == -12);
        storedFollow.ShouldBeNull();
    }

    [Fact]
    public void Unfollow_fails_relationship_doesnt_exist()
    {
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        var result = controller.Follow(-999, -11).Result;

        var badRequest = Assert.IsType<NotFoundResult>(result);
    }

    private static ProfileFollowController CreateController(IServiceScope scope)
    {
        return new ProfileFollowController(scope.ServiceProvider.GetRequiredService<IProfileFollowService>())
        {
            ControllerContext = BuildContext("-1")
        };
    }
}
