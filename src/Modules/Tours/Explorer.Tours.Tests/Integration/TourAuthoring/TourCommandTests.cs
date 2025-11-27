using Explorer.API.Controllers.Author;
using Explorer.BuildingBlocks.Core.Exceptions;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Infrastructure.Database;
using Explorer.Tours.Tests.Builders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using TourDifficulty = Explorer.Tours.Core.Domain.TourDifficulty;

namespace Explorer.Tours.Tests.Integration.Administration;

[Collection("Sequential")]
public class TourCommandTests : BaseToursIntegrationTest
{
    private void ShouldFailValidation(string field, object? value, bool isCreate = true)
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var updatedEntity = TourDtoBuilder.CreateValid();
        var prop = typeof(TourDto).GetProperty(field);
        prop.SetValue(updatedEntity, value);
        Type exception = field == "Difficulty" || field == "Status" ? typeof(AutoMapper.AutoMapperMappingException) : typeof(ArgumentException);

        // Act & Assert
        Should.Throw(() => isCreate ? controller.Create(updatedEntity) : controller.Update(updatedEntity), exception);
    }

    public TourCommandTests(ToursTestFactory factory) : base(factory) { }

    [Fact]
    public void Creates()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();
        var newEntity = TourDtoBuilder.CreateValid();

        // Act
        var result = ((ObjectResult)controller.Create(newEntity).Result)?.Value as TourDto;

        // Assert - Response
        result.ShouldNotBeNull();
        result.Id.ShouldNotBe(0);
        result.Name.ShouldBe(newEntity.Name);

        // Assert - Database
        var storedEntity = dbContext.Tours.FirstOrDefault(i => i.Name == newEntity.Name);
        storedEntity.ShouldNotBeNull();
        storedEntity.Id.ShouldBe(result.Id);
    }

    [Theory]
    [InlineData(nameof(TourDto.Name))]
    [InlineData(nameof(TourDto.AuthorId))]
    public void Create_fails_null_data(string field)
    {
        ShouldFailValidation(field, null);
    }

    [Theory]
    [InlineData(nameof(TourDto.Difficulty))]
    [InlineData(nameof(TourDto.Status))]
    public void Create_fails_invalid_enum_data(string field)
    {
        ShouldFailValidation(field, "Invalid enum value");
    }

    [Theory]
    [InlineData(nameof(TourDto.Tags))]
    [InlineData(nameof(TourDto.Price))]
    public void Create_fails_invalid_data(string field)
    {
        object value = field == "Price" ? -1 : new List<string> { "Nature", "Nature", "Viewpoints" };
        ShouldFailValidation(field, value);
    }

    [Fact]
    public void Updates()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();
        var updatedEntity = TourDtoBuilder.CreateValid();
        updatedEntity.Id = -3;

        // Act
        var result = ((ObjectResult)controller.Update(updatedEntity).Result)?.Value as TourDto;

        // Assert - Response
        result.ShouldNotBeNull();
        result.Id.ShouldBe(-3);
        result.Name.ShouldBe(updatedEntity.Name);
        result.Description.ShouldBe(updatedEntity.Description);

        // Assert - Database
        var storedEntity = dbContext.Tours.Single(i => i.Id == updatedEntity.Id);
        storedEntity.ShouldNotBeNull();
        storedEntity.Name.ShouldBe(updatedEntity.Name);
        storedEntity.Description.ShouldBe(updatedEntity.Description);
        storedEntity.Difficulty.ShouldBe(Enum.Parse<TourDifficulty>(updatedEntity.Difficulty));
        storedEntity.Status.ShouldBe(Enum.Parse<TourStatus>(updatedEntity.Status));
        storedEntity.AuthorId.ShouldBe(updatedEntity.AuthorId);
        storedEntity.Price.ShouldBe(updatedEntity.Price);
        storedEntity.Tags.ShouldBeEquivalentTo(updatedEntity.Tags);

        var oldEntity = dbContext.Tours.FirstOrDefault(i => i.Name == "Novi Sad Food & Culture Tour");
        oldEntity.ShouldBeNull();
    }

    [Fact]
    public void Update_fails_invalid_id()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var updatedEntity = TourDtoBuilder.CreateValid();
        updatedEntity.Id = -999;

        // Act & Assert
        Should.Throw<NotFoundException>(() => controller.Update(updatedEntity));
    }

    [Theory]
    [InlineData(nameof(TourDto.Name))]
    [InlineData(nameof(TourDto.AuthorId))]
    public void Update_fails_null_data(string field)
    {
        ShouldFailValidation(field, null, false);
    }

    [Theory]
    [InlineData(nameof(TourDto.Difficulty))]
    [InlineData(nameof(TourDto.Status))]
    public void Update_fails_invalid_enum_data(string field)
    {
        ShouldFailValidation(field, "Invalid enum value", false);
    }

    [Theory]
    [InlineData(nameof(TourDto.Tags))]
    [InlineData(nameof(TourDto.Price))]
    public void Update_fails_invalid_data(string field)
    {
        object value = field == "Price" ? -1 : new List<string> { "Nature", "Nature", "Viewpoints" };
        ShouldFailValidation(field, value, false);
    }

    [Fact]
    public void Deletes()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();

        // Act
        var result = (OkResult)controller.Delete(-1);

        // Assert - Response
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(200);

        // Assert - Database
        var storedCourse = dbContext.Tours.FirstOrDefault(i => i.Id == -1);
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

    private static TourController CreateController(IServiceScope scope)
    {
        return new TourController(scope.ServiceProvider.GetRequiredService<ITourService>())
        {
            ControllerContext = BuildContext("-1")
        };
    }
}