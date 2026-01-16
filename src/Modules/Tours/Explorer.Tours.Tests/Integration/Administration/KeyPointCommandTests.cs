using Explorer.API.Controllers.Tours.Author;
using Explorer.BuildingBlocks.Core.Exceptions;
using Explorer.Tours.API.Dtos.KeyPoints;
using Explorer.Tours.API.Dtos.Locations;
using Explorer.Tours.API.Dtos.Tours;
using Explorer.Tours.API.Public.Tour;
using Explorer.Tours.Core.Domain.Tours.Entities;
using Explorer.Tours.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Explorer.Tours.Tests.Integration.Administration;

[Collection("Sequential")]
public class KeyPointCommandTests : BaseToursIntegrationTest
{
    public KeyPointCommandTests(ToursTestFactory factory) : base(factory) { }

    private static CreateTourDto CreateTestTour()
    {
        return new CreateTourDto
        {
            AuthorId = 1,
            Name = "Test Tour for KeyPoints",
            Description = "Test description",
            Difficulty = "Easy",
            Tags = new List<string> { "Test" },
            Price = 0
        };
    }

    private static UpdateTourDto UpdateTestTour()
    {
        return new UpdateTourDto
        {
            Name = "Test Tour for KeyPoints",
            Description = "Test description",
            Difficulty = "Easy",
            Tags = new List<string> { "Test" },
            Price = 0,
            Durations = new List<TourDurationDto>
            {
                new TourDurationDto { TransportType = "Bicycle", DurationMinutes = 20 },
                new TourDurationDto { TransportType = "Walking", DurationMinutes = 120 }
            }
        };
    }

    [Fact]
    public void Adds_key_point()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();

        var newTour = CreateTestTour();
        var createdTour = ((ObjectResult)controller.Create(newTour).Result)?.Value as TourDto;
        var tourId = createdTour.Id;

        var newKeyPoint = new CreateKeyPointDto
        {
            Name = "New Key Point",
            Description = "Test description",
            Location = new LocationDto { Latitude = 44.0, Longitude = 20.0 },
            ImagePath = null,
            Secret = "Test secret"
        };

        // Act
        var result = ((ObjectResult)controller.AddKeyPoint(tourId, newKeyPoint).Result)?.Value as TourDto;

        // Assert - Response
        result.ShouldNotBeNull();
        result.KeyPoints.ShouldNotBeEmpty();
        result.KeyPoints.Any(kp => kp.Name == "New Key Point").ShouldBeTrue();

