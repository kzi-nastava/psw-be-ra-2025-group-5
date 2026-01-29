using Explorer.API.Controllers.Tours.Tourist;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos.Equipments;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.API.Public.Tour;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Explorer.Tours.Tests.Integration.Administration;

[Collection("Sequential")]
public class TouristEquipmentQueryTests : BaseToursIntegrationTest
{
    public TouristEquipmentQueryTests(ToursTestFactory factory) : base(factory) { }

    [Fact]
    public void Retrieves_all()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope, "-21");

        // Act
        var result = ((ObjectResult)controller.GetOwnedEquipment(0, 0).Result)?.Value as PagedResult<TouristEquipmentDto>;

        // Assert
        result.ShouldNotBeNull();
        result.Results.Count.ShouldBe(2);
        result.TotalCount.ShouldBe(2);
    }

    private static TouristEquipmentController CreateController(IServiceScope scope, string personId)
    {
         return new TouristEquipmentController(
         scope.ServiceProvider.GetRequiredService<ITouristEquipmentService>(),
         scope.ServiceProvider.GetRequiredService<ITourService>()
        )
        {
            ControllerContext = BuildContext(personId)
        };

    }
}