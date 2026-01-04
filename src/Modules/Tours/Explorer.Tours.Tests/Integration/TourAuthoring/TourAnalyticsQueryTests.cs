using AutoMapper;
using Explorer.API.Controllers.Author;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Payments.API.Internal;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public;
using Explorer.Tours.Core.UseCases;
using Explorer.Tours.Infrastructure.Database.Repositories;
using Explorer.Tours.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Explorer.Tours.Tests.Integration.TourAuthoring
{
    [Collection("Sequential")]
    public class TourAnalyticsQueryTests : BaseToursIntegrationTest
    {
        public TourAnalyticsQueryTests(ToursTestFactory factory) : base(factory) { }

        [Fact]
        public void GetToursCountByPrice_ReturnsData()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();
            var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();
            var tokenService = scope.ServiceProvider
                .GetRequiredService<ITourPurchaseTokenSharedService>();

            var repository = new TourStatisticsDbRepository(dbContext);
            var repositoryTour = new TourDbRepository(dbContext);

            var service = new TourStatisticsService(repository, mapper, tokenService, repositoryTour);

            var controller = new AuthorAnalyticsController(service)
            {
                ControllerContext = BuildContext("-11")
            };

            // Act
            var result = ((OkObjectResult)controller.GetToursCountByPrice(-11).Result)?
                .Value as IReadOnlyCollection<ToursByPriceDto>;

            // Assert
            result.ShouldNotBeNull();
            result.Count.ShouldBeGreaterThan(0);
            result.All(r => !string.IsNullOrEmpty(r.PriceRange)).ShouldBeTrue();
            result.All(r => r.Count >= 0).ShouldBeTrue();
        }

    }
}
