using Explorer.API.Controllers.Administrator.Administration;
using Explorer.BuildingBlocks.Core.Exceptions;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
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
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();
            var newEntity = new MonumentDto
            {
                Name = "Spomenik",
                Description = "Description",
                Year = 1945,
                Status = "Active",
                Location = new MonumentLocationDto
                {
                    Latitude = 44.815, 
                    Longitude = 20.460
                }
            };

            // Act
            var result = ((ObjectResult)controller.Create(newEntity).Result)?.Value as MonumentDto;

            // Assert - Response
            result.ShouldNotBeNull();
            result.Id.ShouldNotBe(0);
            result.Name.ShouldBe(newEntity.Name);

            // Assert - Database
            var storedEntity = dbContext.Monument.FirstOrDefault(i => i.Name == newEntity.Name);
            storedEntity.ShouldNotBeNull();
            storedEntity.Id.ShouldBe(result.Id);
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
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();
            var updatedEntity = new MonumentDto
            {
                Id = -1,
                Name = "Spomenik neznanom junaku",
                Description = "Opis",
                Year = 1945,
                Status = "Active",
                Location = new MonumentLocationDto
                {
                    Latitude = 44.815,
                    Longitude = 20.463
                }
            };

            // Act
            var result = ((ObjectResult)controller.Update(updatedEntity).Result)?.Value as MonumentDto;

            // Assert - Response
            result.ShouldNotBeNull();
            result.Id.ShouldBe(-1);
            result.Name.ShouldBe(updatedEntity.Name);
            result.Description.ShouldBe(updatedEntity.Description);

            // Assert - Database
            var storedEntity = dbContext.Monument.FirstOrDefault(i => i.Name == "Spomenik neznanom junaku");
            storedEntity.ShouldNotBeNull();
            storedEntity.Description.ShouldBe(updatedEntity.Description);
            var oldEntity = dbContext.Monument.FirstOrDefault(i => i.Name == "Spomenik");
            oldEntity.ShouldBeNull();
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

            // Act
            var result = (OkResult)controller.Delete(-3);

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
