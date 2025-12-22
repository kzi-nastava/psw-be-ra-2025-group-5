using Explorer.API.Controllers.Shopping;
using Explorer.Tours.API.Dtos.Shoppings;
using Explorer.Tours.API.Public.Shopping;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Explorer.Tours.Tests.Integration.Shopping;

[Collection("Sequential")]
public class ShoppingCartQueryTests : BaseToursIntegrationTest
{
    public ShoppingCartQueryTests(ToursTestFactory factory) : base(factory) { }

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
        result.Total.ShouldBe(10.05);
    }

    private static ShoppingCartController CreateController(IServiceScope scope)
    {
        return new ShoppingCartController(scope.ServiceProvider.GetRequiredService<IShoppingCartService>())
        {
            ControllerContext = BuildContext("-1")
        };
    }
}