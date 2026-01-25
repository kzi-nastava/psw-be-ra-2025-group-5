using Explorer.API.Controllers.Shopping;
using Explorer.API.Controllers.Tours.Author;
using Explorer.API.Controllers.Tours.Tourist;
using Explorer.Payments.API.Dtos.PurchaseToken;
using Explorer.Payments.API.Dtos.ShoppingCart;
using Explorer.Payments.API.Public;
using Explorer.Payments.Infrastructure.Database;
using Explorer.Tours.API.Dtos.Tours;
using Explorer.Tours.API.Public.Tour;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Explorer.Payments.Tests.Integration.ShoppingCart;

[Collection("Sequential")]
public class ShoppingCartCommandTests : BasePaymentsIntegrationTest
{
    public ShoppingCartCommandTests(PaymentsTestFactory factory) : base(factory) { }

    [Fact]
    public void Creates()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<PaymentsContext>();
        var newEntity = new CreateShoppingCartDto { TouristId = -1 };

        // Act
        var result = ((ObjectResult)controller.Create(newEntity).Result)?.Value as ShoppingCartDto;

        // Assert - Response
        result.ShouldNotBeNull();
        result.Id.ShouldNotBe(0);
        result.TouristId.ShouldBe(newEntity.TouristId);

        // Assert - Database
        var storedEntity = dbContext.ShoppingCarts.FirstOrDefault(i => i.TouristId == newEntity.TouristId);
        storedEntity.ShouldNotBeNull();
        storedEntity.Id.ShouldBe(result.Id);
    }

    [Fact]
    public void Create_fails_duplicate_cart()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<PaymentsContext>();
        var newEntity = new CreateShoppingCartDto { TouristId = -2 };

        // Act & Assert
        Should.Throw<InvalidOperationException>(() => controller.Create(newEntity));
    }

    [Fact]
    public void Create_fails_invalid_data()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<PaymentsContext>();
        var newEntity = new CreateShoppingCartDto { TouristId = 0 };

        // Act & Assert
        Should.Throw<ArgumentException>(() => controller.Create(newEntity));
    }

    [Fact]
    public void Adds_item_to_cart()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<PaymentsContext>();
        var cartController = CreateController(scope);
        var tourController = CreateTourController(scope);

        // Act
        var result = ((ObjectResult)cartController.AddOrderItem(-4, -5).Result)?.Value as ShoppingCartDto;
        var tour = ((ObjectResult)tourController.Get(-5).Result)?.Value as TourDto;

        // Assert - Response
        result.ShouldNotBeNull();
        result.Id.ShouldBe(-2);
        result.TouristId.ShouldBe(-4);
        result.Total.ShouldBe(0);
        result.Items.Count.ShouldBe(2);
        result.Items.ShouldContain(i => i.TourId == tour.Id);

        // Assert - Database
        var storedEntity = dbContext.ShoppingCarts.Single(i => i.Id == -2);
        storedEntity.ShouldNotBeNull();
        storedEntity.Items.ShouldContain(i => i.TourId == tour.Id);
    }

    [Fact]
    public void Add_item_fails_item_already_in_cart()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<PaymentsContext>();
        var controller = CreateController(scope);

        // Act
        var result = controller.AddOrderItem(-4, -2).Result as ConflictObjectResult;

        // Assert
        result?.StatusCode.ShouldBe(409);
        result?.Value?.ToString().ShouldContain("Tour is already in the cart.");
    }

    [Fact]
    public void Removes_item_from_cart()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<PaymentsContext>();
        var cartController = CreateController(scope);
        var tourController = CreateTourController(scope);

        // Act
        var result = ((ObjectResult)cartController.RemoveOrderItem(-2, -2).Result)?.Value as ShoppingCartDto;
        var tour = ((ObjectResult)tourController.Get(-2).Result)?.Value as TourDto;

        // Assert - Response
        result.ShouldNotBeNull();
        result.Id.ShouldBe(-1);
        result.TouristId.ShouldBe(-2);
        result.Total.ShouldBe(2.5);
        result.Items.Count.ShouldBe(1);
        result.Items.ShouldNotContain(i => i.TourId == tour.Id);

        // Assert - Database
        var storedEntity = dbContext.ShoppingCarts.Single(i => i.Id == -1);
        storedEntity.ShouldNotBeNull();
        storedEntity.Items.ShouldNotContain(i => i.TourId == tour.Id);
    }

    [Fact]
    public void Remove_item_fails_item_not_in_cart()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<PaymentsContext>();
        var controller = CreateController(scope);

        var result = controller.RemoveOrderItem(-4, -1, null);

        result.Result.ShouldBeOfType<BadRequestObjectResult>();
        var badRequest = result.Result as BadRequestObjectResult;
        badRequest.Value.ToString().ShouldContain("Item not found in the cart");
    }

    [Fact]
    public void Checkout()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<PaymentsContext>();
        var cartController = CreateController(scope);
        var tokenController = CreateTokenController(scope);

        // Act
        var result = ((ObjectResult)cartController.Checkout(-5).Result)?.Value as ShoppingCartDto;
        var token = ((ObjectResult)tokenController.GetByTourAndTourist(-1, -5).Result)?.Value as TourPurchaseTokenDto;

        // Assert - Response
        result.ShouldNotBeNull();
        result.Id.ShouldNotBe(0);
        result.TouristId.ShouldBe(-5);
        result.Items.Count.ShouldBe(0);
        result.Total.ShouldBe(0);

        token.ShouldNotBeNull();
        token.TouristId.ShouldBe(-5);
        token.TourId.ShouldBe(-1);
    }

    [Fact]
    public void Checkout_with_archived_tour()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<PaymentsContext>();
        var cartController = CreateController(scope);
        var tokenController = CreateTokenController(scope);

        // Act
        cartController.AddOrderItem(-5, -4);
        var result = ((ObjectResult)cartController.Checkout(-5).Result)?.Value as ShoppingCartDto;
        var tokenResult = tokenController.GetByTourAndTourist(-4, -5).Result;
        var okResult = tokenResult as ObjectResult;
        var token = okResult?.Value;

        // Assert - Cart response
        result.ShouldNotBeNull();
        result.Id.ShouldNotBe(0);
        result.TouristId.ShouldBe(-5);
        result.Items.Count.ShouldBe(0);
        result.Total.ShouldBe(0);

        // Assert - Token
        okResult.ShouldNotBeNull();
        okResult.StatusCode.ShouldBe(200);
        token.ShouldBeNull(); 
    }
    
    [Fact]
    public void Checkout_fails_tour_already_bought()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<PaymentsContext>();
        var controller = CreateController(scope);

        // Act
        var result = controller.Checkout(-2);

        // Assert
        result.Result.ShouldBeOfType<BadRequestObjectResult>();
        var badRequest = result.Result as BadRequestObjectResult;
        badRequest.Value.ToString().ShouldContain("already own");
    }

    private static ShoppingCartController CreateController(IServiceScope scope)
    {
        return new ShoppingCartController(scope.ServiceProvider.GetRequiredService<IShoppingCartService>())
        {
            ControllerContext = BuildContext("-1")
        };
    }

    private static TourController CreateTourController(IServiceScope scope)
    {
        return new TourController(
            scope.ServiceProvider.GetRequiredService<ITourService>(),
            scope.ServiceProvider.GetRequiredService<ITourSearchHistoryService>())
        {
            ControllerContext = BuildContext("-1")
        };
    }

    private static TourPurchaseTokenController CreateTokenController(IServiceScope scope)
    {
        return new TourPurchaseTokenController(scope.ServiceProvider.GetRequiredService<ITourPurchaseTokenService>())
        {
            ControllerContext = BuildContext("-1")
        };
    }
}