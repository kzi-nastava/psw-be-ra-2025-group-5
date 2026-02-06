using Explorer.API.Controllers.Tours.Tourist;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos.Tours;
using Explorer.Tours.API.Public.Tour;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Explorer.Tours.Tests.Integration.TourAuthoring;

[Collection("Sequential")]
public class TourSuggestionsQueryTests : BaseToursIntegrationTest
{
    public TourSuggestionsQueryTests(ToursTestFactory factory) : base(factory) { }

    [Fact]
    public void GetSuggestions_ReturnsPagedTours_ForUserWithPreferences()
    {
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        var result = ((ObjectResult)controller.Get(1, 10).Result)?.Value as PagedResult<TourDto>;

        result.ShouldNotBeNull();
        result.Results.ShouldNotBeNull();
        result.TotalCount.ShouldBeGreaterThanOrEqualTo(0);
    }

    [Fact]
    public void GetSuggestions_ReturnsPagedResults()
    {
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        var result = ((ObjectResult)controller.Get(1, 2).Result)?.Value as PagedResult<TourDto>;

        result.ShouldNotBeNull();
        result.Results.Count.ShouldBeLessThanOrEqualTo(2);
        result.TotalCount.ShouldBeGreaterThanOrEqualTo(result.Results.Count);
    }

    private static TourSuggestionsController CreateController(IServiceScope scope)
    {
        return new TourSuggestionsController(
            scope.ServiceProvider.GetRequiredService<ITourSuggestionService>())
        {
            ControllerContext = BuildContext("-23")
        };
    }
}
