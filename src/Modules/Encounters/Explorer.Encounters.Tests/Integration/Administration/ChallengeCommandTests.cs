using Explorer.API.Controllers.Administrator.Administration;
using Explorer.BuildingBlocks.Core.Exceptions;
using Explorer.Encounters.API.Dtos;
using Explorer.Encounters.API.Public.Administration;
using Explorer.Encounters.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Explorer.Encounters.Tests.Integration.Administration;

[Collection("Sequential")]
public class ChallengeCommandTests : BaseEncountersIntegrationTest
{
    public ChallengeCommandTests(EncountersTestFactory factory) : base(factory) { }

    [Fact]
    public void Creates()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<EncountersContext>();
        var newEntity = new ChallengeDto
        {
            Name = "Novi izazov",
            Description = "Opis novog izazova koji treba da se izvrši.",
            Latitude = 44.8125,
            Longitude = 20.4612,
            ExperiencePoints = 250,
            Status = "Active",
            Type = "Location"
        };

        // Act
        var result = ((ObjectResult)controller.Create(newEntity).Result)?.Value as ChallengeDto;

        // Assert - Response
        result.ShouldNotBeNull();
        result.Id.ShouldNotBe(0);
        result.Name.ShouldBe(newEntity.Name);
        result.ExperiencePoints.ShouldBe(newEntity.ExperiencePoints);
        result.Status.ShouldBe(newEntity.Status);
        result.Type.ShouldBe(newEntity.Type);

        // Assert - Database
        var storedEntity = dbContext.Challenges.FirstOrDefault(i => i.Name == newEntity.Name);
        storedEntity.ShouldNotBeNull();
        storedEntity.Id.ShouldBe(result.Id);
        storedEntity.ExperiencePoints.ShouldBe(newEntity.ExperiencePoints);
    }

    [Fact]
    public void Create_fails_invalid_name()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var invalidEntity = new ChallengeDto
        {
            Name = "",
            Description = "Opis",
            Latitude = 44.8125,
            Longitude = 20.4612,
            ExperiencePoints = 100,
            Status = "Active",
            Type = "Location"
        };

        // Act & Assert
        Should.Throw<ArgumentException>(() => controller.Create(invalidEntity));
    }

    [Fact]
    public void Create_fails_invalid_coordinates()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var invalidEntity = new ChallengeDto
        {
            Name = "Test izazov",
            Description = "Opis",
            Latitude = 100.0, // Invalid latitude
            Longitude = 20.4612,
            ExperiencePoints = 100,
            Status = "Active",
            Type = "Location"
        };

        // Act & Assert
        Should.Throw<ArgumentException>(() => controller.Create(invalidEntity));
    }

    [Fact]
    public void Create_fails_negative_experience_points()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var invalidEntity = new ChallengeDto
        {
            Name = "Test izazov",
            Description = "Opis",
            Latitude = 44.8125,
            Longitude = 20.4612,
            ExperiencePoints = -50, // Invalid negative XP
            Status = "Active",
            Type = "Location"
        };

        // Act & Assert
        Should.Throw<ArgumentException>(() => controller.Create(invalidEntity));
    }

    [Fact]
    public void Updates()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<EncountersContext>();
        var updatedEntity = new ChallengeDto
        {
            Id = -1,
            Name = "Ažurirani naziv izazova",
            Description = "Ažuriran opis izazova sa novim detaljima.",
            Latitude = 44.825,
            Longitude = 20.455,
            ExperiencePoints = 150,
            Status = "Active",
            Type = "Location"
        };

        // Act
        var result = ((ObjectResult)controller.Update(updatedEntity).Result)?.Value as ChallengeDto;

        // Assert - Response
        result.ShouldNotBeNull();
        result.Id.ShouldBe(-1);
        result.Name.ShouldBe(updatedEntity.Name);
        result.Description.ShouldBe(updatedEntity.Description);
        result.ExperiencePoints.ShouldBe(updatedEntity.ExperiencePoints);

        // Assert - Database
        var storedEntity = dbContext.Challenges.FirstOrDefault(i => i.Id == -1);
        storedEntity.ShouldNotBeNull();
        storedEntity.Name.ShouldBe(updatedEntity.Name);
        storedEntity.Description.ShouldBe(updatedEntity.Description);
        
        var oldEntity = dbContext.Challenges.FirstOrDefault(i => i.Name == "Beogradska tvr?ava");
        oldEntity.ShouldBeNull();
    }

    [Fact]
    public void Update_fails_invalid_id()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var updatedEntity = new ChallengeDto
        {
            Id = -1000,
            Name = "Test",
            Description = "Test opis",
            Latitude = 44.8125,
            Longitude = 20.4612,
            ExperiencePoints = 100,
            Status = "Active",
            Type = "Location"
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
        var dbContext = scope.ServiceProvider.GetRequiredService<EncountersContext>();

        // Act
        var result = (OkResult)controller.Delete(-3);

        // Assert - Response
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(200);

        // Assert - Database
        var storedEntity = dbContext.Challenges.FirstOrDefault(i => i.Id == -3);
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

    [Fact]
    public void GetPaged_returns_challenges()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        // Act
        var result = ((ObjectResult)controller.GetAll(0, 0).Result)?.Value as BuildingBlocks.Core.UseCases.PagedResult<ChallengeDto>;

        // Assert
        result.ShouldNotBeNull();
        result.Results.Count.ShouldBeGreaterThan(0);
        result.TotalCount.ShouldBeGreaterThan(0);
    }

    [Fact]
    public void ApproveChallenge_should_set_status_to_active()
    {
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        controller.ApproveChallenge(-5);

        var db = scope.ServiceProvider.GetRequiredService<EncountersContext>();
        var challenge = db.Challenges.First(c => c.Id == -5);
        challenge.Status.ToString().ShouldBe("Active");
    }

    [Fact]
    public void RejectChallenge_should_set_status_to_archived()
    {
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        controller.RejectChallenge(-4);

        var db = scope.ServiceProvider.GetRequiredService<EncountersContext>();
        var challenge = db.Challenges.First(c => c.Id == -4);
        challenge.Status.ToString().ShouldBe("Archived");
    }



    private static ChallengeController CreateController(IServiceScope scope)
    {
        return new ChallengeController(scope.ServiceProvider.GetRequiredService<IChallengeService>())
        {
            ControllerContext = BuildContext("-1")
        };
    }
}
