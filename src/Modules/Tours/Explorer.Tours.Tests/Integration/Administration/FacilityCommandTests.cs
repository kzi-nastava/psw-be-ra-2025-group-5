using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    public class FacilityCommandTests : BaseToursIntegrationTest
    {
        public FacilityCommandTests(ToursTestFactory factory) : base(factory) { }

        [Fact]
        public void Creates()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();
            var newEntity = new FacilityDto
            {
                Name = "Planinarski dom",
                Latitude = 45.123456,
                Longitude = 15.123456,
                Type = 1
            };

            // Act
            var result = ((ObjectResult)controller.Create(newEntity).Result)?.Value as FacilityDto;

            // Assert - Response
            result.ShouldNotBeNull();
            result.Id.ShouldNotBe(0);
            result.Name.ShouldBe(newEntity.Name);
            result.Latitude.ShouldBe(newEntity.Latitude);
            result.Longitude.ShouldBe(newEntity.Longitude);
            result.Type.ShouldBe(newEntity.Type);

            // Assert - Database
            var storedEntity = dbContext.Facilities.FirstOrDefault(i => i.Name == newEntity.Name);
            storedEntity.ShouldNotBeNull();
            storedEntity.Id.ShouldBe(result.Id);
        }

        [Fact]
        public void Create_fails_invalid_data()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var updatedEntity = new FacilityDto
            {
                Latitude = 45.123456,
                Longitude = 15.123456,
                Type = 1
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
            var updatedEntity = new FacilityDto
            {
                Id = -1,
                Name = "Planinarski dom Stara planina",
                Latitude = 46.654321,
                Longitude = 16.654321,
                Type = 2
            };

            // Act
            var result = ((ObjectResult)controller.Update(updatedEntity).Result)?.Value as FacilityDto;

            // Assert - Response
            result.ShouldNotBeNull();
            result.Id.ShouldBe(-1);
            result.Name.ShouldBe(updatedEntity.Name);
            result.Latitude.ShouldBe(updatedEntity.Latitude);
            result.Longitude.ShouldBe(updatedEntity.Longitude);
            result.Type.ShouldBe(updatedEntity.Type);

            // Assert - Database
            var storedEntity = dbContext.Facilities.FirstOrDefault(i => i.Name == "Planinarski dom Stara planina");
            storedEntity.ShouldNotBeNull();
            storedEntity.Latitude.ShouldBe(updatedEntity.Latitude);
            storedEntity.Longitude.ShouldBe(updatedEntity.Longitude);
            ((int)storedEntity.Type).ShouldBe(updatedEntity.Type);
            var oldEntity = dbContext.Facilities.FirstOrDefault(i => i.Name == "Planinarski dom Kopaonik");
            oldEntity.ShouldBeNull();
        }

        [Fact]
        public void Update_fails_invalid_id()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var updatedEntity = new FacilityDto
            {
                Id = -1000,
                Name = "Test",
                Latitude = 0,
                Longitude = 0,
                Type = 0
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
            var result = (OkResult)controller.Delete(-4);

            // Assert - Response
            result.ShouldNotBeNull();
            result.StatusCode.ShouldBe(200);

            // Assert - Database
            var storedCourse = dbContext.Facilities.FirstOrDefault(i => i.Id == -4);
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

        private static FacilityController CreateController(IServiceScope scope)
        {
            return new FacilityController(scope.ServiceProvider.GetRequiredService<IFacilityService>())
            {
                ControllerContext = BuildContext("-1")
            };
        }
    }
}
