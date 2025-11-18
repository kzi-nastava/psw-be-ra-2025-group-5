using Explorer.API.Controllers.Administrator.Administration;
using Explorer.BuildingBlocks.Core.Exceptions;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Explorer.Tours.Tests.Integration.Administration
{
    [Collection("Sequential")]
    public class MonumentCommandTests: BaseToursIntegrationTest
    {
        public MonumentCommandTests(ToursTestFactory factory) : base(factory) { }

        [Fact]
        public void Creates()
        {
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();

            var newEntity = new MonumentDto
            {
                Name = "Spomenik",
                Description = "Description",
                Year = 1945,
                Status = 0,
                Location = new MonumentLocationDto { Latitude = 44.815, Longitude = 20.460 }
            };

            // Act
            var result = ((ObjectResult)controller.Create(newEntity).Result)?.Value as MonumentDto;

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldNotBe(0);
            result.Name.ShouldBe(newEntity.Name);

            var storedEntity = dbContext.Monument.FirstOrDefault(i => i.Id == result.Id);
            storedEntity.ShouldNotBeNull();
            storedEntity.Name.ShouldBe(newEntity.Name);
        }

        [Fact]
        public void Create_fails_invalid_data()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var updatedEntity = new MonumentDto
            {
                Description = "Test"
            };

            // Act & Assert
            Should.Throw<ArgumentException>(() => controller.Create(updatedEntity));
        }

        [Fact]
        public void Updates()
        {
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();

            // Uzmi prvi entitet iz baze za update
            var existingEntity = dbContext.Monument.AsNoTracking().FirstOrDefault();
            existingEntity.ShouldNotBeNull("Nema podataka u bazi za testiranje.");

            var updatedDto = new MonumentDto
            {
                Id = existingEntity.Id,
                Name = "Novo ime",
                Description = "Opis",
                Year = existingEntity.Year,
                Status = MonumentStatus.Active,
                Location = new MonumentLocationDto
                {
                    Latitude = existingEntity.Location.Latitude,
                    Longitude = existingEntity.Location.Longitude
                }
            };

            var result = ((ObjectResult)controller.Update(updatedDto).Result)?.Value as MonumentDto;

            result.ShouldNotBeNull();
            result.Id.ShouldBe(existingEntity.Id);
            result.Name.ShouldBe(updatedDto.Name);
            result.Description.ShouldBe(updatedDto.Description);

            var storedEntity = dbContext.Monument.FirstOrDefault(i => i.Id == existingEntity.Id);
            storedEntity.ShouldNotBeNull();
            storedEntity.Name.ShouldBe(updatedDto.Name);
        }

        [Fact]
        public void Update_fails_invalid_id()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var updatedEntity = new MonumentDto
            {
                Id = -1000,
                Name = "Test"
            };

            // Act & Assert
            Should.Throw<NotFoundException>(() => controller.Update(updatedEntity));
        }

        [Fact]
        public void Deletes()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();
            var firstMonument = dbContext.Monument.FirstOrDefault();
            if (firstMonument == null)
                throw new Exception("Nema podataka u bazi za testiranje.");

            var existingId = firstMonument.Id;
            // Act
            var result = (OkResult)controller.Delete(existingId);

            // Assert - Response
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(200);

            // Assert - Database
            var storedCourse = dbContext.Monument.FirstOrDefault(i => i.Id == -3);
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

        private static MonumentController CreateController(IServiceScope scope)
        {
            return new MonumentController(scope.ServiceProvider.GetRequiredService<IMonumentService>())
            {
                ControllerContext = BuildContext("-1")
            };
        }
    }
}
