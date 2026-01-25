using Explorer.API.Controllers.Social;
using Explorer.BuildingBlocks.Core.FileStorage;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Dtos.Social;
using Explorer.Stakeholders.API.Internal;
using Explorer.Stakeholders.API.Public.Social;
using Explorer.Stakeholders.API.Public.Statistics;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces.Social;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces.Users;
using Explorer.Stakeholders.Core.UseCases.Administration.Social;
using Explorer.Stakeholders.Core.UseCases.Administration.Users;
using Explorer.Stakeholders.Core.UseCases.Statistics;
using Explorer.Stakeholders.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using System.Collections.Generic;

namespace Explorer.Stakeholders.Tests.Integration.Social;

[Collection("Sequential")]
public class ProfileFollowCommandTests : BaseStakeholdersIntegrationTest
{
    public ProfileFollowCommandTests(StakeholdersTestFactory factory) : base(factory) { }

    [Fact]
    public void Follow()
    {
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();

        var result = ((ObjectResult)controller.Follow(-11, -12).Result)?.Value as ProfileFollowDto;

        result.ShouldNotBeNull();
        result.FollowerId.ShouldBe(-11);
        result.FollowingId.ShouldBe(-12);

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

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void Unfollow()
    {
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();

        var result = controller.Unfollow(-21, -12);

        Assert.IsType<OkResult>(result);

        var storedFollow = dbContext.ProfileFollows.FirstOrDefault(f => f.FollowerId == -21 && f.FollowingId == -12);
        storedFollow.ShouldBeNull();
    }

    [Fact]
    public void Unfollow_fails_relationship_doesnt_exist()
    {
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        var result = controller.Unfollow(-999, -11);

        var notFound = Assert.IsType<NotFoundObjectResult>(result);
    }


    private static ProfileFollowController CreateController(IServiceScope scope)
    {
        var profileService = new ProfileService(
            scope.ServiceProvider.GetRequiredService<IPersonRepository>(),
            scope.ServiceProvider.GetRequiredService<AutoMapper.IMapper>(),
            new StubTouristStatisticsService(),
            scope.ServiceProvider.GetRequiredService<IImageStorage>(),
            scope.ServiceProvider.GetRequiredService<IUserRepository>()
        );

        var profileFollowService = new ProfileFollowService(
            scope.ServiceProvider.GetRequiredService<IProfileFollowRepository>(),
            profileService,
            scope.ServiceProvider.GetRequiredService<AutoMapper.IMapper>()
        );

        return new ProfileFollowController(profileFollowService)
        {
            ControllerContext = BuildContext("-1")
        };
    }

    public class StubTouristStatisticsService : ITouristStatisticsService
    {
        public TouristStatisticsDto GetStatistics(long userId)
        {
            return new TouristStatisticsDto
            {
                PurchasedToursCount = 5,
                CompletedToursCount = 3,
                MostCommonTag = "Adventure",
                MostCommonDifficulty = "Medium"
            };
        }
    }

    public class StubPremiumService : IPremiumSharedService
    {
        public bool IsPremium(long userId) => true;
        public void GrantPremium(long userId, DateTime validUntil) { }
        public void ExtendPremium(long userId, DateTime validUntil) { }
        public void RemovePremium(long userId) { }
    }
}
