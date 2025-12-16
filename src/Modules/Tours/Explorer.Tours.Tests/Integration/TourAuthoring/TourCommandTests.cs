using Explorer.API.Controllers.Author;
using Explorer.BuildingBlocks.Core.Exceptions;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Infrastructure.Database;
using Explorer.Tours.Tests.Builders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using TourDifficulty = Explorer.Tours.Core.Domain.TourDifficulty;

namespace Explorer.Tours.Tests.Integration.TourAuthoring;

[Collection("Sequential")]
public class TourCommandTests : BaseToursIntegrationTest
{
    private void ShouldFailValidation(string field, object? value, bool isCreate = true)
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var updatedEntity = TourDtoBuilder.CreateValid();
        var prop = typeof(CreateTourDto).GetProperty(field);
        prop?.SetValue(updatedEntity, value);

        Type exception = typeof(ArgumentException);

        // Act & Assert
        Should.Throw(() => controller.Create(updatedEntity), exception);
    }

    public TourCommandTests(ToursTestFactory factory) : base(factory) { }

    [Fact]
    public void Creates()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();
        var newEntity = TourDtoBuilder.CreateValid();

        // Act
        var result = ((ObjectResult)controller.Create(newEntity).Result)?.Value as TourDto;

        // Assert - Response
        result.ShouldNotBeNull();
        result.Id.ShouldNotBe(0);
        result.Name.ShouldBe(newEntity.Name);

        // Assert - Database
        var storedEntity = dbContext.Tours.FirstOrDefault(i => i.Name == newEntity.Name);
        storedEntity.ShouldNotBeNull();
        storedEntity.Id.ShouldBe(result.Id);
    }

    [Theory]
    [InlineData(nameof(CreateTourDto.Name))]
    [InlineData(nameof(CreateTourDto.AuthorId))]
    public void Create_fails_null_data(string field)
    {
        ShouldFailValidation(field, null);
    }

    [Theory]
    [InlineData(nameof(CreateTourDto.Difficulty))]
    public void Create_fails_invalid_enum_data(string field)
    {
        ShouldFailValidation(field, "Invalid enum value");
    }

    [Theory]
    [InlineData(nameof(CreateTourDto.Tags))]
    [InlineData(nameof(CreateTourDto.Price))]
    public void Create_fails_invalid_data(string field)
    {
        object value = field == "Price" ? -1 : new List<string> { "Nature", "Nature", "Viewpoints" };
        ShouldFailValidation(field, value);
    }

    [Fact]
    public void Updates()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();

        var updatedEntity = new UpdateTourDto
        {
            Name = "Updated Zlatibor Tour",
            Description = "Updated description",
            Difficulty = "Hard",
            Tags = new List<string> { "Updated", "Nature" },
            Price = 100
        };

        // Act
        var result = ((ObjectResult)controller.Update(-3, updatedEntity).Result)?.Value as TourDto;

        // Assert - Response
        result.ShouldNotBeNull();
        result.Id.ShouldBe(-3);
        result.Name.ShouldBe(updatedEntity.Name);
        result.Description.ShouldBe(updatedEntity.Description);

        // Assert - Database
        var storedEntity = dbContext.Tours.Single(i => i.Id == -3);
        storedEntity.ShouldNotBeNull();
        storedEntity.Name.ShouldBe(updatedEntity.Name);
        storedEntity.Description.ShouldBe(updatedEntity.Description);
        storedEntity.Difficulty.ShouldBe(Enum.Parse<TourDifficulty>(updatedEntity.Difficulty));
        storedEntity.Price.ShouldBe(updatedEntity.Price);
        storedEntity.Tags.ShouldBeEquivalentTo(updatedEntity.Tags);
    }

    [Fact]
    public void Update_fails_invalid_id()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var updatedEntity = new UpdateTourDto
        {
            Name = "Test",
            Description = "Test",
            Difficulty = "Easy",
            Tags = new List<string> { "Test" },
            Price = 0
        };

        // Act & Assert
        Should.Throw<NotFoundException>(() => controller.Update(-999, updatedEntity));
    }

    [Fact]
    public void Deletes()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();

        // Act
        var result = (OkResult)controller.Delete(-1);

        // Assert - Response
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(200);

        // Assert - Database
        var storedCourse = dbContext.Tours.FirstOrDefault(i => i.Id == -1);
        storedCourse.ShouldBeNull();
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

    private static TourController CreateController(IServiceScope scope)
    {
        return new TourController(scope.ServiceProvider.GetRequiredService<ITourService>())
        {
            ControllerContext = BuildContext("-1")
        };
    }



    //[Theory]
    //[InlineData(-6, 422, TourStatus.Archived)]
    //[InlineData(-5, 422, TourStatus.Published)]
    //[InlineData(-4, 200, TourStatus.Published)]
    //public void Publishes(long tourId, int expectedResponseCode, TourStatus expectedStatus)
    //{
    //    // Arrange
    //    using var scope = Factory.Services.CreateScope();
    //    var controller = CreateController(scope);
    //    var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();

    //    var newKeyPoint1 = new CreateKeyPointDto
    //    {
    //        Name = "New Key Point",
    //        Description = "Test description",
    //        Location = new LocationDto { Latitude = 44.0, Longitude = 20.0 },
    //        Image = null,
    //        Secret = "Test secret"
    //    };

    //    var newKeyPoint2 = new CreateKeyPointDto
    //    {
    //        Name = "New Key Point",
    //        Description = "Test description",
    //        Location = new LocationDto { Latitude = 44.0, Longitude = 20.0 },
    //        Image = null,
    //        Secret = "Test secret"
    //    };



    //    controller.AddKeyPoint(tourId, newKeyPoint1);
    //    controller.AddKeyPoint(tourId, newKeyPoint2);

    //    // Act
    //    var result = (ObjectResult)controller.Publish(tourId).Result;

    //    // Assert - Response
    //    result.ShouldNotBeNull();
    //    result.StatusCode.ShouldBe(expectedResponseCode);

    //    // Assert - Database
    //    var storedEntity = dbContext.Tours.FirstOrDefault(t => t.Id == tourId);
    //    storedEntity.ShouldNotBeNull();
    //    storedEntity.Status.ShouldBe(expectedStatus);
    //}

    //[Theory]
    //[InlineData(-6, 422, TourStatus.Archived)]
    //[InlineData(-5, 200, TourStatus.Archived)]
    //[InlineData(-4, 422, TourStatus.Draft)]
    //public void Archives(long tourId, int expectedResponseCode, TourStatus expectedStatus)
    //{
    //    // Arrange
    //    using var scope = Factory.Services.CreateScope();
    //    var controller = CreateController(scope);
    //    var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();

    //    // Act
    //    //var result = (ObjectResult)controller.Archive(tourId).Result;

    //    // Assert - Response
    //    //result.ShouldNotBeNull();
    //    //result.StatusCode.ShouldBe(expectedResponseCode);


    //    Should.Throw<InvalidOperationException>(() => controller.Archive(tourId));

    //    // Assert - Database
    //    var storedEntity = dbContext.Tours.FirstOrDefault(t => t.Id == tourId);
    //    storedEntity.ShouldNotBeNull();
    //    storedEntity.Status.ShouldBe(expectedStatus);
    //}

    //[Theory]
    //[InlineData(-6, 200, TourStatus.Published)]
    //[InlineData(-5, 422, TourStatus.Published)]
    //[InlineData(-4, 422, TourStatus.Draft)]
    //public void Reactivates(long tourId, int expectedResponseCode, TourStatus expectedStatus)
    //{
    //    // Arrange
    //    using var scope = Factory.Services.CreateScope();
    //    var controller = CreateController(scope);
    //    var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();

    //    // Act
    //    var result = (ObjectResult)controller.Reactivate(tourId).Result;

    //    // Assert - Response
    //    result.ShouldNotBeNull();
    //    result.StatusCode.ShouldBe(expectedResponseCode);

    //    // Assert - Database
    //    var storedEntity = dbContext.Tours.FirstOrDefault(t => t.Id == tourId);
    //    storedEntity.ShouldNotBeNull();
    //    storedEntity.Status.ShouldBe(expectedStatus);
    //}

    [Theory]
    [InlineData(-2, TourStatus.Published)] // Draft tour
    [InlineData(-3, TourStatus.Published)] // Draft tour
    public void Publishes(long tourId, TourStatus expectedStatus)
    {
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();

        // Dodaj KeyPoints samo ako je Draft
        AddKeyPoints(controller, tourId);

        // Dodaj Durations kroz Update
        AddDurations(controller, tourId);

        // Act
        var result = (ObjectResult)controller.Publish(tourId).Result;

        // Assert - Response
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(200);

        // Assert - Database
        var storedEntity = dbContext.Tours.First(t => t.Id == tourId);
        storedEntity.Status.ShouldBe(expectedStatus);
    }

    [Theory]
    [InlineData(-2, TourStatus.Archived)]
    [InlineData(-3, TourStatus.Archived)]
    public void Archives(long tourId, TourStatus expectedStatus)
    {
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();

        // Prvo publish ako je Draft
        var tour = dbContext.Tours.First(t => t.Id == tourId);
        if (tour.Status == TourStatus.Draft)
        {
            AddKeyPoints(controller, tourId);
            AddDurations(controller, tourId);
            controller.Publish(tourId);
        }

        // Act
        var result = (ObjectResult)controller.Archive(tourId).Result;

        // Assert - Response
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(200);

        // Assert - Database
        var storedEntity = dbContext.Tours.First(t => t.Id == tourId);
        storedEntity.Status.ShouldBe(expectedStatus);
    }

    [Theory]
    [InlineData(-2, TourStatus.Published)]
    [InlineData(-3, TourStatus.Published)]
    public void Reactivates(long tourId, TourStatus expectedStatus)
    {
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();

        // Prvo publish i archive ako je Draft
        var tour = dbContext.Tours.First(t => t.Id == tourId);
        if (tour.Status == TourStatus.Draft)
        {
            AddKeyPoints(controller, tourId);
            AddDurations(controller, tourId);
            controller.Publish(tourId);
            controller.Archive(tourId);
        }
        else if (tour.Status == TourStatus.Published)
        {
            controller.Archive(tourId);
        }

        // Act
        var result = (ObjectResult)controller.Reactivate(tourId).Result;

        // Assert - Response
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(200);

        // Assert - Database
        var storedEntity = dbContext.Tours.First(t => t.Id == tourId);
        storedEntity.Status.ShouldBe(expectedStatus);
    }

    // --- Pomoćne funkcije ---

    private void AddKeyPoints(TourController controller, long tourId)
    {
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();
        var tour = dbContext.Tours.First(t => t.Id == tourId);

        if (tour.Status != TourStatus.Draft) return; // samo Draft

        var kp1 = new CreateKeyPointDto
        {
            Name = "KP1",
            Description = "Description1",
            Location = new LocationDto { Latitude = 44.0, Longitude = 20.0 },
            Image = null,
            Secret = "Secret1"
        };
        var kp2 = new CreateKeyPointDto
        {
            Name = "KP2",
            Description = "Description2",
            Location = new LocationDto { Latitude = 44.1, Longitude = 20.1 },
            Image = null,
            Secret = "Secret2"
        };

        controller.AddKeyPoint(tourId, kp1);
        controller.AddKeyPoint(tourId, kp2);
    }

    private void AddDurations(TourController controller, long tourId)
    {
        var updateDto = new UpdateTourDto
        {
            Name = "Updated Tour Name",
            Description = "Updated description",
            Difficulty = "Easy",
            Tags = new List<string> { "Tag1" },
            Price = 0,
            Durations = new List<TourDurationDto>
        {
            new TourDurationDto { TransportType = "Walking", DurationMinutes = 60 }
        }
        };

        controller.Update(tourId, updateDto);
    }

}