using Explorer.API.Controllers.Tours.Tourist;
using Explorer.BuildingBlocks.Core.Exceptions;
using Explorer.Tours.API.Dtos.Equipments;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Explorer.Tours.Tests.Integration.Administration;

[Collection("Sequential")]
public class TouristEquipmentCommandTests : BaseToursIntegrationTest
{
    public TouristEquipmentCommandTests(ToursTestFactory factory) : base(factory) { }

    [Fact]
    public void Creates()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();

        dbContext.Database.EnsureDeleted();
        dbContext.Database.EnsureCreated();

        var controller = CreateController(scope, "-21");  //Oprema koju poseduje turista sa id-jem -21
        var newEntity = new TouristEquipmentDto
        {
            EquipmentId = -1
        };

        // Act
        var result = ((ObjectResult)controller.Create(newEntity).Result)?.Value as TouristEquipmentDto;

        // Assert - Response
        result.ShouldNotBeNull();
        result.Id.ShouldNotBe(0);
        result.TouristId.ShouldBe(-21);
        result.EquipmentId.ShouldBe(newEntity.EquipmentId);

        // Assert - Database
        var storedEntity = dbContext.TouristEquipment.FirstOrDefault(i => i.TouristId == -21 && i.EquipmentId == newEntity.EquipmentId);
        storedEntity.ShouldNotBeNull();
        storedEntity.Id.ShouldBe(result.Id);
    }

    [Fact]
    public void Create_fails_invalid_data()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope, "-21");

        var newEntity = new TouristEquipmentDto
        {
            EquipmentId = 0
        };

        // Act & Assert
        Should.Throw<ArgumentException>(() => controller.Create(newEntity));
    }

    [Fact]
    public void Updates()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();

        dbContext.Database.EnsureDeleted();
        dbContext.Database.EnsureCreated();

        var controller = CreateController(scope, "-21");

        var newEntity = new TouristEquipmentDto 
        { 
            EquipmentId = -1 
        };
        var createdEntity = ((ObjectResult)controller.Create(newEntity).Result)?.Value as TouristEquipmentDto;
        createdEntity.EquipmentId = -2;

        // Act
        var result = ((ObjectResult)controller.Update(createdEntity).Result)?.Value as TouristEquipmentDto;

        // Assert - Response
        result.ShouldNotBeNull();
        result.Id.ShouldBe(createdEntity.Id);
        result.EquipmentId.ShouldBe(-2);

        // Assert - Database
        var storedEntity = dbContext.TouristEquipment.FirstOrDefault(i => i.Id == createdEntity.Id);
        storedEntity.ShouldNotBeNull();
        storedEntity.EquipmentId.ShouldBe(-2);
    }

    [Fact]
    public void Update_fails_invalid_id()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope, "-21");

        var updatedEntity = new TouristEquipmentDto
        {
            Id = -1000,
            TouristId = -21,
            EquipmentId = -1
        };

        // Act & Assert
        Should.Throw<NotFoundException>(() => controller.Update(updatedEntity));
    }

    [Fact]
    public void Deletes()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();

        dbContext.Database.EnsureDeleted();
        dbContext.Database.EnsureCreated();

        var controller = CreateController(scope, "-21");
        var newEntity = new TouristEquipmentDto { EquipmentId = -1 };
        var createdEntity = ((ObjectResult)controller.Create(newEntity).Result)?.Value as TouristEquipmentDto;

        // Act
        var result = (OkResult)controller.Delete(createdEntity.Id);

        // Assert - Response
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(200);

        // Assert - Database
        var storedEntity = dbContext.TouristEquipment.FirstOrDefault(i => i.Id == createdEntity.Id);
        storedEntity.ShouldBeNull();
    }

    [Fact]
    public void Delete_fails_invalid_id()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope, "-21");

        // Act & Assert
        Should.Throw<NotFoundException>(() => controller.Delete(-1000));
    }

    private static TouristEquipmentController CreateController(IServiceScope scope, string personId)
    {
        return new TouristEquipmentController(scope.ServiceProvider.GetRequiredService<ITouristEquipmentService>())
        {
            ControllerContext = BuildContext(personId)
        };
    }
}