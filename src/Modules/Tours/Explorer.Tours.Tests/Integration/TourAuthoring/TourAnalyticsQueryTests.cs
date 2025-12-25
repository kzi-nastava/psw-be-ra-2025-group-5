using Explorer.API.Controllers.Author;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public;
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
            var controller = scope.ServiceProvider.GetRequiredService<AuthorAnalyticsController>();

            var userId = -11;

            // Act
            var result = ((OkObjectResult)controller.GetToursCountByPrice(userId).Result)?.Value
                         as IReadOnlyCollection<ToursByPriceDto>;

            // Assert
            result.ShouldNotBeNull();
            result.Count.ShouldBeGreaterThan(0);
            result.All(r => !string.IsNullOrEmpty(r.PriceRange)).ShouldBeTrue();
            result.All(r => r.Count >= 0).ShouldBeTrue();
        }
    }
}
