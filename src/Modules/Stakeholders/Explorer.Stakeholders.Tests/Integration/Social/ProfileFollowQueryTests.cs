using Explorer.API.Controllers.Social;
using Explorer.Stakeholders.API.Dtos.Social;
using Explorer.Stakeholders.API.Public.Social;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Explorer.Stakeholders.Tests.Integration.Social;

[Collection("Sequential")]
public class ProfileFollowQueryTests : BaseStakeholdersIntegrationTest
{
    public ProfileFollowQueryTests(StakeholdersTestFactory factory) : base(factory) { }

    [Fact]
    public async Task Retrieves_follower_list_for_profile()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        // Act
        var result = ((ObjectResult)(await controller.GetFollowers(-11)).Result)?.Value as List<FollowerDto>;

        // Assert
        result.ShouldNotBeEmpty();
        result.Count.ShouldBe(1);
        result[0].FollowerId.ShouldBe(-21);
        result[0].FollowerName.ShouldBe("Pera Perić");
    }

    [Fact]
    public async Task Retrieves_following_list_for_profile()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        // Act
        var result = ((ObjectResult)(await controller.GetFollowing(-12)).Result)?.Value as List<FollowingDto>;

        // Assert
        result.ShouldNotBeEmpty();
        result.Count.ShouldBe(1);
        result[0].FollowingId.ShouldBe(-23);
        result[0].FollowingName.ShouldBe("Steva Stević");
    }

    private static ProfileFollowController CreateController(IServiceScope scope)
    {
        return new ProfileFollowController(scope.ServiceProvider.GetRequiredService<IProfileFollowService>())
        {
            ControllerContext = BuildContext("-1")
        };
    }
}
