using Explorer.API.Controllers.Administrator.Administration;
using Explorer.BuildingBlocks.Core.UseCases;
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
    public void Retrieves_all()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        // Act
        var result = ((ObjectResult)controller.GetAll(0, 0).Result)?.Value as PagedResult<ChallengeResponseDto>;

        // Assert
        result.ShouldNotBeNull();
        result.Results.ShouldNotBeNull();
    }

    [Fact]
    public void Creates()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<EncountersContext>();
        var newEntity = new ChallengeCreateDto
        {
            Name = "New Challenge",
            Description = "Challenge Description",
            Latitude = 45.2,
            Longitude = 19.8,
            ExperiencePoints = 20,
            Status = "Draft",
            Type = "Social",
            CreatedById = -1,
            RequiredParticipants = 5,
            RadiusInMeters = 50,
            Image = null
        };

        // Act
        var result = ((ObjectResult)controller.Create(newEntity).Result)?.Value as ChallengeResponseDto;

        // Assert - Response
        result.ShouldNotBeNull();
        result.Id.ShouldNotBe(0);
        result.Name.ShouldBe(newEntity.Name);
        result.Status.ShouldBe(newEntity.Status);

        // Assert - Database
        var storedEntity = dbContext.Challenges.FirstOrDefault(c => c.Name == newEntity.Name);
        storedEntity.ShouldNotBeNull();
        storedEntity.Id.ShouldBe(result.Id);
    }

    [Fact]
    public void Updates()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<EncountersContext>();

        // Create initial challenge
        var createDto = new ChallengeCreateDto
        {
            Name = "Challenge To Update",
            Description = "Initial Description",
            Latitude = 45.0,
            Longitude = 19.0,
            ExperiencePoints = 10,
            Status = "Draft",
            Type = "Social",
            CreatedById = -1,
            RequiredParticipants = 5,
            RadiusInMeters = 10
        };
        var createResult = ((ObjectResult)controller.Create(createDto).Result)?.Value as ChallengeResponseDto;

        var updateDto = new ChallengeCreateDto
        {
            Id = createResult.Id,
            Name = "Updated Challenge Name",
            Description = "Updated Description",
            Latitude = 46.0,
            Longitude = 20.0,
            ExperiencePoints = 50,
            Status = "Active",
            Type = "Location",
            CreatedById = -1,
            RequiredParticipants = 10,
            RadiusInMeters = 100,
            Image = null
        };

        // Act
        var result = ((ObjectResult)controller.Update(updateDto).Result)?.Value as ChallengeResponseDto;

        // Assert - Response
        result.ShouldNotBeNull();
        result.Id.ShouldBe(createResult.Id);
        result.Name.ShouldBe(updateDto.Name);
        result.Description.ShouldBe(updateDto.Description);

        // Assert - Database
        var storedEntity = dbContext.Challenges.FirstOrDefault(c => c.Id == createResult.Id);
        storedEntity.ShouldNotBeNull();
        storedEntity.Name.ShouldBe(updateDto.Name);
        storedEntity.Description.ShouldBe(updateDto.Description);
    }

    [Fact]
    public void Deletes()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<EncountersContext>();

        var createDto = new ChallengeCreateDto
        {
            Name = "Challenge To Delete",
            Description = "Description",
            Latitude = 45.0,
            Longitude = 19.0,
            ExperiencePoints = 10,
            Status = "Draft",
            Type = "Social",
            CreatedById = -1,
            RequiredParticipants = 5,  
            RadiusInMeters = 50,       
            Image = null
        };
        var createResult = ((ObjectResult)controller.Create(createDto).Result)?.Value as ChallengeResponseDto;

        // Act
        var result = (OkResult)controller.Delete(createResult.Id);

        // Assert - Response
        result.StatusCode.ShouldBe(200);

        // Assert - Database
        var storedEntity = dbContext.Challenges.FirstOrDefault(c => c.Id == createResult.Id);
        storedEntity.ShouldBeNull();
    }

    [Fact]
    public void Approves()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<EncountersContext>();

        var createDto = new ChallengeCreateDto
        {
            Name = "Challenge To Approve",
            Description = "Description",
            Latitude = 45.0,
            Longitude = 19.0,
            ExperiencePoints = 10,
            Status = "Pending",
            Type = "Social",
            CreatedById = -1,
            RequiredParticipants = 10, 
            RadiusInMeters = 50,       
            Image = null
        };
        var createResult = ((ObjectResult)controller.Create(createDto).Result)?.Value as ChallengeResponseDto;

        // Act
        var result = (OkResult)controller.ApproveChallenge(createResult.Id);

        // Assert
        result.StatusCode.ShouldBe(200);

        // Verify Status in Database
        var storedEntity = dbContext.Challenges.FirstOrDefault(c => c.Id == createResult.Id);
        storedEntity.Status.ToString().ShouldNotBe("Draft");
    }

    [Fact]
    public void Rejects()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<EncountersContext>();

        var createDto = new ChallengeCreateDto
        {
            Name = "Challenge To Reject",
            Description = "Description",
            Latitude = 45.0,
            Longitude = 19.0,
            ExperiencePoints = 10,
            Status = "Pending",
            Type = "Social",
            CreatedById = -1,
            RequiredParticipants = 5, 
            RadiusInMeters = 50,      
            Image = null
        };
        var createResult = ((ObjectResult)controller.Create(createDto).Result)?.Value as ChallengeResponseDto;

        // Act
        var result = (OkResult)controller.RejectChallenge(createResult.Id);

        // Assert
        result.StatusCode.ShouldBe(200);

        var storedEntity = dbContext.Challenges.FirstOrDefault(c => c.Id == createResult.Id);
        storedEntity.Status.ToString().ShouldBe("Archived");
    }

    private static ChallengeController CreateController(IServiceScope scope)
    {
        return new ChallengeController(scope.ServiceProvider.GetRequiredService<IChallengeService>())
        {
            ControllerContext = BuildContext("-1")
        };
    }
}