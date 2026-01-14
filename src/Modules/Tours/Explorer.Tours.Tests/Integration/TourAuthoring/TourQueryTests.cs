using Explorer.API.Controllers.Tours.Author;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos.Tours;
using Explorer.Tours.API.Public.Tour;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Explorer.Tours.Tests.Integration.TourAuthoring;

[Collection("Sequential")]
public class TourQueryTests : BaseToursIntegrationTest
{
    public TourQueryTests(ToursTestFactory factory) : base(factory) { }

    [Fact]
    public void Retrieves_all_tours()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        // Act
        var result = ((ObjectResult)controller.GetAll(0, 0, null).Result)?.Value as PagedResult<TourDto>;

        // Assert
        result.ShouldNotBeNull();
        result.Results.Count.ShouldBe(6);
        result.TotalCount.ShouldBe(6);
    }

    [Fact]
    public void Retrieves_all_tours_by_author()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        // Act
        var result = ((ObjectResult)controller.GetAll(0, 0, -11).Result)?.Value as PagedResult<TourDto>;

        // Assert
        result.ShouldNotBeNull();
        result.Results.Count.ShouldBe(3);
        result.TotalCount.ShouldBe(3);
    }

    [Fact]
    public void Retrieves_all_tags()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        // Act
        var result = ((ObjectResult)controller.GetAllTags().Result)?.Value as List<string>;

        // Assert
        result.ShouldNotBeNull();
        result.Count.ShouldBe(7);
    }

    [Fact]
    public void Search_ReturnsTours_ForValidLocation()
    {
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        var result = ((ObjectResult)controller.Search(44.7866, 20.4489, 100, 1, 10, null, null, null, null, null, null).Result)?.Value as PagedResult<TourDto>;

        result.ShouldNotBeNull();
        result.Results.ShouldNotBeNull();
    }

    [Fact]
    public void Search_ReturnsFilteredTours_ForDifficulty()
    {
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        var result = ((ObjectResult)controller.Search(44.7866, 20.4489, 100, 1, 10, "Easy", null, null, null, null, null).Result)?.Value as PagedResult<TourDto>;

        result.ShouldNotBeNull();
        result.Results.ShouldNotBeNull();
    }

    [Fact]
    public void Search_ReturnsFilteredTours_ForPriceRange()
    {
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        var result = ((ObjectResult)controller.Search(44.7866, 20.4489, 100, 1, 10, null, 0, 100, null, null, null).Result)?.Value as PagedResult<TourDto>;

        result.ShouldNotBeNull();
        result.Results.ShouldNotBeNull();
    }

    [Fact]
    public void Search_ReturnsSortedTours_ForSortBy()
    {
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        var result = ((ObjectResult)controller.Search(44.7866, 20.4489, 100, 1, 10, null, null, null, null, "price", "asc").Result)?.Value as PagedResult<TourDto>;

        result.ShouldNotBeNull();
        result.Results.ShouldNotBeNull();
    }

    [Fact]
    public void Search_ReturnsPagedResults()
    {
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        var result = ((ObjectResult)controller.Search(44.7866, 20.4489, 100, 1, 2, null, null, null, null, null, null).Result)?.Value as PagedResult<TourDto>;

        result.ShouldNotBeNull();
        result.Results.Count.ShouldBeLessThanOrEqualTo(2);
        result.TotalCount.ShouldBeGreaterThanOrEqualTo(result.Results.Count);
    }

    private static TourController CreateController(IServiceScope scope)
    {
        return new TourController(
            scope.ServiceProvider.GetRequiredService<ITourService>(),
            scope.ServiceProvider.GetRequiredService<ITourSearchHistoryService>())
        {
            ControllerContext = BuildContext("-1")
        };
    }
}