using Explorer.API.Controllers.Tourist;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Shopping;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Explorer.Tours.Tests.Integration.Shopping;

[Collection("Sequential")]
public class TourPurchaseTokenQueryTests : BaseToursIntegrationTest
{
    public TourPurchaseTokenQueryTests(ToursTestFactory factory) : base(factory) { }

    [Fact]
    public void Retrieves_token_by_tour_and_tourist()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        // Act
        var result = ((ObjectResult)controller.GetByTourAndTourist(-1, -21).Result)?.Value as TourPurchaseTokenDto;

        // Assert
        result.ShouldNotBeNull();
        result.Id.ShouldBe(-1);
        result.TouristId.ShouldBe(-21);
        result.TourId.ShouldBe(-1);
    }

    private static TourPurchaseTokenController CreateController(IServiceScope scope)
    {
        return new TourPurchaseTokenController(scope.ServiceProvider.GetRequiredService<ITourPurchaseTokenService>())
        {
            ControllerContext = BuildContext("-1")
        };
    }
}
