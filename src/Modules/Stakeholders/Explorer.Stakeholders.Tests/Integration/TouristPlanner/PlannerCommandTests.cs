using Explorer.API.Controllers.TouristPlanner;
using Explorer.Stakeholders.API.Dtos.TouristPlanner;
using Explorer.Stakeholders.API.Public.TouristPlanner;
using Explorer.Stakeholders.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Explorer.Stakeholders.Tests.Integration.TouristPlanner;

[Collection("Sequential")]
public class PlannerCommandTests : BaseStakeholdersIntegrationTest
{
    public PlannerCommandTests(StakeholdersTestFactory factory) : base(factory) { }

    [Fact]
    public void Adds_block_to_new_day()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();
        var date = new DateOnly(2026, 3, 10);
        var newBlock = new CreatePlannerTimeBlockDto
        {
            TourId = -1,
            StartTime = new TimeOnly(10, 0),
            EndTime = new TimeOnly(12, 0),
            Duration = 0
        };

        // Act
        var result = ((ObjectResult)controller.AddBlock(-21, date, newBlock).Result)?.Value as PlannerDayDto;

        // Assert - Response
        result.ShouldNotBeNull();
        result.Date.ShouldBe(date);
        result.TimeBlocks.ShouldNotBeEmpty();
        result.TimeBlocks.Any(b => b.TourId == -1).ShouldBeTrue();

