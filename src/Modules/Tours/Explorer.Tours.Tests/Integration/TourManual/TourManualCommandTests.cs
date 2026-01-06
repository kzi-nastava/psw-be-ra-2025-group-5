using Explorer.API.Controllers;
using Explorer.API.Controllers.Tours.Manual;
using Explorer.BuildingBlocks.Core.Exceptions;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public;
using Explorer.Tours.API.Public.Tour;
using Explorer.Tours.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Explorer.Tours.Tests.Integration.TourManual;

[Collection("Sequential")]
public class TourManualCommandTests : BaseToursIntegrationTest
{
    public TourManualCommandTests(ToursTestFactory factory) : base(factory) { }

    [Fact]
    public void Marks_manual_as_seen()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();

        // Act
        var result = controller.MarkAsSeen("tour-execution-map");

        // Assert - response
        result.ShouldBeOfType<OkResult>();

        // Assert - database
        var progress = dbContext.TourManualProgress
            .Single(p => p.UserId == -21 && p.PageKey == "tour-execution-map");

        progress.Seen.ShouldBeTrue();
    }

    [Fact]
    public void Creates_entry_if_not_existing()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();

        // Act
        var result = controller.MarkAsSeen("new-page-key");

        // Assert
        result.ShouldBeOfType<OkResult>();

        var progress = dbContext.TourManualProgress
            .Single(p => p.UserId == -21 && p.PageKey == "new-page-key");

        progress.Seen.ShouldBeTrue();
    }

    private static TourManualController CreateController(IServiceScope scope)
    {
        return new TourManualController(scope.ServiceProvider.GetRequiredService<ITourManualService>())
        {
            ControllerContext = BuildContext("-21") 
        };
    }
}

