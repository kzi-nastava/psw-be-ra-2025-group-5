using Explorer.API.Controllers.Tours.Execution;
using Explorer.Tours.API.Dtos.Tours.Executions;
using Explorer.Tours.API.Public.Tour;
using Explorer.Tours.Core.Domain.TourExecutions;
using Explorer.Tours.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Explorer.Tours.Tests.Integration.TourExe;

[Collection("Sequential")]
public class TourExecutionQueryTests : BaseToursIntegrationTest
{
    public TourExecutionQueryTests(ToursTestFactory factory) : base(factory) { }

    [Fact]
    public void GetExecution_ReturnsExecution()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<ITourExecutionService>();
        var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();

        var testExecution = TourExecution.StartNew(userId: -1, tourId: -1);
        dbContext.TourExecutions.Add(testExecution);
        dbContext.SaveChanges();

        var controller = CreateController(scope);
        long executionId = testExecution.Id; 

        // Act
        var result = ((ObjectResult)controller.Get(executionId).Result)?.Value as TourExecutionDto;

        // Assert
        result.ShouldNotBeNull();
        result.Id.ShouldBe(executionId);
        result.UserId.ShouldBe(-1);
        result.TourId.ShouldBe(-1);
        result.Status.ShouldBe("Active");
    }


    [Fact]
    public void GetExecution_ReturnsNotFound_ForInvalidId()
    {
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        long invalidId = 999;

        var result = controller.Get(invalidId).Result;

        result.ShouldBeOfType<NotFoundResult>();
    }

    [Fact]
    public void GetForUser_ReturnsOnlyExecutionsForLoggedInUser()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();

        dbContext.TourExecutions.RemoveRange(dbContext.TourExecutions);
        dbContext.SaveChanges();

        var execution1 = TourExecution.StartNew(userId: -1, tourId: 1);
        var execution2 = TourExecution.StartNew(userId: -1, tourId: 2);
        var executionOtherUser = TourExecution.StartNew(userId: -2, tourId: 3);

        dbContext.TourExecutions.AddRange(execution1, execution2, executionOtherUser);
        dbContext.SaveChanges();

        var controller = CreateController(scope);

        // Act
        var result = ((ObjectResult)controller.GetForUser().Result)?.Value as List<TourExecutionDto>;

        // Assert
        result.ShouldNotBeNull();
        result.Count.ShouldBe(2);

        result.All(e => e.UserId == -1).ShouldBeTrue();
    }


    private static TourExecutionController CreateController(IServiceScope scope)
    {
        return new TourExecutionController(scope.ServiceProvider.GetRequiredService<ITourExecutionService>())
        {
            ControllerContext = BuildContext("-1")
        };
    }
}

