using Explorer.API.Controllers.Social;
using Explorer.BuildingBlocks.Core.Exceptions;
using Explorer.Stakeholders.API.Dtos.Diaries;
using Explorer.Stakeholders.API.Public.Diaries;
using Explorer.Stakeholders.Core.Domain.Diaries;
using Explorer.Stakeholders.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Explorer.Stakeholders.Tests.Integration.Diary;

[Collection("Sequential")]
public class DiaryCommandTests : BaseStakeholdersIntegrationTest
{
    public DiaryCommandTests(StakeholdersTestFactory factory) : base(factory) { }

    [Fact]
    public void Successfully_creates_diary()
    {
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope, "-21");
        var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();

        var dto = new DiaryDto
        {
            Name = "Novi dnevnik",
            Country = "Srbija",
            City = "Novi Sad",
            TouristId = -21
        };

        var result = ((ObjectResult)controller.Create(dto).Result)?.Value as DiaryDto;

        result.ShouldNotBeNull();
        result.Id.ShouldNotBe(0);
        result.Name.ShouldBe("Novi dnevnik");
        result.Country.ShouldBe("Srbija");
        result.City.ShouldBe("Novi Sad");
        result.TouristId.ShouldBe(-21);

        dbContext.ChangeTracker.Clear();
        var stored = dbContext.Diaries.Find(result.Id);
        stored.ShouldNotBeNull();
        stored.Name.ShouldBe("Novi dnevnik");
    }

    [Fact]
    public void Creates_diary_without_city()
    {
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope, "-21");

        var dto = new DiaryDto
        {
            Name = "Dnevnik bez grada",
            Country = "Crna Gora",
            City = null,
            TouristId = -21
        };

        var result = ((ObjectResult)controller.Create(dto).Result)?.Value as DiaryDto;

        result.ShouldNotBeNull();
        result.City.ShouldBeNull();
    }

    [Fact]
    public void Successfully_updates_diary()
    {
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope, "-21");
        var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();

        var dto = new DiaryDto
        {
            Id = -1,
            Name = "Izmenjen naziv",
            Country = "Hrvatska",
            City = "Zagreb",
            CreatedAt = DateTime.SpecifyKind(new DateTime(2024, 1, 15, 10, 0, 0), DateTimeKind.Utc),
            TouristId = -21
        };

        var result = ((ObjectResult)controller.Update(-1, dto).Result)?.Value as DiaryDto;

        result.ShouldNotBeNull();
        result.Id.ShouldBe(-1);
        result.Name.ShouldBe("Izmenjen naziv");
        result.Country.ShouldBe("Hrvatska");

        dbContext.ChangeTracker.Clear();
        var stored = dbContext.Diaries.Find(-1L);
        stored.ShouldNotBeNull();
        stored.Name.ShouldBe("Izmenjen naziv");
    }

    [Fact]
    public void Fails_update_when_diary_not_found()
    {
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope, "-21");

        var dto = new DiaryDto
        {
            Id = -999,
            Name = "Test",
            Country = "Srbija",
            TouristId = -21
        };

        Should.Throw<NotFoundException>(() => controller.Update(-999, dto));
    }

    [Fact]
    public void Successfully_deletes_diary()
    {
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope, "-21");
        var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();

        var createDto = new DiaryDto
        {
            Name = "Dnevnik za brisanje",
            Country = "Srbija",
            TouristId = -21
        };
        var created = ((ObjectResult)controller.Create(createDto).Result)?.Value as DiaryDto;
        var diaryId = created!.Id;

        var result = controller.Delete(diaryId);

        result.ShouldBeOfType<OkResult>();

        dbContext.ChangeTracker.Clear();
        var stored = dbContext.Diaries.Find(diaryId);
        stored.ShouldBeNull();
    }

    private static DiaryController CreateController(IServiceScope scope, string personId)
    {
        return new DiaryController(scope.ServiceProvider.GetRequiredService<IDiaryService>())
        {
            ControllerContext = BuildContext(personId)
        };
    }
}