        // Assert - Database
        var planner = dbContext.Planners.FirstOrDefault(p => p.TouristId == -21);
        planner.ShouldNotBeNull();
        var day = planner.Days.FirstOrDefault(d => d.Date == date);
        day.ShouldNotBeNull();
        day.TimeBlocks.Any(b => b.TourId == -1).ShouldBeTrue();
    }

    [Fact]
    public void Adds_block_to_existing_day()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();
        var date = new DateOnly(2026, 1, 15);
        var newBlock = new CreatePlannerTimeBlockDto
        {
            TourId = -2,
            StartTime = new TimeOnly(23, 0),
            EndTime = new TimeOnly(23, 10),
            Duration = 0
        };

        // Act
        var result = ((ObjectResult)controller.AddBlock(-21, date, newBlock).Result)?.Value as PlannerDayDto;

        // Assert - Response
        result.ShouldNotBeNull();
        result.TimeBlocks.Any(b => b.TourId == -2).ShouldBeTrue();

        // Assert - Database
        var planner = dbContext.Planners.FirstOrDefault(p => p.TouristId == -21);
        var day = planner.Days.FirstOrDefault(d => d.Date == date);
        day.TimeBlocks.Any(b => b.TourId == -2).ShouldBeTrue();
    }

    [Fact]
    public void Adds_block_with_auto_scheduling()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var date = new DateOnly(2026, 2, 11);
        var newBlock = new CreatePlannerTimeBlockDto
        {
            TourId = -1,
            StartTime = new TimeOnly(0, 0),
            EndTime = new TimeOnly(0, 0),
            Duration = 120
        };

        // Act
        var result = ((ObjectResult)controller.AddBlock(-21, date, newBlock).Result)?.Value as PlannerDayDto;

        // Assert
        result.ShouldNotBeNull();
        var addedBlock = result.TimeBlocks.First(b => b.TourId == -1);
        addedBlock.EndTime.ShouldNotBe(new TimeOnly(0, 0));
        var duration = addedBlock.EndTime - addedBlock.StartTime;
        duration.TotalMinutes.ShouldBe(120);
    }

    [Fact]
    public void Add_block_fails_overlap()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var date = new DateOnly(2026, 1, 15);
        var overlappingBlock = new CreatePlannerTimeBlockDto
        {
            TourId = -3,
            StartTime = new TimeOnly(9, 0),
            EndTime = new TimeOnly(11, 0),
            Duration = 0
        };

        // Act & Assert
        var result = controller.AddBlock(-21, date, overlappingBlock).Result;
        result.ShouldBeOfType<ConflictObjectResult>();
    }

    [Fact]
    public void Reschedules_block()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();
        var date = new DateOnly(2026, 1, 15);
        var blockId = -1;
        var rescheduledBlock = new CreatePlannerTimeBlockDto
        {
            TourId = -2,
            StartTime = new TimeOnly(17, 0),
            EndTime = new TimeOnly(19, 0),
            Duration = 0
        };

        // Act
        var result = ((ObjectResult)controller.RescheduleBlock(-21, date, blockId, rescheduledBlock).Result)?.Value as PlannerDayDto;

        // Assert - Response
        result.ShouldNotBeNull();
        var updatedBlock = result.TimeBlocks.First(b => b.Id == blockId);
        updatedBlock.StartTime.ShouldBe(new TimeOnly(17, 0));
        updatedBlock.EndTime.ShouldBe(new TimeOnly(19, 0));

        // Assert - Database
        var planner = dbContext.Planners.FirstOrDefault(p => p.TouristId == -21);
        var day = planner.Days.FirstOrDefault(d => d.Date == date);
        var storedBlock = day.TimeBlocks.First(b => b.Id == blockId);
        storedBlock.TimeRange.Start.ShouldBe(new TimeOnly(17, 0));
        storedBlock.TimeRange.End.ShouldBe(new TimeOnly(19, 0));
    }

    [Fact]
    public void Reschedule_creates_block_if_day_not_exists()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var date = new DateOnly(2026, 3, 1);
        var blockId = -999;
        var newBlock = new CreatePlannerTimeBlockDto
        {
            TourId = -1,
            StartTime = new TimeOnly(10, 0),
            EndTime = new TimeOnly(12, 0),
            Duration = 0
        };

        // Act
        var result = ((ObjectResult)controller.RescheduleBlock(-21, date, blockId, newBlock).Result)?.Value as PlannerDayDto;

        // Assert
        result.ShouldNotBeNull();
        result.TimeBlocks.ShouldNotBeEmpty();
    }

    [Fact]
    public void Reschedule_fails_overlap()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var date = new DateOnly(2026, 1, 16);
        var blockId = -1;
        var overlappingBlock = new CreatePlannerTimeBlockDto
        {
            TourId = -2,
            StartTime = new TimeOnly(20, 0),
            EndTime = new TimeOnly(23, 0),
            Duration = 0
        };

        // Act & Assert
        var result = controller.RescheduleBlock(-21, date, blockId, overlappingBlock).Result;
        result.ShouldBeOfType<ConflictObjectResult>();
    }

    [Fact]
    public void Removes_block()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();
        var date = new DateOnly(2026, 1, 16);
        var blockId = -2;

        // Act
        var result = (OkResult)controller.RemoveBlock(-21, date, blockId);

        // Assert - Response
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(200);

        // Assert - Database
        var planner = dbContext.Planners.FirstOrDefault(p => p.TouristId == -21);
        var day = planner.Days.FirstOrDefault(d => d.Date == date);
        day.TimeBlocks.Any(b => b.Id == blockId).ShouldBeFalse();
    }

    [Fact]
    public void Remove_fails_day_not_found()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var date = new DateOnly(2099, 12, 31);
        var blockId = -1;

        // Act & Assert
        var result = controller.RemoveBlock(-21, date, blockId);
        result.ShouldBeOfType<ConflictObjectResult>();
    }

    [Fact]
    public void Remove_fails_block_not_found()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var date = new DateOnly(2026, 1, 16);
        var blockId = -999;

        // Act & Assert
        var result = controller.RemoveBlock(-21, date, blockId);
        result.ShouldBeOfType<ConflictObjectResult>();
    }

    private static PlannerController CreateController(IServiceScope scope)
    {
        return new PlannerController(scope.ServiceProvider.GetRequiredService<IPlannerService>())
        {
            ControllerContext = BuildContext("-1")
        };
    }
}