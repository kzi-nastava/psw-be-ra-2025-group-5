using Explorer.API.Controllers.Notifications;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos.Notifications;
using Explorer.Stakeholders.API.Public.Notifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System.Security.Claims;

namespace Explorer.Stakeholders.Tests.Integration.Notifications;

[Collection("Sequential")]
public class NotificationTests : BaseStakeholdersIntegrationTest
{
    public NotificationTests(StakeholdersTestFactory factory) : base(factory) { }

    [Fact]
    public void Gets_user_notifications()
    {
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope, "-21", "tourist");

        var result = ((ObjectResult)controller.GetMyNotifications(1, 10).Result)?.Value as PagedResult<NotificationDto>;

        result.ShouldNotBeNull();
        result.Results.Count.ShouldBeGreaterThan(0);
        result.Results.ShouldAllBe(n => n.UserId == -21);
    }

    [Fact]
    public void Gets_unread_notifications()
    {
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope, "-11", "author");

        var result = ((ObjectResult)controller.GetMyUnreadNotifications().Result)?.Value as List<NotificationDto>;

        result.ShouldNotBeNull();
        result.ShouldAllBe(n => !n.IsRead);
        result.ShouldAllBe(n => n.UserId == -11);
    }

    [Fact]
    public void Gets_unread_count()
    {
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope, "-11", "author");

        var result = ((ObjectResult)controller.GetMyUnreadCount().Result)?.Value as int?;

        result.ShouldNotBeNull();
        result.Value.ShouldBeGreaterThan(0);
    }

    [Fact]
    public void Marks_notification_as_read()
    {
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope, "-21", "tourist");

        var result = ((ObjectResult)controller.MarkAsRead(-2).Result)?.Value as NotificationDto;

        result.ShouldNotBeNull();
        result.Id.ShouldBe(-2);
        result.IsRead.ShouldBeTrue();
    }

    [Fact]
    public void Cannot_mark_other_users_notification()
    {
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope, "-22", "tourist");

        var result = controller.MarkAsRead(-1);

        result.Result.ShouldBeOfType<ForbidResult>();
    }

    [Fact]
    public void Deletes_notification()
    {
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope, "-11", "author");

        var result = controller.Delete(-3);

        result.ShouldBeOfType<OkResult>();
    }

    private static NotificationController CreateController(IServiceScope scope, string userId, string role)
    {
        var controller = new NotificationController(
            scope.ServiceProvider.GetRequiredService<INotificationService>());

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
