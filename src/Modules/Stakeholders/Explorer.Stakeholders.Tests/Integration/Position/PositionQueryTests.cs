using Explorer.API.Controllers.Tourist;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Explorer.Stakeholders.Tests.Integration.Position;

[Collection("Sequential")]
public class PositionQueryTests : BaseStakeholdersIntegrationTest
{
    public PositionQueryTests(StakeholdersTestFactory factory) : base(factory) { }

    [Fact]
    public void Retrieves_all_positions()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        // Act
        var result = ((ObjectResult)controller.GetAll().Result)?.Value as List<PositionDto>;

        // Assert
        result.ShouldNotBeNull();
        result.Count.ShouldBe(2);
    }

    [Fact]
    public void Retrieves_position_by_tourist()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        // Act
        var result = ((ObjectResult)controller.GetByTourist(-21).Result)?.Value as PositionDto;

        // Assert
        result.ShouldNotBeNull();
    }

    private static PositionController CreateController(IServiceScope scope)
    {
        return new PositionController(scope.ServiceProvider.GetRequiredService<IPositionService>())
        {
            ControllerContext = BuildContext("-1")
        };
    }
}