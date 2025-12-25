using Explorer.API.Controllers.Tourist;
using Explorer.Payments.API.Dtos;
using Explorer.Payments.API.Public;
using Explorer.Payments.Tests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Explorer.Payments.Tests.Integration;

[Collection("Sequential")]
public class TourPurchaseTokenQueryTests : BasePaymentsIntegrationTest
{
    public TourPurchaseTokenQueryTests(PaymentsTestFactory factory) : base(factory) { }

    [Fact]
    public void Retrieves_token_by_tour_and_tourist()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        // Act
        var result = ((ObjectResult)controller.GetByTourAndTourist(-1, -2).Result)?.Value as TourPurchaseTokenDto;

        // Assert
        result.ShouldNotBeNull();
        result.Id.ShouldBe(-1);
        result.TouristId.ShouldBe(-2);
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
