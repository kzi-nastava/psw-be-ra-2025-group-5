using Explorer.API.Controllers.Tours.Author;
using Explorer.Tours.API.Dtos.KeyPoints;
using Explorer.Tours.API.Dtos.Tours;
using Explorer.Tours.API.Public.Tour;
using Explorer.Tours.API.Dtos.Locations;
using Explorer.Tours.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Microsoft.AspNetCore.Http;

namespace Explorer.Tours.Tests.Integration.Administration;

[Collection("Sequential")]
public class KeyPointQueryTests : BaseToursIntegrationTest
{
    public KeyPointQueryTests(ToursTestFactory factory) : base(factory) { }

    private static CreateTourDto CreateTestTour()
    {
        return new CreateTourDto
        {
            AuthorId = 1,
            Name = "Test Tour for Query",
            Description = "Test description",
            Difficulty = "Easy",
            Tags = new List<string> { "Test" },
            Price = 0
        };
    }

    [Fact]
    public void Retrieves_tour_with_key_points()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        var newTour = CreateTestTour();
        var createdTour = ((ObjectResult)controller.Create(newTour).Result)?.Value as TourDto;
        var tourId = createdTour.Id;

        var kp1 = new CreateKeyPointDto
        {
            Name = "First Point",
            Description = "First description",
            Location = new LocationDto { Latitude = 44.0, Longitude = 20.0 },
            Secret = "First secret"
        };
        var kp2 = new CreateKeyPointDto
        {
            Name = "Second Point",
            Description = "Second description",
            Location = new LocationDto { Latitude = 44.1, Longitude = 20.1 },
            Secret = "Second secret"
        };

        controller.AddKeyPoint(tourId, kp1);
        controller.AddKeyPoint(tourId, kp2);

        // Act
        var result = ((ObjectResult)controller.Get(tourId).Result)?.Value as TourDto;

        // Assert
        result.ShouldNotBeNull();
        result.Id.ShouldBe(tourId);
        result.KeyPoints.ShouldNotBeNull();
        result.KeyPoints.Count.ShouldBe(2);
        result.KeyPoints.Any(kp => kp.Name == "First Point").ShouldBeTrue();
        result.KeyPoints.Any(kp => kp.Name == "Second Point").ShouldBeTrue();
    }

    [Fact]
    public void Retrieves_tour_with_empty_key_points()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        var newTour = CreateTestTour();
        var createdTour = ((ObjectResult)controller.Create(newTour).Result)?.Value as TourDto;
        var tourId = createdTour.Id;

        // Act
        var result = ((ObjectResult)controller.Get(tourId).Result)?.Value as TourDto;

        // Assert
        result.ShouldNotBeNull();
        result.Id.ShouldBe(tourId);
        result.KeyPoints.ShouldNotBeNull();
        result.KeyPoints.Count.ShouldBe(0);
    }

    [Fact]
    public void Key_points_include_location_data()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        var newTour = CreateTestTour();
        var createdTour = ((ObjectResult)controller.Create(newTour).Result)?.Value as TourDto;
        var tourId = createdTour.Id;

        var keyPoint = new CreateKeyPointDto
        {
            Name = "Location Test Point",
            Location = new LocationDto { Latitude = 45.5, Longitude = 21.5 },
            Description = "Description Test"
        };

        controller.AddKeyPoint(tourId, keyPoint);

        // Act
        var result = ((ObjectResult)controller.Get(tourId).Result)?.Value as TourDto;

        // Assert
        result.ShouldNotBeNull();
        var retrievedKp = result.KeyPoints.FirstOrDefault(kp => kp.Name == "Location Test Point");
        retrievedKp.ShouldNotBeNull();
        retrievedKp.Location.ShouldNotBeNull();
        retrievedKp.Location.Latitude.ShouldBe(45.5);
        retrievedKp.Location.Longitude.ShouldBe(21.5);
    }

    [Fact]
    public void Key_points_include_optional_fields()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        var newTour = CreateTestTour();
        var createdTour = ((ObjectResult)controller.Create(newTour).Result)?.Value as TourDto;
        var tourId = createdTour.Id;

        var keyPoint = new CreateKeyPointDto
        {
            Name = "Complete Point",
            Description = "Full description",
            Location = new LocationDto { Latitude = 44.0, Longitude = 20.0 },
            ImagePath = CreateTestImage(),
            Secret = "Hidden secret"
        };

        controller.AddKeyPoint(tourId, keyPoint);

        // Act
        var result = ((ObjectResult)controller.Get(tourId).Result)?.Value as TourDto;

        // Assert
        result.ShouldNotBeNull();
        var retrievedKp = result.KeyPoints.FirstOrDefault(kp => kp.Name == "Complete Point");
        retrievedKp.ShouldNotBeNull();
        retrievedKp.Description.ShouldBe("Full description");
        retrievedKp.ImagePath.ShouldNotBeNull();
        retrievedKp.ImagePath.ShouldNotBeNull();
        retrievedKp.ImagePath.Length.ShouldBeGreaterThan(0);
        retrievedKp.Secret.ShouldBe("Hidden secret");
    }

    private static TourController CreateController(IServiceScope scope)
    {
        return new TourController(scope.ServiceProvider.GetRequiredService<ITourService>())
        {
            ControllerContext = BuildContext("-1")
        };
    }

    private static IFormFile CreateTestImage(string name = "image.png")
    {
        var bytes = new byte[] { 1, 2, 3 };
        var stream = new MemoryStream(bytes);
        return new FormFile(stream, 0, bytes.Length, name, name)
        {
            Headers = new HeaderDictionary(),
            ContentType = "image/png"
        };
    }
}
