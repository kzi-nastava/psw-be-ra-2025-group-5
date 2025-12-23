using Explorer.BuildingBlocks.Core.Exceptions;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Infrastructure.Database;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace Explorer.Tours.Tests.Integration.TourAuthoring
{
    [Collection("Sequential")]
    public class TourThumbnailTest : BaseToursIntegrationTest
    {
        public TourThumbnailTest(ToursTestFactory factory) : base(factory) { }

        [Fact]
        public void Successfully_adds_thumbnail_to_tour()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<ITourService>();
            var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();

            var createDto = new CreateTourDto
            {
                Name = "Tour with Thumbnail",
                Description = "Test tour",
                Difficulty = "Easy",
                Tags = new List<string> { "Nature" },
                Price = 100
            };

            var tour = service.Create(createDto, authorId: 1);

            // Create fake image bytes (PNG header)
            var imageBytes = new byte[] { 0x89, 0x50, 0x4E, 0x47 };

            // Act
            var result = service.AddThumbnail(tour.Id, imageBytes, "image/png");

            // Assert
            result.ShouldNotBeNull();
            result.TourId.ShouldBe(tour.Id);
            result.ContentType.ShouldBe("image/png");
            result.Url.ShouldNotBeNull();
            result.Url.ShouldContain("tours");
            result.Url.ShouldContain(tour.Id.ToString());

            dbContext.ChangeTracker.Clear();
            var stored = dbContext.Tours.Find(tour.Id);
            stored.ShouldNotBeNull();
            stored.ThumbnailPath.ShouldBe(result.Url);
            stored.ThumbnailContentType.ShouldBe("image/png");
        }

        [Fact]
        public void Successfully_retrieves_thumbnail()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<ITourService>();

            var createDto = new CreateTourDto
            {
                Name = "Tour with Thumbnail",
                Description = "Test tour",
                Difficulty = "Medium",
                Tags = new List<string> { "Adventure" },
                Price = 150
            };

            var tour = service.Create(createDto, authorId: 1);
            var imageBytes = new byte[] { 0x89, 0x50, 0x4E, 0x47 };
            
            service.AddThumbnail(tour.Id, imageBytes, "image/png");

            // Act
            var result = service.GetThumbnail(tour.Id);

            // Assert
            result.ShouldNotBeNull();
            result.TourId.ShouldBe(tour.Id);
            result.ContentType.ShouldBe("image/png");
            result.Url.ShouldNotBeNull();
        }

        [Fact]
        public void Thumbnail_is_deleted_when_tour_is_deleted()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<ITourService>();

            var createDto = new CreateTourDto
            {
                Name = "Tour to Delete",
                Description = "Test",
                Difficulty = "Hard",
                Tags = new List<string> { "Extreme" },
                Price = 500
            };

            var tour = service.Create(createDto, authorId: 1);
            var imageBytes = new byte[] { 0x89, 0x50, 0x4E, 0x47 };
            var thumbnail = service.AddThumbnail(tour.Id, imageBytes, "image/png");

            // Act
            service.Delete(tour.Id);

            // Assert - File should be deleted
            var filePath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "UserUploads",
                thumbnail.Url.Substring(thumbnail.Url.IndexOf("tours"))
            );
            File.Exists(filePath).ShouldBeFalse();
        }

        [Fact]
        public void Fails_when_tour_not_found()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<ITourService>();
            var imageBytes = new byte[] { 0x89, 0x50, 0x4E, 0x47 };

            // Act & Assert
            Should.Throw<NotFoundException>(() => service.AddThumbnail(999999, imageBytes, "image/png"));
        }

        [Fact]
        public void Fails_when_file_is_empty()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<ITourService>();

            var createDto = new CreateTourDto
            {
                Name = "Tour",
                Description = "Test",
                Difficulty = "Easy",
                Tags = new List<string> { "Test" },
                Price = 100
            };

            var tour = service.Create(createDto, authorId: 1);
            var emptyBytes = new byte[] { };

            // Act & Assert
            Should.Throw<ArgumentException>(() => 
                service.AddThumbnail(tour.Id, emptyBytes, "image/png")
            );
        }

    }
}