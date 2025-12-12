using Explorer.API.Controllers.Tourist.ProblemReporting;
using Explorer.BuildingBlocks.Core.Exceptions;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public.Reporting;
using Explorer.Stakeholders.Infrastructure.Database;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using DomainProblemPriority = Explorer.Stakeholders.Core.Domain.ProblemPriority;

namespace Explorer.Stakeholders.Tests.Integration.Reporting;

[Collection("Sequential")]
public class TourProblemCommandTests : BaseStakeholdersIntegrationTest
{
    public TourProblemCommandTests(StakeholdersTestFactory factory) : base(factory) { }

    [Fact]
    public void Creates()
    {
      // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();
        var newEntity = new TourProblemDto
        {
            Id = -999,
            TourId = 111,
            ReporterId = -21,
            Category = ProblemCategory.Safety,
            Priority = ProblemPriority.High,
            Description = "Problem sa bezbednošću na turi",
            OccurredAt = DateTimeOffset.UtcNow,
            CreatedAt = DateTimeOffset.UtcNow,
            IsResolved = false
        };

        // Act
        var result = ((ObjectResult)controller.Create(newEntity).Result)?.Value as TourProblemDto;

   // Assert - Response
        result.ShouldNotBeNull();
        result.Id.ShouldNotBe(0);
        result.Description.ShouldBe(newEntity.Description);
        result.Category.ShouldBe(newEntity.Category);
        result.Priority.ShouldBe(newEntity.Priority);

        // Assert - Database
        var storedEntity = dbContext.TourProblems.FirstOrDefault(i => i.Description == newEntity.Description);
        storedEntity.ShouldNotBeNull();
        storedEntity.Id.ShouldBe(result.Id);
    }

    [Fact]
    public void Create_fails_invalid_data()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var updatedEntity = new TourProblemDto
        {
      Description = "Test"
     // Missing required fields: TourId, ReporterId, Category, Priority, OccurredAt
     };

        // Act & Assert
   Should.Throw<ArgumentException>(() => controller.Create(updatedEntity));
    }

    [Fact]
    public void Updates()
    {
     // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();
        var updatedEntity = new TourProblemDto
      {
            Id = -11,
            TourId = 1,
            ReporterId = -21,
            Category = ProblemCategory.Safety,
            Priority = ProblemPriority.Critical,
            Description = "Ažurirani problem bezbednosti",
            OccurredAt = DateTimeOffset.UtcNow,
            CreatedAt = DateTimeOffset.UtcNow,
            IsResolved = false
        };

      // Act
        var result = ((ObjectResult)controller.Update(updatedEntity).Result)?.Value as TourProblemDto;

     // Assert - Response
        result.ShouldNotBeNull();
        result.Id.ShouldBe(-11);
        result.Description.ShouldBe(updatedEntity.Description);
        result.Priority.ShouldBe(updatedEntity.Priority);

      // Assert - Database
        var storedEntity = dbContext.TourProblems.FirstOrDefault(i => i.Id == -11);
        storedEntity.ShouldNotBeNull();
        storedEntity.Description.ShouldBe(updatedEntity.Description);
        storedEntity.Priority.ShouldBe(DomainProblemPriority.Critical);
    }

    [Fact]
    public void Update_fails_invalid_id()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var updatedEntity = new TourProblemDto
        {
            Id = -1000,
            TourId = 1,
            ReporterId = 1,
            Category = ProblemCategory.Safety,
            Priority = ProblemPriority.Low,
            Description = "Test",
            OccurredAt = DateTimeOffset.UtcNow,
            CreatedAt = DateTimeOffset.UtcNow,
            IsResolved = false
        };

// Act & Assert
   Should.Throw<NotFoundException>(() => controller.Update(updatedEntity));
    }

    [Fact]
    public void Deletes()
    {
    // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();

        // Act
        var result = (OkResult)controller.Delete(-33);

        // Assert - Response
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(200);

        // Assert - Database
        var storedEntity = dbContext.TourProblems.FirstOrDefault(i => i.Id == -33);
        storedEntity.ShouldBeNull();
    }

    [Fact]
    public void Delete_fails_invalid_id()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        // Act & Assert
        Should.Throw<NotFoundException>(() => controller.Delete(-1000));
    }

    private static TourProblemController CreateController(IServiceScope scope)
    {
     return new TourProblemController(scope.ServiceProvider.GetRequiredService<ITourProblemService>(), scope.ServiceProvider.GetRequiredService<ITourRepository>())
        {
        ControllerContext = BuildContext("-21")
        };
    }
}