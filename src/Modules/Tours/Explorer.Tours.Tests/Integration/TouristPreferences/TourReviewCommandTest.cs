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

        private TourReviewController CreateController(IServiceScope scope)
        {
            // Registruj servis ako nije registrovan
            var services = scope.ServiceProvider;
            return new TourReviewController(
                services.GetRequiredService<ITourReviewService>()
            )
            {
                ControllerContext = BuildContext("-1")
            };
        }

        [Fact]
        public void Creates()
        {
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var db = scope.ServiceProvider.GetRequiredService<ToursContext>();

            var tour = db.Tours.FirstOrDefault();
            tour.ShouldNotBeNull();

            var dto = new TourReviewDto
            {
                TouristID = 1,
                Grade = 5,
                Comment = "Sjajna tura!",
                Progress = 100
            };

            var result = controller.Create(tour.Id, dto).Result as ObjectResult;
            var created = result?.Value as TourReviewDto;

            created.ShouldNotBeNull();
            created.Id.ShouldNotBe(0);
            created.TourID.ShouldBe(tour.Id);

            var stored = db.TourReviews.FirstOrDefault(r => r.Id == created.Id);
            stored.ShouldNotBeNull();
            stored.Grade.ShouldBe(5);
        }

        [Fact]
        public void Create_fails_invalid_id()
        {
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);

            var dto = new TourReviewDto
            {
                TouristID = 1,
                Grade = 5,
                Comment = "Test",
                Progress = 100
            };

            var invalidTourId = -123;

            Should.Throw<Explorer.BuildingBlocks.Core.Exceptions.NotFoundException>(
                () => controller.Create(invalidTourId, dto)
            );
        }

        [Fact]
        public void Updates()
        {
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var db = scope.ServiceProvider.GetRequiredService<ToursContext>();

            var tour = db.Tours.FirstOrDefault();
            tour.ShouldNotBeNull();

            // Kreiraj review preko Create()
            var original = new TourReviewDto
            {
                TouristID = 1,
                Grade = 3,
                Comment = "Ok tura",
                Progress = 50
            };
            var created = (controller.Create(tour.Id, original).Result as ObjectResult)?.Value as TourReviewDto;
            created.ShouldNotBeNull();

            // Update preko Update() metode
            var updatedDto = new TourReviewDto
            {
                TouristID = created.TouristID,
                Grade = 4,
                Comment = "Izmenjen komentar",
                Progress = 100
            };
            var updated = (controller.Update(tour.Id, created.Id, updatedDto).Result as ObjectResult)?.Value as TourReviewDto;

            updated.ShouldNotBeNull();
            updated.Id.ShouldBe(created.Id);
            updated.Grade.ShouldBe(4);
            updated.Comment.ShouldBe("Izmenjen komentar");
        }

        [Fact]
        public void Deletes()
        {
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var db = scope.ServiceProvider.GetRequiredService<ToursContext>();

            var tour = db.Tours.FirstOrDefault();
            tour.ShouldNotBeNull();

            var dto = new TourReviewDto
            {
                TouristID = 1,
                Grade = 5,
                Comment = "Brisanje test",
                Progress = 100
            };
            var created = (controller.Create(tour.Id, dto).Result as ObjectResult)?.Value as TourReviewDto;
            created.ShouldNotBeNull();

            var deleteResult = controller.Delete(created.Id) as OkResult;
            deleteResult.ShouldNotBeNull();

            var stored = db.TourReviews.FirstOrDefault(r => r.Id == created.Id);
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

        [Fact]
        public void Update_fails_invalid_id()
        {
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);

            var dto = new TourReviewDto
            {
                TouristID = 1,
                Grade = 4,
                Comment = "Nece uspeti",
                Progress = 80
            };

            Should.Throw<Explorer.BuildingBlocks.Core.Exceptions.NotFoundException>(
                () => controller.Update(1, -123, dto)
            );
        }
    }
}

