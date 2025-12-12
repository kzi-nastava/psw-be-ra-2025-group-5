using Explorer.API.Controllers.Tourist;
using Explorer.BuildingBlocks.Core.Exceptions;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System.Net.Sockets;

namespace Explorer.Tours.Tests.Integration.TouristPreferences
{
    [Collection("Sequential")]
    public class TourReviewCommandTests : BaseToursIntegrationTest
    {
        public TourReviewCommandTests(ToursTestFactory factory) : base(factory) { }

        [Fact]
        public void Creates()
        {
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();

            // Arrange - uzmi postojecu turu iz seed-a
            var anyTour = dbContext.Tours.Include(t => t.Reviews).FirstOrDefault();
            anyTour.ShouldNotBeNull("Nema ture u bazi za testiranje. Dodaj seed podatke.");

            var tourId = anyTour.Id;
            long userId = -21;
            string username = "";
            var newReview = new TourReviewDto
            {
                TouristID = -21, // pretpostavka: postoji turist sa ID = 1 u seed-u
                Grade = 5,
                Comment = "Sjajna tura!",
                Progress = 100.0
            };

            // Act
            var actionResult = controller.Create(tourId, userId, username, newReview).Result;
            var objectResult = actionResult as ObjectResult;
            var createdTour = objectResult?.Value as TourDto;

            // Assert - response
            createdTour.ShouldNotBeNull();
            createdTour.Reviews.ShouldNotBeNull();
            createdTour.Reviews.Any().ShouldBeTrue();
            var createdReview = createdTour.Reviews.Last();
            createdReview.Grade.ShouldBe(5);
            createdReview.Comment.ShouldBe("Sjajna tura!");
            createdReview.TourID.ShouldBe(tourId);
            createdReview.TouristID.ShouldBe(newReview.TouristID);

            // Assert - persisted in DB
            var storedTour = dbContext.Tours
                .Include(t => t.Reviews)
                .ThenInclude(r => r.Images)
                .FirstOrDefault(t => t.Id == tourId);

            storedTour.ShouldNotBeNull();
            storedTour.Reviews.ShouldContain(r => r.Id == createdReview.Id && r.Grade == 5 && r.Comment == "Sjajna tura!");
        }

        [Fact]
        public void Create_fails_invalid_id()
        {
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);

            // Arrange — nepostojeći tourId
            long invalidTourId = -123;
            long userId = -21;
            string username = "";
            var dto = new TourReviewDto
            {
                TouristID = -21,
                Grade = 5,
                Comment = "Test",
                Progress = 100
            };

            // Act & Assert - service should throw NotFoundException before controller returns
            Should.Throw<NotFoundException>(() => controller.Create(invalidTourId, userId, username, dto));
        }

        [Fact]
        public void Updates()
        {
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();

            // Arrange – uzmi postojecu turu
            var tour = dbContext.Tours.Include(t => t.Reviews).FirstOrDefault();
            tour.ShouldNotBeNull();

            // Prvo kreiramo recenziju koju ćemo menjati
            var original = new TourReviewDto
            {
                TouristID = -21,
                Grade = 3,
                Comment = "Ok tura",
                Progress = 50
            };
            long userId = -21;
            string username = "";
            var createResult = controller.Create(tour.Id, userId, username, original).Result as ObjectResult;
            var createdTour = createResult?.Value as TourDto;

            createdTour.ShouldNotBeNull();
            var createdReview = createdTour.Reviews.Last();
            createdReview.ShouldNotBeNull();
            createdReview.Id.ShouldNotBe(0);

            // Novi podaci za update (ID se prosleđuje kroz rutu)
            var updatedDto = new TourReviewDto
            {
                TouristID = -21,
                Grade = 4,
                Comment = "Izmenjen komentar",
                Progress = 100
            };

            // Act
            var updateResult = controller.Update(tour.Id, userId, createdReview.Id, updatedDto).Result as ObjectResult;
            var updatedTour = updateResult?.Value as TourDto;

            // Assert - response
            updatedTour.ShouldNotBeNull();
            var updatedReview = updatedTour.Reviews.First(r => r.Id == createdReview.Id);
            updatedReview.ShouldNotBeNull();
            updatedReview.Id.ShouldBe(createdReview.Id);
            updatedReview.Grade.ShouldBe(4);
            updatedReview.Comment.ShouldBe("Izmenjen komentar");
            updatedReview.TourID.ShouldBe(tour.Id);

            // Assert - DB
            var stored = dbContext.Tours
                .Include(t => t.Reviews)
                .ThenInclude(r => r.Images)
                .FirstOrDefault(t => t.Id == tour.Id);
            stored.ShouldNotBeNull();
            stored.Reviews.First(r => r.Id == createdReview.Id).Grade.ShouldBe(4);
            stored.Reviews.First(r => r.Id == createdReview.Id).Comment.ShouldBe("Izmenjen komentar");
        }

        [Fact]
        public void Update_fails_invalid_id()
        {
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);

            var invalidId = -123;  // ne postoji u seed bazi
            var tourId = 1;        // pretpostavka: postoji tura sa ID=1
            long userId = -21;
            var updateDto = new TourReviewDto
            {
                TouristID = -21,
                Grade = 4,
                Comment = "Nece uspeti",
                Progress = 80
            };

            // Act & Assert
            Should.Throw<NotFoundException>(() => controller.Update(tourId, userId, invalidId, updateDto));
        }

        [Fact]
        public void Deletes()
        {
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();
            long userId = -21;
            // Arrange — kreiramo review koji ćemo obrisati
            var tour = dbContext.Tours.Include(t => t.Reviews).FirstOrDefault();
            tour.ShouldNotBeNull();

            var newReview = new TourReviewDto
            {
                TouristID = -21,
                Grade = 5,
                Comment = "Brisanje test",
                Progress = 100
            };

            string username = "";
            var createResult = controller.Create(tour.Id, userId, username, newReview).Result as ObjectResult;
            var createdTour = createResult?.Value as TourDto;
            createdTour.ShouldNotBeNull();
            var created = createdTour.Reviews.Last();

            // Act
            var deleteResult = controller.Delete(tour.Id, created.Id) as NoContentResult;

            // Assert — response
            deleteResult.ShouldNotBeNull();
            deleteResult.StatusCode.ShouldBe(204);

            // Assert — DB
            var stored = dbContext.Tours
                .Include(t => t.Reviews)
                .FirstOrDefault(t => t.Id == tour.Id);
            stored.ShouldNotBeNull();
            stored.Reviews.ShouldNotContain(r => r.Id == created.Id);
        }

        [Fact]
        public void Delete_fails_invalid_id()
        {
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);

            // Act & Assert
            Should.Throw<NotFoundException>(() => controller.Delete(-1, -999));
        }

        private static TourReviewController CreateController(IServiceScope scope)
        {
            return new TourReviewController(
                scope.ServiceProvider.GetRequiredService<ITourService>()
            )
            {
                ControllerContext = BuildContext("-1")
            };
        }
    }
}
