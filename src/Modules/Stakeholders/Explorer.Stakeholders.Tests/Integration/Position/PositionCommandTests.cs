using Explorer.API.Controllers.Tourist;
using Explorer.BuildingBlocks.Core.Exceptions;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Infrastructure.Database;
using Explorer.Stakeholders.Tests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Explorer.Stakeholders.Tests.Integration.Position;

[Collection("Sequential")]
public class PositionCommandTests : BaseStakeholdersIntegrationTest
{
    private void ShouldFailValidation(string field, object? value, bool isCreate = true)
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var updatedEntity = new PositionDto
        {
            TouristId = -23,
            Latitude = 47.123456,
            Longitude = 19.654321
        };
        var prop = typeof(PositionDto).GetProperty(field);
        prop.SetValue(updatedEntity, value);
        Type exception = field == "Longitude" || field == "Latitude" ? typeof(ArgumentOutOfRangeException) : typeof(ArgumentException);

        // Act & Assert
        Should.Throw(() => isCreate ? controller.Create(updatedEntity) : controller.Update(updatedEntity), exception);
    }

    public PositionCommandTests(StakeholdersTestFactory factory) : base(factory) { }

    [Fact]
    public void Creates()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();
        var newEntity = new PositionDto
        {
            TouristId = -23,
            Latitude = 47.123456,
            Longitude = 19.654321
        };

        // Act
        var result = ((ObjectResult)controller.Create(newEntity).Result)?.Value as PositionDto;

        // Assert - Response
        result.ShouldNotBeNull();
        result.Id.ShouldNotBe(0);
        result.TouristId.ShouldBe(newEntity.TouristId);

        // Assert - Database
        var storedEntity = dbContext.Positions.FirstOrDefault(i => i.TouristId == newEntity.TouristId);
        storedEntity.ShouldNotBeNull();
        storedEntity.Id.ShouldBe(result.Id);
    }

    [Fact]
    public void Create_fails_null_data()
    {
        ShouldFailValidation(nameof(PositionDto.TouristId), null);
    }

    [Theory]
    [InlineData(nameof(PositionDto.Latitude))]
    [InlineData(nameof(PositionDto.Longitude))]
    public void Create_fails_out_of_range_data(string field)
    {
        ShouldFailValidation(field, 190);
    }

    [Fact]
    public void Updates()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();
        var updatedEntity = new PositionDto
        {
            Id = -1,
            TouristId = -21,
            Latitude = 47.123456,
            Longitude = 19.654321
        };

        // Act
        var result = ((ObjectResult)controller.Update(updatedEntity).Result)?.Value as PositionDto;

        // Assert - Response
        result.ShouldNotBeNull();
        result.Id.ShouldBe(-1);
        result.TouristId.ShouldBe(updatedEntity.TouristId);
        result.Latitude.ShouldBe(updatedEntity.Latitude);
        result.Longitude.ShouldBe(updatedEntity.Longitude);

        // Assert - Database
        var storedEntity = dbContext.Positions.Single(i => i.Id == updatedEntity.Id);
        storedEntity.ShouldNotBeNull();
        storedEntity.TouristId.ShouldBe(updatedEntity.TouristId);
        storedEntity.Latitude.ShouldBe(updatedEntity.Latitude);
        storedEntity.Longitude.ShouldBe(updatedEntity.Longitude);

        var oldEntity = dbContext.Positions.FirstOrDefault(i => i.Latitude == 41.422);
        oldEntity.ShouldBeNull();
    }

    [Fact]
    public void Update_fails_invalid_id()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var updatedEntity = new PositionDto
        {
            Id = -999,
            TouristId = -23,
            Latitude = 47.123456,
            Longitude = 19.654321
        };

        // Act & Assert
        Should.Throw<NotFoundException>(() => controller.Update(updatedEntity));
    }

    [Fact]
    public void Update_fails_null_data()
    {
        ShouldFailValidation(nameof(PositionDto.TouristId), null, false);
    }

    [Theory]
    [InlineData(nameof(PositionDto.Latitude))]
    [InlineData(nameof(PositionDto.Longitude))]
    public void Update_fails_out_of_range_data(string field)
    {
        ShouldFailValidation(field, 190, false);
    }

    private static PositionController CreateController(IServiceScope scope)
    {
        return new PositionController(scope.ServiceProvider.GetRequiredService<IPositionService>())
        {
            ControllerContext = BuildContext("-1")
        };
    }
}