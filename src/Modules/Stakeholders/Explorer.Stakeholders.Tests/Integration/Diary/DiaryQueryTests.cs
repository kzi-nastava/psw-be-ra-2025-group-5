using Explorer.API.Controllers.Tourist;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Explorer.Stakeholders.Tests.Integration.Diary;

[Collection("Sequential")]
public class DiaryQueryTests : BaseStakeholdersIntegrationTest
{
    public DiaryQueryTests(StakeholdersTestFactory factory) : base(factory) { }

    [Fact]
    public void Successfully_retrieves_diaries_by_tourist()
    {
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope, "-21");

        var result = ((ObjectResult)controller.GetAll(1, 10).Result)?.Value as PagedResult<DiaryDto>;

        result.ShouldNotBeNull();
        result.Results.ShouldNotBeNull();
        result.Results.Count.ShouldBeGreaterThan(0);
        result.Results.ShouldAllBe(d => d.TouristId == -21);
    }

    [Fact]
    public void Retrieves_empty_result_for_tourist_with_no_diaries()
    {
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope, "-22"); // turista2 nema dnevnike u testnim podacima

        var result = ((ObjectResult)controller.GetAll(1, 10).Result)?.Value as PagedResult<DiaryDto>;

        result.ShouldNotBeNull();
        result.Results.ShouldNotBeNull();
        result.Results.Count.ShouldBe(0);
        result.TotalCount.ShouldBe(0);
    }

    [Fact]
    public void Retrieves_diaries_ordered_by_created_at_descending()
    {
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope, "-21");

        var result = ((ObjectResult)controller.GetAll(1, 10).Result)?.Value as PagedResult<DiaryDto>;

        result.ShouldNotBeNull();
        result.Results.Count.ShouldBeGreaterThan(1);
        
        for (int i = 0; i < result.Results.Count - 1; i++)
        {
            result.Results[i].CreatedAt.ShouldBeGreaterThanOrEqualTo(result.Results[i + 1].CreatedAt);
        }
    }

    private static DiaryController CreateController(IServiceScope scope, string personId)
    {
        return new DiaryController(scope.ServiceProvider.GetRequiredService<IDiaryService>())
        {
            ControllerContext = BuildContext(personId)
        };
    }
}

