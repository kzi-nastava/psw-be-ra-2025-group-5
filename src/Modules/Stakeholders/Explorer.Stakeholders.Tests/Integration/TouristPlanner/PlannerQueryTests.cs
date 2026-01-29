using Explorer.API.Controllers.TouristPlanner;
using Explorer.Stakeholders.API.Dtos.TouristPlanner;
using Explorer.Stakeholders.API.Public.TouristPlanner;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Explorer.Stakeholders.Tests.Integration.TouristPlanner;

[Collection("Sequential")]
public class PlannerQueryTests : BaseStakeholdersIntegrationTest
{
    public PlannerQueryTests(StakeholdersTestFactory factory) : base(factory) { }

    [Fact]
    public void Retrieves_or_creates_planner()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        // Act
        var result = ((ObjectResult)controller.GetPlanner(-21).Result)?.Value as PlannerDto;

        // Assert
        result.ShouldNotBeNull();
        result.TouristId.ShouldBe(-21);
        result.Days.ShouldNotBeNull();
    }

    [Fact]
    public void Retrieves_day_with_blocks()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var date = new DateOnly(2026, 1, 15);

        // Act
        var result = ((ObjectResult)controller.GetDay(-21, date).Result)?.Value as PlannerDayDto;

        // Assert
        result.ShouldNotBeNull();
        result.Date.ShouldBe(date);
        result.TimeBlocks.ShouldNotBeNull();
        result.TimeBlocks.Count.ShouldBeGreaterThan(0);
    }

    [Fact]
    public void Get_day_fails_day_not_found()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var date = new DateOnly(2099, 12, 31);

        // Act
        var result = controller.GetDay(-21, date).Result;

        // Assert
        result.ShouldBeOfType<NotFoundObjectResult>();
    }

    private static PlannerController CreateController(IServiceScope scope)
    {
        return new PlannerController(
            scope.ServiceProvider.GetRequiredService<IPlannerService>(),
            scope.ServiceProvider.GetRequiredService<IPlannerOptimizationService>())
        {
            ControllerContext = BuildContext("-1")
        };
    }
}
