using Explorer.API.Controllers.Tourist;
using Explorer.Payments.Core.Domain;
using Explorer.Payments.Infrastructure.Database;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System.Linq;
using Xunit;

namespace Explorer.Tours.Tests.Integration.TourExe;

[Collection("Sequential")]
public class TourExecutionCommandTests : BaseToursIntegrationTest
{
    public TourExecutionCommandTests(ToursTestFactory factory) : base(factory) { }

    private Tour CreateTestTour(ToursContext dbContext)
    {
        var tour = new Tour(
            authorId: 1,
            name: "Test Tour",
            description: "Opis test ture",
            difficulty: Explorer.Tours.Core.Domain.TourDifficulty.Easy,
            tags: new List<string> { "test" },
            price: 10.0
        );

        var location = new Explorer.Tours.Core.Domain.Location(44.818, 20.456);
        tour.AddKeyPoint("Start", "Prva ključna tačka", location, null, null);
        tour.AddKeyPoint("End", "Druga ključna tačka", location, null, null);

        dbContext.Tours.Add(tour);
        dbContext.SaveChanges();

        return tour;
    }

    private TourExecution CreateTestExecution(ToursContext dbContext, long userId, Tour tour)
    {
        var execution = TourExecution.StartNew(userId, tour.Id);
        dbContext.TourExecutions.Add(execution);
        dbContext.SaveChanges();
        return execution;
    }

    [Fact]
    public void StartsExecution_NewExecution_CreatesSession()
    {
        using var scope = Factory.Services.CreateScope();
        var tourDbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();
        var paymentDbContext = scope.ServiceProvider.GetRequiredService<PaymentsContext>();
        var controller = CreateController(scope);

        // Arrange
        var tour = CreateTestTour(tourDbContext);
        long userId = -1;

        paymentDbContext.TourPurchaseTokens.Add(new TourPurchaseToken(tour.Id, -1));
        paymentDbContext.SaveChanges();

        // Act
        var result = ((ObjectResult)controller.Start(tour.Id).Result)?.Value as StartExecutionResultDto;

        // Assert
        result.ShouldNotBeNull();
        result.ExecutionId.ShouldNotBe(0);
        result.StartTime.ShouldNotBe(default);
        result.NextKeyPoint.ShouldNotBeNull();

        var stored = tourDbContext.TourExecutions.First(e => e.Id == result.ExecutionId);
        stored.ShouldNotBeNull();
        stored.UserId.ShouldBe(userId);
        stored.TourId.ShouldBe(tour.Id);
        stored.Status.ShouldBe(TourExecutionStatus.Active);
    }

    [Fact]
    public void CheckProximity_CompletesKeyPoint_WhenNear()
    {
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();
        var controller = CreateController(scope);

        // Arrange
        var tour = CreateTestTour(dbContext);
        var execution = CreateTestExecution(dbContext, -1, tour);

        var keyPoint = tour.KeyPoints.First();
        var location = new LocationDto
        {
            Latitude = keyPoint.Location.Latitude,
            Longitude = keyPoint.Location.Longitude
        };

        // Act
        var result = ((ObjectResult)controller.CheckProximity(execution.Id, location).Result)?.Value as CheckProximityDto;

        // Assert
        result.ShouldNotBeNull();
        result.IsNearKeyPoint.ShouldBeTrue();
        result.CompletedKeyPointId.ShouldBe(keyPoint.Id);
        result.NextKeyPoint.ShouldNotBeNull();
        result.PercentCompleted.ShouldBeGreaterThan(0);
    }

    [Fact]
    public void CompleteExecution_MarksTourCompleted()
    {
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();
        var controller = CreateController(scope);

        // Arrange
        var tour = CreateTestTour(dbContext);
        var execution = CreateTestExecution(dbContext, -1, tour);

        // Act
        var result = controller.Complete(execution.Id);

        // Assert
        result.ShouldBeOfType<OkResult>();
        var stored = dbContext.TourExecutions.First(e => e.Id == execution.Id);
        stored.Status.ShouldBe(TourExecutionStatus.Completed);
        stored.EndTime.ShouldNotBeNull();
    }

    [Fact]
    public void AbandonExecution_MarksTourAbandoned()
    {
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();
        var controller = CreateController(scope);

        // Arrange
        var tour = CreateTestTour(dbContext);
        var execution = CreateTestExecution(dbContext, -1, tour);

        // Act
        var result = controller.Abandon(execution.Id);

        // Assert
        result.ShouldBeOfType<OkResult>();
        var stored = dbContext.TourExecutions.First(e => e.Id == execution.Id);
        stored.Status.ShouldBe(TourExecutionStatus.Abandoned);
        stored.EndTime.ShouldNotBeNull();
    }

    [Fact]
    public void StartExecution_Fails_When_Tour_Not_Purchased()
    {
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();
        var controller = CreateController(scope);

        // Arrange
        var tour = CreateTestTour(dbContext);
        var countBefore = dbContext.TourExecutions.Count();

        // Act
        Should.Throw<InvalidOperationException>(() =>
            controller.Start(tour.Id)
        );

        // Assert
        dbContext.TourExecutions.Count().ShouldBe(countBefore);
    }

    private static TourExecutionController CreateController(IServiceScope scope)
    {
        return new TourExecutionController(scope.ServiceProvider.GetRequiredService<ITourExecutionService>())
        {
            ControllerContext = BuildContext("-1")
        };
    }

    private void CreatePurchaseToken(PaymentsContext dbContext, long tourId, long touristId)
    {
        dbContext.TourPurchaseTokens.Add(new TourPurchaseToken(tourId, touristId));
        dbContext.SaveChanges();
    }
}
