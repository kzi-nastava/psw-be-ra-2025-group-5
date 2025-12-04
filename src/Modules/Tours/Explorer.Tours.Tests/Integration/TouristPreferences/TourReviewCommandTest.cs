using Explorer.API.Controllers.Tourist;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public;
using Explorer.Tours.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Explorer.Tours.Tests.Integration.TouristPreferences
{
    [Collection("Sequential")]
    public class TourReviewCommandTest : BaseToursIntegrationTest
    {
        public TourReviewCommandTest(ToursTestFactory factory) : base(factory) { }

        [Fact]
        public void Creates()
        {
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();

            var tour = dbContext.Tours.FirstOrDefault();
            tour.ShouldNotBeNull("Nema ture u bazi za testiranje.");

            var dto = new TourReviewDto
            {
                TouristID = 1,
                Grade = 5,
                Comment = "Sjajna tura!",
                Progress = 100
            };

            // Act
            var result = controller.Create(tour.Id, dto).Result as ObjectResult;
            var created = result?.Value as TourReviewDto;

            // Assert response
            created.ShouldNotBeNull();
            created.Id.ShouldNotBe(0);
            created.Grade.ShouldBe(5);
            created.TourID.ShouldBe(tour.Id);
            created.TouristID.ShouldBe(1);

            // Assert persisted in DB
            var stored = dbContext.TourReviews.FirstOrDefault(r => r.Id == created.Id);
            stored.ShouldNotBeNull();
            stored.Grade.ShouldBe(5);
            stored.Comment.ShouldBe("Sjajna tura!");
        }

        [Fact]
        public void Create_fails_invalid_id()
        {
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);

            var invalidTourId = -123;

            var dto = new TourReviewDto
            {
                TouristID = 1,
                Grade = 5,
                Comment = "Test",
                Progress = 100
            };

            Should.Throw<Explorer.BuildingBlocks.Core.Exceptions.NotFoundException>(
                () => controller.Create(invalidTourId, dto)
            );
        }

        [Fact]
        public void Updates()
        {
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();

            var tour = dbContext.Tours.FirstOrDefault();
            tour.ShouldNotBeNull();

            // Kreiramo recenziju
            var original = new TourReviewDto
            {
                TouristID = 1,
                Grade = 3,
                Comment = "Ok tura",
                Progress = 50
            };

            var createResult = controller.Create(tour.Id, original).Result as ObjectResult;
            var created = createResult?.Value as TourReviewDto;
            created.ShouldNotBeNull();
            created.Id.ShouldNotBe(0);

            // Update
            var updatedDto = new TourReviewDto
            {
                TouristID = created.TouristID,
                Grade = 4,
                Comment = "Izmenjen komentar",
                Progress = 100
            };

            var updateResult = controller.Update(tour.Id, created.Id, updatedDto).Result as ObjectResult;
            var updated = updateResult?.Value as TourReviewDto;

            updated.ShouldNotBeNull();
            updated.Id.ShouldBe(created.Id);
            updated.Grade.ShouldBe(4);
            updated.Comment.ShouldBe("Izmenjen komentar");
            updated.TourID.ShouldBe(tour.Id);

            var stored = dbContext.TourReviews.FirstOrDefault(r => r.Id == created.Id);
            stored.ShouldNotBeNull();
            stored.Grade.ShouldBe(4);
            stored.Comment.ShouldBe("Izmenjen komentar");
        }

        [Fact]
        public void Update_fails_invalid_id()
        {
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);

            var invalidId = -123;
            var tourId = 1;

            var dto = new TourReviewDto
            {
                TouristID = 1,
                Grade = 4,
                Comment = "Nece uspeti",
                Progress = 80
            };

            Should.Throw<Explorer.BuildingBlocks.Core.Exceptions.NotFoundException>(
                () => controller.Update(tourId, invalidId, dto)
            );
        }

        [Fact]
        public void Deletes()
        {
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();

            var tour = dbContext.Tours.FirstOrDefault();
            tour.ShouldNotBeNull();

            var dto = new TourReviewDto
            {
                TouristID = 1,
                Grade = 5,
                Comment = "Brisanje test",
                Progress = 100
            };

            var createResult = controller.Create(tour.Id, dto).Result as ObjectResult;
            var created = createResult?.Value as TourReviewDto;
            created.ShouldNotBeNull();

            var deleteResult = controller.Delete(created.Id) as OkResult;
            deleteResult.ShouldNotBeNull();
            deleteResult.StatusCode.ShouldBe(200);

            var stored = dbContext.TourReviews.FirstOrDefault(r => r.Id == created.Id);
            stored.ShouldBeNull();
        }

        [Fact]
        public void Delete_fails_invalid_id()
        {
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);

            Should.Throw<Explorer.BuildingBlocks.Core.Exceptions.NotFoundException>(
                () => controller.Delete(-999)
            );
        }

        private static TourReviewController CreateController(IServiceScope scope)
        {
            return new TourReviewController(
                scope.ServiceProvider.GetRequiredService<ITourReviewService>()
            )
            {
                ControllerContext = BuildContext("-1")
            };
        }
    }
}

