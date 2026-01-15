using Explorer.API.Controllers.Shopping;
using Explorer.BuildingBlocks.Core.Exceptions;
using Explorer.Payments.API.Dtos.Pricing;
using Explorer.Payments.API.Public;
using Explorer.Payments.Infrastructure.Database;
using Explorer.Payments.Tests.Builders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Explorer.Payments.Tests.Integration.Pricing;

[Collection("Sequential")]
public class TourSaleCommandTests : BasePaymentsIntegrationTest
{
    public TourSaleCommandTests(PaymentsTestFactory factory) : base(factory) { }

    [Fact]
    public void Creates()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<PaymentsContext>();
        var newEntity = TourSaleDtoBuilder.CreateValidCreateDto();

        // Act
        var result = ((ObjectResult)controller.Create(newEntity).Result)?.Value as TourSaleDto;

        // Assert - Response
        result.ShouldNotBeNull();
        result.Id.ShouldNotBe(0);
        result.AuthorId.ShouldBe(newEntity.AuthorId);
        result.TourIds.ShouldBe(newEntity.TourIds);
        result.ExpirationDate.ShouldBe(newEntity.ExpirationDate);
        result.DiscountPercentage.ShouldBe(newEntity.DiscountPercentage);

        // Assert - Database
        var storedEntity = dbContext.TourSales.FirstOrDefault(i => i.Id == result.Id);
        storedEntity.ShouldNotBeNull();
        storedEntity.AuthorId.ShouldBe(newEntity.AuthorId);
        storedEntity.TourIds.ShouldBe(newEntity.TourIds);
        storedEntity.ExpirationDate.ShouldBe(newEntity.ExpirationDate);
        storedEntity.DiscountPercentage.ShouldBe(newEntity.DiscountPercentage);
    }

    [Fact]
    public void Create_fails_tour_already_on_sale()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<PaymentsContext>();
        var newEntity = TourSaleDtoBuilder.CreateValidCreateDto();
        newEntity.TourIds = [-1];

        // Act & Assert
        Should.Throw<InvalidOperationException>(() => controller.Create(newEntity));
    }

    [Fact]
    public void Create_fails_tour_from_different_author()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<PaymentsContext>();
        var newEntity = TourSaleDtoBuilder.CreateValidCreateDto();
        newEntity.TourIds = [-6];

        // Act & Assert
        Should.Throw<InvalidOperationException>(() => controller.Create(newEntity));
    }

    [Fact]
    public void Updates()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<PaymentsContext>();
        var updatedEntity = TourSaleDtoBuilder.CreateValidRegularDto();
        updatedEntity.DiscountPercentage = 45;

        // Act
        var result = ((ObjectResult)controller.Update(-1, updatedEntity).Result)?.Value as TourSaleDto;

        // Assert - Response
        result.ShouldNotBeNull();
        result.Id.ShouldBe(-1);
        result.AuthorId.ShouldBe(updatedEntity.AuthorId);
        result.TourIds.ShouldBe(updatedEntity.TourIds);
        result.ExpirationDate.ShouldBe(updatedEntity.ExpirationDate);
        result.CreationDate.ShouldBe(updatedEntity.CreationDate);
        result.DiscountPercentage.ShouldBe(updatedEntity.DiscountPercentage);

        // Assert - Database
        var storedEntity = dbContext.TourSales.Single(i => i.Id == -1);
        storedEntity.ShouldNotBeNull();
        storedEntity.AuthorId.ShouldBe(updatedEntity.AuthorId);
        storedEntity.TourIds.ShouldBe(updatedEntity.TourIds);
        storedEntity.ExpirationDate.ShouldBe(updatedEntity.ExpirationDate);
        storedEntity.DiscountPercentage.ShouldBe(updatedEntity.DiscountPercentage);
    }

    [Fact]
    public void Update_fails_invalid_id()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var updatedEntity = TourSaleDtoBuilder.CreateValidRegularDto();

        // Act & Assert
        Should.Throw<NotFoundException>(() => controller.Update(-999, updatedEntity));
    }

    [Fact]
    public void Update_fails_tour_already_on_sale()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var updatedEntity = TourSaleDtoBuilder.CreateValidRegularDto();
        updatedEntity.TourIds = [-2];

        // Act & Assert
        Should.Throw<InvalidOperationException>(() => controller.Update(-1, updatedEntity));
    }

    [Fact]
    public void Update_fails_tour_from_different_author()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var updatedEntity = TourSaleDtoBuilder.CreateValidRegularDto();
        updatedEntity.TourIds = [-5];

        // Act & Assert
        Should.Throw<InvalidOperationException>(() => controller.Update(-3, updatedEntity));
    }

    [Fact]
    public void Deletes()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<PaymentsContext>();

        // Act
        var result = (OkResult)controller.Delete(-1);

        // Assert - Response
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(200);

        // Assert - Database
        var storedCourse = dbContext.Payments.FirstOrDefault(i => i.Id == -1);
        storedCourse.ShouldBeNull();
    }

    [Fact]
    public void Delete_fails_invalid_id()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        // Act & Assert
        Should.Throw<NotFoundException>(() => controller.Delete(-1000));
    }

    private static TourSaleController CreateController(IServiceScope scope)
    {
        return new TourSaleController(scope.ServiceProvider.GetRequiredService<ITourSaleService>())
        {
            ControllerContext = BuildContext("-1")
        };
    }
}
