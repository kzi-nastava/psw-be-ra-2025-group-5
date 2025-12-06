using Explorer.API.Controllers.Tourist;
using Explorer.BuildingBlocks.Core.Exceptions;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public;
using Explorer.Tours.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

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
            var controller = CreateController(scope);
            var db = scope.ServiceProvider.GetRequiredService<ToursContext>();

            var tour = db.Tours.Include(t => t.Reviews).FirstOrDefault();
            tour.ShouldNotBeNull();

            if (tour.Reviews.Count < 3)
            {
                for (int i = 0; i < 3; i++)
                {
                    controller.Create(tour.Id, new TourReviewDto
                    {
                        TouristID = 1,
                        Grade = 5,
                        Comment = "Auto-gen",
                        Progress = 100
                    });
                }
                db.SaveChanges();
            }

            var action = controller.GetByTour(tour.Id, page: 1, pageSize: 2);
            var result = action.Result as OkObjectResult;
            var paged = result?.Value as PagedResult<TourReviewDto>;

            paged.ShouldNotBeNull();
            paged.Results.Count.ShouldBe(2);
            paged.TotalCount.ShouldBe(tour.Reviews.Count);

            // uporedi drugu "stranicu"
            var expectedIds = tour.Reviews
                .OrderBy(r => r.Id)
                .Skip(2)
                .Take(2)
                .Select(r => r.Id);

            paged.Results.Select(r => r.Id).ShouldBe(expectedIds);
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
                emptyTour = new Explorer.Tours.Core.Domain.Tour(
                    authorId: 1,
                    name: "Empty test tour",
                    description: "desc",
                    difficulty: Explorer.Tours.Core.Domain.TourDifficulty.Easy,
                    tags: new List<string>(),
                    price: 0
                );

                db.Tours.Add(emptyTour);
                db.SaveChanges();
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

            var action = controller.GetByTour(-999);
            var result = action.Result as NotFoundObjectResult;

            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(404);
        }

        private static TourReviewController CreateController(IServiceScope scope)
        {
            return new TourReviewController(
                scope.ServiceProvider.GetRequiredService<ITourService>()
            )
            {
                ControllerContext = BuildContext("1")
            };
        }
    }
}
