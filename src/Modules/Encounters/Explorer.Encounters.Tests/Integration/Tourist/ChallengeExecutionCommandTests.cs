using Explorer.API.Controllers.Tourist;
using Explorer.BuildingBlocks.Core.Exceptions;
using Explorer.Encounters.API.Dtos;
using Explorer.Encounters.API.Public.Tourist;
using Explorer.Encounters.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Explorer.Encounters.Tests.Integration.Tourist;

[Collection("Sequential")]
public class ChallengeExecutionCommandTests : BaseEncountersIntegrationTest, IDisposable
{
    private readonly IServiceScope _scope;
    private readonly EncountersContext _dbContext;

    public ChallengeExecutionCommandTests(EncountersTestFactory factory) : base(factory)
    {
        _scope = Factory.Services.CreateScope();
        _dbContext = _scope.ServiceProvider.GetRequiredService<EncountersContext>();
    }

    public void Dispose()
    {
        // Cleanup nakon svakog testa
        _dbContext.ChallengeExecutions.RemoveRange(_dbContext.ChallengeExecutions);
        _dbContext.SaveChanges();
        _scope?.Dispose();
    }

    [Fact]
    public void Starts_challenge_execution()
    {
        // Arrange
        var controller = CreateController(_scope);
        long challengeId = -1; // Existing active challenge from b-challenges.sql
        long touristId = -21; // Tourist ID from claims

        // Act
        var result = ((ObjectResult)controller.StartChallenge(challengeId).Result)?.Value as ChallengeExecutionDto;

        // Assert - Response
        result.ShouldNotBeNull();
        result.Id.ShouldNotBe(0);
        result.ChallengeId.ShouldBe(challengeId);
        result.TouristId.ShouldBe(touristId);
        result.Status.ShouldBe("InProgress");
        result.StartedAt.ShouldNotBe(default(DateTime));
        result.CompletedAt.ShouldBeNull();
        result.AbandonedAt.ShouldBeNull();

        // Assert - Database
        var storedEntity = _dbContext.ChallengeExecutions.FirstOrDefault(e => e.Id == result.Id);
        storedEntity.ShouldNotBeNull();
        storedEntity.ChallengeId.ShouldBe(challengeId);
        storedEntity.TouristId.ShouldBe(touristId);
    }

    [Fact]
    public void Start_fails_challenge_not_found()
    {
        // Arrange
        var controller = CreateController(_scope);
        long invalidChallengeId = -9999;

        // Act & Assert
        Should.Throw<KeyNotFoundException>(() => controller.StartChallenge(invalidChallengeId));
    }

    [Fact]
    public void Start_fails_challenge_not_active()
    {
        // Arrange
        var controller = CreateController(_scope);
        long draftChallengeId = -3; // Draft challenge from b-challenges.sql (Status = 0)

        // Act & Assert
        Should.Throw<InvalidOperationException>(() => controller.StartChallenge(draftChallengeId));
    }

    [Fact]
    public void Start_fails_already_active_execution()
    {
        // Arrange
        var controller = CreateController(_scope);
        long challengeId = -1;

        // Act - Start first execution
        controller.StartChallenge(challengeId);

        // Act & Assert - Try to start again
        Should.Throw<InvalidOperationException>(() => controller.StartChallenge(challengeId));
    }

    [Fact]
    public void Completes_challenge_execution()
    {
        // Arrange
        var controller = CreateController(_scope);
        long challengeId = -1;
        
        // Start execution first
        var startResult = ((ObjectResult)controller.StartChallenge(challengeId).Result)?.Value as ChallengeExecutionDto;
        long executionId = startResult!.Id;

        // Act
        var result = ((ObjectResult)controller.CompleteChallenge(executionId).Result)?.Value as ChallengeExecutionDto;

        // Assert - Response
        result.ShouldNotBeNull();
        result.Status.ShouldBe("Completed");
        result.CompletedAt.ShouldNotBeNull();
        result.AbandonedAt.ShouldBeNull();

        // Assert - Database
        var storedEntity = _dbContext.ChallengeExecutions.FirstOrDefault(e => e.Id == executionId);
        storedEntity.ShouldNotBeNull();
        storedEntity.Status.ToString().ShouldBe("Completed");
        storedEntity.CompletedAt.ShouldNotBeNull();
    }

