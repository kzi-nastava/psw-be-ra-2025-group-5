using Explorer.API.Controllers.Streak;
using Explorer.Stakeholders.API.Dtos.Streaks;
using Explorer.Stakeholders.API.Public.Streaks;
using Explorer.Stakeholders.Infrastructure.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System.Security.Claims;

namespace Explorer.Stakeholders.Tests.Integration.Streaks
{
    [Collection("Sequential")]
    public class StreakTests : BaseStakeholdersIntegrationTest
    {
        public StreakTests(StakeholdersTestFactory factory) : base(factory) { }

        [Fact]
        public void First_activity_creates_streak()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope, userId: -21);

            // Act
            controller.RecordActivity();

            var result = controller.GetActivity() as OkObjectResult;
            var streak = result!.Value as StreakDto;

            // Assert
            streak.ShouldNotBeNull();
            streak.UserId.ShouldBe(-21);
            streak.CurrentStreak.ShouldBe(1);
            streak.LongestStreak.ShouldBe(1);
        }

        [Fact]
        public void Multiple_activities_same_day_do_not_increase_streak()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope, userId: -23);

            // Act
            controller.RecordActivity();
            controller.RecordActivity();

            var result = controller.GetActivity() as OkObjectResult;
            var streak = result!.Value as StreakDto;

            // Assert
            streak.ShouldNotBeNull();
            streak.CurrentStreak.ShouldBe(1);
            streak.LongestStreak.ShouldBe(1);
        }

        [Fact]
        public void User_can_access_only_own_streak()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope, userId: -24);

            // Act
            controller.RecordActivity();
            var result = controller.GetActivity() as OkObjectResult;
            var streak = result!.Value as StreakDto;

            // Assert
            streak.UserId.ShouldBe(-24);
        }

        private static StreakController CreateController(IServiceScope scope, long userId)
        {
            var controller = new StreakController(
                scope.ServiceProvider.GetRequiredService<IStreakService>()
            );

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(
                        new[]
                        {
                            new Claim("id", userId.ToString()),
                            new Claim(ClaimTypes.Role, "tourist")
                        },
                        "TestAuth"
                    ))
                }
            };

            return controller;
        }
    }
}
