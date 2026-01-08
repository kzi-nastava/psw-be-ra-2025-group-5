using Explorer.API.Controllers;
using Explorer.API.Controllers.Tours.Manual;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Dtos.Tours;
using Explorer.Tours.API.Public;
using Explorer.Tours.API.Public.Tour;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Explorer.Tours.Tests.Integration.TourManual;

[Collection("Sequential")]
public class TourManualQueryTests : BaseToursIntegrationTest
{
    public TourManualQueryTests(ToursTestFactory factory) : base(factory) { }

    [Fact]
    public void Retrieves_seen_manual()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        controller.MarkAsSeen("tour-execution-map");

        // Act
        var result = ((ObjectResult)controller.GetStatus("tour-execution-map").Result)
            ?.Value as TourManualStatusDto;

        // Assert
        result.ShouldNotBeNull();
        result.Seen.ShouldBeTrue();

    }

    [Fact]
    public void Retrieves_not_seen_manual()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        // Act
        var result = ((ObjectResult)controller.GetStatus("some-random-page").Result)
            ?.Value as TourManualStatusDto;

        // Assert
        result.ShouldNotBeNull();
        result.Seen.ShouldBeFalse();
    }

    private static TourManualController CreateController(IServiceScope scope)
    {
        return new TourManualController(
            scope.ServiceProvider.GetRequiredService<ITourManualService>())
        {
            ControllerContext = BuildContext("-23") 
        };
    }
}

