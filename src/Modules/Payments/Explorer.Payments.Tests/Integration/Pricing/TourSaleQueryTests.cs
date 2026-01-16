using Explorer.API.Controllers.Shopping;
using Explorer.Payments.API.Dtos.Pricing;
using Explorer.Payments.API.Public;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Explorer.Payments.Tests.Integration.Pricing;

[Collection("Sequential")]
public class TourSaleQueryTests : BasePaymentsIntegrationTest
{
    public TourSaleQueryTests(PaymentsTestFactory factory) : base(factory) { }

    [Fact]
    public void Retrieves_sale_by_id()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        // Act
        var result = ((ObjectResult)controller.Get(-1).Result)?.Value as TourSaleDto;

        // Assert
        result.ShouldNotBeNull();
        result.AuthorId.ShouldBe(-11);
        result.CreationDate.ToLocalTime().ToString("yyyy-MM-dd").ShouldBe("2026-01-04");
        result.ExpirationDate.ToLocalTime().ToString("yyyy-MM-dd").ShouldBe("2036-01-14");
        result.DiscountPercentage.ShouldBe(50u);
        result.TourIds.ShouldBe([-1]);
    }

    [Fact]
    public void Retrieves_active_sale_for_tour()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        // Act
        var result = ((ObjectResult)controller.GetActiveSaleForTour(-1).Result)?.Value as TourSaleDto;

        // Assert
        result.ShouldNotBeNull();
        result.AuthorId.ShouldBe(-11);
        result.CreationDate.ToLocalTime().ToString("yyyy-MM-dd").ShouldBe("2026-01-04");
        result.ExpirationDate.ToLocalTime().ToString("yyyy-MM-dd").ShouldBe("2036-01-14");
        result.DiscountPercentage.ShouldBe(50u);
        result.TourIds.ShouldBe([-1]);
    }

    [Fact]
    public void Retrieves_all_sales_by_author()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        // Act
        var result = ((ObjectResult)controller.GetByAuthor(-11).Result)?.Value as List<TourSaleDto>;

        // Assert
        result.ShouldNotBeNull();
        result.Count.ShouldBe(3);
    }

    [Fact]
    public void Retrieves_active_sales_by_author()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        // Act
        var result = ((ObjectResult)controller.GetByAuthor(-11, true).Result)?.Value as List<TourSaleDto>;

        // Assert
        result.ShouldNotBeNull();
        result.Count.ShouldBe(2);
    }

    [Fact]
    public void Retrieves_final_pricing()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        // Act
        var result = ((ObjectResult)controller.GetFinalPrice(-1).Result)?.Value as TourPriceDto;

        // Assert
        result.ShouldNotBeNull();
        result.BasePrice.ShouldBe(5);
        result.DiscountPercentage.ShouldBe(50u);
        result.FinalPrice.ShouldBe(2.5);
    }

    private static TourSaleController CreateController(IServiceScope scope)
    {
        return new TourSaleController(scope.ServiceProvider.GetRequiredService<ITourSaleService>())
        {
            ControllerContext = BuildContext("-1")
        };
    }
}