    [Fact]
    public void Complete_fails_unauthorized()
    {
        // Arrange
        var controller = CreateController(_scope);
        var otherTouristController = CreateController(_scope, "-22"); // Different tourist
        long challengeId = -1;

        // Start execution with first tourist
        var startResult = ((ObjectResult)controller.StartChallenge(challengeId).Result)?.Value as ChallengeExecutionDto;
        long executionId = startResult!.Id;

        // Act & Assert - Try to complete with different tourist
        Should.Throw<UnauthorizedAccessException>(() => otherTouristController.CompleteChallenge(executionId));
    }

    [Fact]
    public void Abandons_challenge_execution()
    {
        // Arrange
        var controller = CreateController(_scope);
        long challengeId = -1;

        // Start execution first
        var startResult = ((ObjectResult)controller.StartChallenge(challengeId).Result)?.Value as ChallengeExecutionDto;
        long executionId = startResult!.Id;

        // Act
        var result = ((ObjectResult)controller.AbandonChallenge(executionId).Result)?.Value as ChallengeExecutionDto;

        // Assert - Response
        result.ShouldNotBeNull();
        result.Status.ShouldBe("Abandoned");
        result.AbandonedAt.ShouldNotBeNull();
        result.CompletedAt.ShouldBeNull();

        // Assert - Database
        var storedEntity = _dbContext.ChallengeExecutions.FirstOrDefault(e => e.Id == executionId);
        storedEntity.ShouldNotBeNull();
        storedEntity.Status.ToString().ShouldBe("Abandoned");
        storedEntity.AbandonedAt.ShouldNotBeNull();
    }

    [Fact]
    public void Gets_tourist_executions()
    {
        // Arrange
        var controller = CreateController(_scope);
        long challengeId = -1;

        // Create some executions
        controller.StartChallenge(challengeId);

        // Act
        var result = ((ObjectResult)controller.GetMyExecutions().Result)?.Value as List<ChallengeExecutionDto>;

        // Assert
        result.ShouldNotBeNull();
        result.Count.ShouldBeGreaterThan(0);
        result.ShouldAllBe(e => e.TouristId == -21);
    }

    [Fact]
    public void Social_challenge_completes_for_all_when_enough_participants_in_range()
    {
        // Arrange
        var controller1 = CreateController(_scope, "-21");
        var controller2 = CreateController(_scope, "-22");
        long challengeId = -2;

        var exec1 = ((ObjectResult)controller1.StartChallenge(challengeId).Result)?.Value as ChallengeExecutionDto;
        var exec2 = ((ObjectResult)controller2.StartChallenge(challengeId).Result)?.Value as ChallengeExecutionDto;

        exec1.ShouldNotBeNull();
        exec2.ShouldNotBeNull();

        var service = _scope.ServiceProvider.GetRequiredService<IChallengeExecutionService>();
        service.UpdateTouristLocation(challengeId, exec1.TouristId, 44.815556, 20.460833);
        service.UpdateTouristLocation(challengeId, exec2.TouristId, 44.815556, 20.460833);

        var storedExec1 = _dbContext.ChallengeExecutions.FirstOrDefault(e => e.Id == exec1.Id);
        var storedExec2 = _dbContext.ChallengeExecutions.FirstOrDefault(e => e.Id == exec2.Id);

        storedExec1.ShouldNotBeNull();
        storedExec2.ShouldNotBeNull();

        storedExec1.Status.ToString().ShouldBe("Completed");
        storedExec2.Status.ToString().ShouldBe("Completed");

        storedExec1.CompletedAt.ShouldNotBeNull();
        storedExec2.CompletedAt.ShouldNotBeNull();
    }


    private static ChallengeExecutionController CreateController(IServiceScope scope, string touristId = "-21")
    {
        return new ChallengeExecutionController(
            scope.ServiceProvider.GetRequiredService<IChallengeExecutionService>())
        {
            ControllerContext = BuildContext(touristId)
        };
    }
}

