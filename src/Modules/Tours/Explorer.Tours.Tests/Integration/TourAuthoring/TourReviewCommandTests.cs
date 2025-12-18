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
using Xunit;

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
            var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();

            long tourId = -1;
            long userId = -2;
            string username = "turista1";

            // KREIRAJTE TOUR EXECUTION KORISTEĆI StartNew METODU
            var existingExecution = dbContext.TourExecutions
                .FirstOrDefault(e => e.UserId == userId && e.TourId == tourId);

            if (existingExecution == null)
            {
                // Koristite fabričku metodu
                var execution = TourExecution.StartNew(userId, tourId);
                dbContext.TourExecutions.Add(execution);
                dbContext.SaveChanges();
            }

            var controller = CreateController(scope);

            var newReview = new TourReviewDto
            {
                TouristID = userId,
                Grade = 5,
                Comment = "Sjajna tura!",
                Progress = 100
            };

            // Act
            var actionResult = controller.Create(tourId, userId, username, newReview).Result;
            var createdTour = (actionResult as ObjectResult)?.Value as TourDto;

            // Assert
            createdTour.ShouldNotBeNull();
            createdTour.Reviews.ShouldNotBeNull();

            var tourFromDb = dbContext.Tours
                .Include(t => t.Reviews)
                .First(t => t.Id == tourId);

            tourFromDb.Reviews.ShouldContain(r =>
                r.TouristID == userId &&
                r.Grade == 5 &&
                r.Comment == "Sjajna tura!");
        }

        [Fact]
        public void Create_fails_invalid_id()
        {
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);

            long invalidTourId = -999;
            long userId = -2;
            string username = "turista1";

            var dto = new TourReviewDto
            {
                TouristID = userId,
                Grade = 5,
                Comment = "Test",
                Progress = 100
            };

            Should.Throw<InvalidOperationException>(() => controller.Create(invalidTourId, userId, username, dto));
        }

        [Fact]
        public void Updates()
        {
            using var scope = Factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();
            var controller = CreateController(scope);

            long tourId = -1;
            long userId = -2;
            string username = "turista1";

            // KREIRAJTE TOUR EXECUTION
            var existingExecution = dbContext.TourExecutions
                .FirstOrDefault(e => e.UserId == userId && e.TourId == tourId);

            if (existingExecution == null)
            {
                var execution = TourExecution.StartNew(userId, tourId);
                dbContext.TourExecutions.Add(execution);
                dbContext.SaveChanges();
            }

            var original = new TourReviewDto
            {
                TouristID = userId,
                Grade = 3,
                Comment = "Ok tura",
                Progress = 50
            };

            var createResult = controller.Create(tourId, userId, username, original).Result as ObjectResult;
            var createdTour = createResult?.Value as TourDto;
            createdTour.ShouldNotBeNull();

            var createdReview = createdTour.Reviews.Last();

            var updatedDto = new TourReviewDto
            {
                TouristID = userId,
                Grade = 4,
                Comment = "Izmenjen komentar",
                Progress = 100
            };

            var updateResult = controller.Update(tourId, userId, createdReview.Id, updatedDto).Result as ObjectResult;
            var updatedTour = updateResult?.Value as TourDto;
            updatedTour.ShouldNotBeNull();

            var updatedReview = updatedTour.Reviews.First(r => r.Id == createdReview.Id);
            updatedReview.Grade.ShouldBe(4);
            updatedReview.Comment.ShouldBe("Izmenjen komentar");
        }

        [Fact]
        public void Update_fails_invalid_id()
        {
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);

            long invalidId = -999;
            long tourId = 1; 
            long userId = -21;

            var updateDto = new TourReviewDto
            {
                TouristID = userId,
                Grade = 4,
                Comment = "Nece uspeti",
                Progress = 80
            };

            Should.Throw<NotFoundException>(() => controller.Update(tourId, userId, invalidId, updateDto));
        }

        [Fact]
        public void Deletes()
        {
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();

            long tourId = -1;
            long userId = -2;
            string username = "turista1";

            var existingExecution = dbContext.TourExecutions
                .FirstOrDefault(e => e.UserId == userId && e.TourId == tourId);

            if (existingExecution == null)
            {
                var execution = TourExecution.StartNew(userId, tourId);
                dbContext.TourExecutions.Add(execution);
                dbContext.SaveChanges();
            }

            var newReview = new TourReviewDto
            {
                TouristID = userId,
                Grade = 5,
                Comment = "Brisanje test",
                Progress = 100
            };

            var createResult = controller.Create(tourId, userId, username, newReview).Result as ObjectResult;
            var createdTour = createResult?.Value as TourDto;
            createdTour.ShouldNotBeNull();

            var createdReview = createdTour.Reviews.Last();

            var deleteResult = controller.Delete(tourId, createdReview.Id) as NoContentResult;
            deleteResult.StatusCode.ShouldBe(204);

            var tourFromDb = dbContext.Tours.Include(t => t.Reviews).First(t => t.Id == tourId);
            tourFromDb.Reviews.ShouldNotContain(r => r.Id == createdReview.Id);
        }

        [Fact]
        public void Delete_fails_invalid_id()
        {
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);

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
