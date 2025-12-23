using Explorer.API.Controllers.Tourist;
using Explorer.Payments.API.Dtos;
using Explorer.Payments.API.Public;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Explorer.Payments.Tests.Integration;

[Collection("Sequential")]
public class ShoppingCartQueryTests : BasePaymentsIntegrationTest
{
    public ShoppingCartQueryTests(PaymentsTestFactory factory) : base(factory) { }

    [Fact]
    public void Retrieves_shopping_cart_by_tourist()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        // Act
        var result = ((ObjectResult)controller.GetByTourist(-2).Result)?.Value as ShoppingCartDto;

        // Assert
        result.ShouldNotBeNull();
        result.Items.Count.ShouldBe(2);
        result.Items[0].TourId.ShouldBe(-1);
        result.Items[1].TourId.ShouldBe(-2);
        result.Total.ShouldBe(5);
    }

    private static ShoppingCartController CreateController(IServiceScope scope)
    {
        return new ShoppingCartController(scope.ServiceProvider.GetRequiredService<IShoppingCartService>())
        {
            ControllerContext = BuildContext("-1")
        };
    }
}