        // Assert - Database
        var storedTour = dbContext.Tours.FirstOrDefault(t => t.Id == tourId);
        storedTour.ShouldNotBeNull();
        storedTour.KeyPoints.Any(kp => kp.Name == "New Key Point").ShouldBeTrue();
    }

    [Fact]
    public void Add_key_point_fails_invalid_name()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        var newTour = CreateTestTour();
        var createdTour = ((ObjectResult)controller.Create(newTour).Result)?.Value as TourDto;
        var tourId = createdTour.Id;

        var invalidKeyPoint = new CreateKeyPointDto
        {
            Name = "",
            Description = "Test",
            Location = new LocationDto { Latitude = 44.0, Longitude = 20.0 }
        };

        // Act & Assert
        Should.Throw<ArgumentException>(() => controller.AddKeyPoint(tourId, invalidKeyPoint));
    }

    [Fact]
    public void Add_key_point_fails_invalid_description()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        var newTour = CreateTestTour();
        var createdTour = ((ObjectResult)controller.Create(newTour).Result)?.Value as TourDto;
        var tourId = createdTour.Id;

        var invalidKeyPoint = new CreateKeyPointDto
        {
            Name = "Test",
            Description = "",
            Location = new LocationDto { Latitude = 44.0, Longitude = 20.0 }
        };

        // Act & Assert
        Should.Throw<ArgumentException>(() => controller.AddKeyPoint(tourId, invalidKeyPoint));
    }

    [Theory]
    [InlineData(-91, 20.0)]
    [InlineData(44.0, -181)]
    public void Add_key_point_fails_invalid_location(double latitude, double longitude)
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        var newTour = CreateTestTour();
        var createdTour = ((ObjectResult)controller.Create(newTour).Result)?.Value as TourDto;
        var tourId = createdTour.Id;

        var invalidKeyPoint = new CreateKeyPointDto
        {
            Name = "Test Point",
            Description = "Test description",
            Location = new LocationDto { Latitude = latitude, Longitude = longitude }
        };

        // Act & Assert
        Should.Throw<ArgumentException>(() => controller.AddKeyPoint(tourId, invalidKeyPoint));
    }

    [Fact]
    public void Add_key_point_fails_tour_not_in_draft()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        // Kreiraj novu turu
        var newTour = CreateTestTour();
        var createdTour = ((ObjectResult)controller.Create(newTour).Result)?.Value as TourDto;
        var tourId = createdTour.Id;

        var updatedTour = ((ObjectResult)controller.Update(tourId, UpdateTestTour()).Result)?.Value as TourDto;

        var kp1 = new CreateKeyPointDto { Name = "KP1", Description = "Description 1", Location = new LocationDto { Latitude = 44.0, Longitude = 20.0 } };
        var kp2 = new CreateKeyPointDto { Name = "KP2", Description = "Description 2", Location = new LocationDto { Latitude = 44.1, Longitude = 20.1 } };
        controller.AddKeyPoint(tourId, kp1);
        controller.AddKeyPoint(tourId, kp2);
        controller.Publish(tourId);

        var newKeyPoint = new CreateKeyPointDto
        {
            Name = "Test",
            Description = "Test description",
            Location = new LocationDto { Latitude = 44.2, Longitude = 20.2 }
        };

        // Act & Assert
        Should.Throw<InvalidOperationException>(() => controller.AddKeyPoint(tourId, newKeyPoint));
    }

    [Fact]
    public void Updates_key_point()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();

        var newTour = CreateTestTour();
        var createdTour = ((ObjectResult)controller.Create(newTour).Result)?.Value as TourDto;
        var tourId = createdTour.Id;

        var newKeyPoint = new CreateKeyPointDto
        {
            Name = "Original Name",
            Description = "Original Description",
            Location = new LocationDto { Latitude = 44.0, Longitude = 20.0 },
            Secret = "Original Secret"
        };
        var addResult = ((ObjectResult)controller.AddKeyPoint(tourId, newKeyPoint).Result)?.Value as TourDto;
        var keyPointId = addResult.KeyPoints.First(kp => kp.Name == "Original Name").Id;

        var updatedKeyPoint = new CreateKeyPointDto
        {
            Name = "Updated Name",
            Description = "Updated Description",
            Location = new LocationDto { Latitude = 44.0, Longitude = 20.0 },
            Secret = "Updated Secret"
        };

        // Act
        var result = ((ObjectResult)controller.UpdateKeyPoint(tourId, keyPointId, updatedKeyPoint).Result)?.Value as TourDto;

        // Assert
        result.ShouldNotBeNull();
        var updatedKp = result.KeyPoints.FirstOrDefault(kp => kp.Id == keyPointId);
        updatedKp.ShouldNotBeNull();
        updatedKp.Name.ShouldBe("Updated Name");
        updatedKp.Description.ShouldBe("Updated Description");
        updatedKp.Secret.ShouldBe("Updated Secret");

        var storedTour = dbContext.Tours.FirstOrDefault(t => t.Id == tourId);
        var storedKp = storedTour.KeyPoints.FirstOrDefault(kp => kp.Id == keyPointId);
        storedKp.ShouldNotBeNull();
        storedKp.Name.ShouldBe("Updated Name");
    }

    [Fact]
    public void Update_key_point_fails_invalid_id()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        var newTour = CreateTestTour();
        var createdTour = ((ObjectResult)controller.Create(newTour).Result)?.Value as TourDto;
        var tourId = createdTour.Id;

        var updatedKeyPoint = new CreateKeyPointDto
        {
            Name = "Test",
            Description = "Test description",
            Location = new LocationDto { Latitude = 44.0, Longitude = 20.0 }
        };

        // Act & Assert
        Should.Throw<InvalidOperationException>(() => controller.UpdateKeyPoint(tourId, -9999, updatedKeyPoint));
    }

    [Fact]
    public void Removes_key_point()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();

        var newTour = CreateTestTour();
        var createdTour = ((ObjectResult)controller.Create(newTour).Result)?.Value as TourDto;
        var tourId = createdTour.Id;

        var kp1 = new CreateKeyPointDto { Name = "KP1", Description = "Desc 1", Location = new LocationDto { Latitude = 44.0, Longitude = 20.0 } };
        var kp2 = new CreateKeyPointDto { Name = "KP2", Description = "Desc 2", Location = new LocationDto { Latitude = 44.1, Longitude = 20.1 } };
        var kp3 = new CreateKeyPointDto { Name = "KP3", Description = "Desc 3", Location = new LocationDto { Latitude = 44.2, Longitude = 20.2 } };

        controller.AddKeyPoint(tourId, kp1);
        controller.AddKeyPoint(tourId, kp2);
        var addResult = ((ObjectResult)controller.AddKeyPoint(tourId, kp3).Result)?.Value as TourDto;

        var keyPointIdToDelete = addResult.KeyPoints.First(kp => kp.Name == "KP2").Id;
        var initialCount = addResult.KeyPoints.Count;

        // Act
        var result = ((ObjectResult)controller.RemoveKeyPoint(tourId, keyPointIdToDelete, 0.0).Result)?.Value as TourDto;

        // Assert
        result.ShouldNotBeNull();
        result.KeyPoints.Count.ShouldBe(initialCount - 1);
        result.KeyPoints.Any(kp => kp.Id == keyPointIdToDelete).ShouldBeFalse();

        var storedTour = dbContext.Tours.FirstOrDefault(t => t.Id == tourId);
        storedTour.ShouldNotBeNull();
        storedTour.KeyPoints.Any(kp => kp.Id == keyPointIdToDelete).ShouldBeFalse();

        var deletedKeyPoint = dbContext.Set<KeyPoint>().FirstOrDefault(kp => kp.Id == keyPointIdToDelete);
        deletedKeyPoint.ShouldBeNull();
    }

    [Fact]
    public void Remove_key_point_fails_invalid_id()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        var newTour = CreateTestTour();
        var createdTour = ((ObjectResult)controller.Create(newTour).Result)?.Value as TourDto;
        var tourId = createdTour.Id;

        // Act & Assert
        Should.Throw<InvalidOperationException>(() => controller.RemoveKeyPoint(tourId, -9999, 0.0));
    }

    [Fact]
    public void Remove_key_point_fails_tour_not_in_draft()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        var newTour = CreateTestTour();
        var createdTour = ((ObjectResult)controller.Create(newTour).Result)?.Value as TourDto;
        var tourId = createdTour.Id;

        var updatedTour = ((ObjectResult)controller.Update(tourId, UpdateTestTour()).Result)?.Value as TourDto;

        var kp1 = new CreateKeyPointDto { Name = "KP1", Description = "Desc 1", Location = new LocationDto { Latitude = 44.0, Longitude = 20.0 } };
        var kp2 = new CreateKeyPointDto { Name = "KP2", Description = "Desc 2", Location = new LocationDto { Latitude = 44.1, Longitude = 20.1 } };

        controller.AddKeyPoint(tourId, kp1);
        var addResult = ((ObjectResult)controller.AddKeyPoint(tourId, kp2).Result)?.Value as TourDto;
        var keyPointId = addResult.KeyPoints.First().Id;

        controller.Publish(tourId);

        // Act & Assert
        Should.Throw<InvalidOperationException>(() => controller.RemoveKeyPoint(tourId, keyPointId, 0.0));
    }

    [Fact]
    public void Reorders_key_points()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        var newTour = CreateTestTour();
        var createdTour = ((ObjectResult)controller.Create(newTour).Result)?.Value as TourDto;
        var tourId = createdTour.Id;

        var kp1 = new CreateKeyPointDto { Name = "First", Description = "Desc 1", Location = new LocationDto { Latitude = 44.0, Longitude = 20.0 } };
        var kp2 = new CreateKeyPointDto { Name = "Second", Description = "Desc 2", Location = new LocationDto { Latitude = 44.1, Longitude = 20.1 } };
        var kp3 = new CreateKeyPointDto { Name = "Third", Description = "Desc 3", Location = new LocationDto { Latitude = 44.2, Longitude = 20.2 } };

        controller.AddKeyPoint(tourId, kp1);
        controller.AddKeyPoint(tourId, kp2);
        var addResult = ((ObjectResult)controller.AddKeyPoint(tourId, kp3).Result)?.Value as TourDto;

        var firstId = addResult.KeyPoints.First(kp => kp.Name == "First").Id;
        var secondId = addResult.KeyPoints.First(kp => kp.Name == "Second").Id;
        var thirdId = addResult.KeyPoints.First(kp => kp.Name == "Third").Id;

        var reorderDto = new ReorderKeyPointsDto
        {
            OrderedKeyPointIds = new List<long> { thirdId, firstId, secondId }
        };

        // Act
        var result = ((ObjectResult)controller.ReorderKeyPoints(tourId, reorderDto).Result)?.Value as TourDto;

        // Assert
        result.ShouldNotBeNull();
        var sortedKeyPoints = result.KeyPoints.OrderBy(kp => kp.Position).ToList();
        sortedKeyPoints[0].Name.ShouldBe("Third");
        sortedKeyPoints[0].Position.ShouldBe(0);
        sortedKeyPoints[1].Name.ShouldBe("First");
        sortedKeyPoints[1].Position.ShouldBe(1);
        sortedKeyPoints[2].Name.ShouldBe("Second");
        sortedKeyPoints[2].Position.ShouldBe(2);
    }

    [Fact]
    public void Reorder_fails_with_missing_key_point()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        var newTour = CreateTestTour();
        var createdTour = ((ObjectResult)controller.Create(newTour).Result)?.Value as TourDto;
        var tourId = createdTour.Id;

        var kp1 = new CreateKeyPointDto { Name = "KP1", Description = "Desc 1", Location = new LocationDto { Latitude = 44.0, Longitude = 20.0 } };
        var kp2 = new CreateKeyPointDto { Name = "KP2", Description = "Desc 2", Location = new LocationDto { Latitude = 44.1, Longitude = 20.1 } };

        controller.AddKeyPoint(tourId, kp1);
        var addResult = ((ObjectResult)controller.AddKeyPoint(tourId, kp2).Result)?.Value as TourDto;

        var firstId = addResult.KeyPoints.First().Id;

        var reorderDto = new ReorderKeyPointsDto
        {
            OrderedKeyPointIds = new List<long> { firstId }
        };

        // Act & Assert
        Should.Throw<InvalidOperationException>(() => controller.ReorderKeyPoints(tourId, reorderDto));
    }

    [Fact]
    public void Reorder_fails_with_invalid_key_point_id()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        var newTour = CreateTestTour();
        var createdTour = ((ObjectResult)controller.Create(newTour).Result)?.Value as TourDto;
        var tourId = createdTour.Id;

        var kp1 = new CreateKeyPointDto { Name = "KP1", Description = "Desc 1", Location = new LocationDto { Latitude = 44.0, Longitude = 20.0 } };
        var kp2 = new CreateKeyPointDto { Name = "KP2", Description = "Desc 2", Location = new LocationDto { Latitude = 44.1, Longitude = 20.1 } };

        controller.AddKeyPoint(tourId, kp1);
        controller.AddKeyPoint(tourId, kp2);

        var reorderDto = new ReorderKeyPointsDto
        {
            OrderedKeyPointIds = new List<long> { -9999, -8888 }
        };

        // Act & Assert
        Should.Throw<InvalidOperationException>(() => controller.ReorderKeyPoints(tourId, reorderDto));
    }

    private static TourController CreateController(IServiceScope scope)
    {
        return new TourController(
            scope.ServiceProvider.GetRequiredService<ITourService>(),
            scope.ServiceProvider.GetRequiredService<ITourSearchHistoryService>())
        {
            ControllerContext = BuildContext("-1")
        };
    }
}
