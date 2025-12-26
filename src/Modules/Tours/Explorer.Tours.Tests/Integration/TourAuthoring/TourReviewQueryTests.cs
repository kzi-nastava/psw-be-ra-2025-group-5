using Explorer.API.Controllers.Tours.Tourist;
using Explorer.BuildingBlocks.Core.Exceptions;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.Core.Domain.TourExecutions;
using Explorer.Payments.Core.Domain;
using Explorer.Payments.Infrastructure.Database;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Explorer.Tours.API.Dtos.Tours.Reviews;
using Explorer.Tours.API.Public.Tour;

namespace Explorer.Tours.Tests.Integration.TouristPreferences
{
    [Collection("Sequential")]
    public class TourReviewQueryTest : BaseToursIntegrationTest
    {
        public TourReviewQueryTest(ToursTestFactory factory) : base(factory) { }

        [Fact]
        public void Gets_by_tour_with_pagination()
        {
            using var scope = Factory.Services.CreateScope();
            var tourDbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();
            var paymentDbContext = scope.ServiceProvider.GetRequiredService<PaymentsContext>();

            // Koristite prvu turu koja postoji
            var tour = tourDbContext.Tours
                .Include(t => t.Reviews)
                .FirstOrDefault();

            tour.ShouldNotBeNull("Nema tura u bazi.");

            long tourId = tour.Id;

            // Obezbedite purchase token i tour execution
            long userId = -2; // Turista koji postoji u seed-u

            var token = paymentDbContext.TourPurchaseTokens
                .FirstOrDefault(t => t.TourId == tourId && t.TouristId == userId);

            if (token == null)
            {
                token = new TourPurchaseToken(tourId, userId);
                paymentDbContext.TourPurchaseTokens.Add(token);
            }

            var execution = tourDbContext.TourExecutions
                .FirstOrDefault(e => e.UserId == userId && e.TourId == tourId);

            if (execution == null)
            {
                execution = TourExecution.StartNew(userId, tourId);
                tourDbContext.TourExecutions.Add(execution);
            }

            tourDbContext.SaveChanges();

            var controller = CreateController(scope);

            var actionResult = controller.GetByTour(tourId, page: 0, pageSize: 2);

            Assert.NotNull(actionResult);
            Assert.NotNull(actionResult.Result);

            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var paged = Assert.IsType<PagedResult<TourReviewDto>>(okResult.Value);

            Assert.Equal(tour.Reviews.Count, paged.TotalCount);

            var expectedCount = Math.Min(2, tour.Reviews.Count);
            Assert.Equal(expectedCount, paged.Results.Count);

            if (expectedCount > 0)
            {
                var expectedIds = tour.Reviews
                    .OrderBy(r => r.Id)
                    .Take(2)
                    .Select(r => r.Id)
                    .ToList();

                var actualIds = paged.Results.Select(r => r.Id).ToList();
                Assert.Equal(expectedIds, actualIds);
            }
        }

        [Fact]
        public void Returns_empty_when_no_reviews()
        {
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var db = scope.ServiceProvider.GetRequiredService<ToursContext>();

            var emptyTour = db.Tours.Include(t => t.Reviews).FirstOrDefault(t => !t.Reviews.Any());
            if (emptyTour is null)
            {
                emptyTour = new Core.Domain.Tours.Tour(
                    authorId: 1,
                    name: "Empty test tour",
                    description: "desc",
                    difficulty: Core.Domain.Tours.TourDifficulty.Easy,
                    tags: new List<string>(),
                    price: 0
                );

                db.Tours.Add(emptyTour);
                db.SaveChanges();

                emptyTour = db.Tours.Include(t => t.Reviews).First(t => t.Id == emptyTour.Id);
            }

            var action = controller.GetByTour(emptyTour.Id);
            var result = action.Result as OkObjectResult;
            var paged = result?.Value as PagedResult<TourReviewDto>;

            paged.ShouldNotBeNull();
            paged.Results.Count.ShouldBe(0);
            paged.TotalCount.ShouldBe(0);
        }

        [Fact]
        public void Get_fails_invalid_tour_id()
        {
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var invalidTourId = -999;

            Should.Throw<NotFoundException>(() =>
            {
                var actionResult = controller.GetByTour(invalidTourId).Result;
            });
        }

        private static TourReviewController CreateController(IServiceScope scope)
        {
            return new TourReviewController(scope.ServiceProvider.GetRequiredService<ITourService>())
            {
                ControllerContext = BuildContext("1") 
            };
        }
    }
}
