using Explorer.API.Controllers.Tours.Author;
using Explorer.Payments.API.Dtos;
using Explorer.Payments.API.Public;
using Explorer.Payments.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Explorer.Payments.Tests.Integration
{
    [Collection("Sequential")]
    public class BundleCommandTests : BasePaymentsIntegrationTest
    {
        public BundleCommandTests(PaymentsTestFactory factory) : base(factory) { }

        [Fact]
        public void Creates()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<PaymentsContext>();

            var newEntity = new BundleDto
            {
                Name = "Test Bundle",
                Price = 100.0,
                TourIds = new List<long> { -1, -2 }
            };

            // Act
            var result = ((ObjectResult)controller.Create(-11, newEntity).Result)?.Value as BundleDto;

            // Assert - Response
            result.ShouldNotBeNull();
            result.Id.ShouldNotBe(0);
            result.Name.ShouldBe(newEntity.Name);
            result.AuthorId.ShouldBe(-11);

            // Assert - Database
            var storedEntity = dbContext.Bundles.FirstOrDefault(b => b.Id == result.Id);
            storedEntity.ShouldNotBeNull();
            storedEntity.BundleItems.Count.ShouldBe(2);
        }

        [Fact]
        public void Updates()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<PaymentsContext>();

            var updatedEntity = new BundleDto
            {
                Name = "Updated Spring Bundle",
                Price = 110.0,
                AuthorId = -11,
                Status = "Draft",
                TourIds = new List<long> { -1, -3 }
            };

            // Act
            var result = ((ObjectResult)controller.Update(-3, updatedEntity).Result)?.Value as BundleDto;

            // Assert
            result.ShouldNotBeNull();
            result.Name.ShouldBe("Updated Spring Bundle");

            var storedEntity = dbContext.Bundles.FirstOrDefault(b => b.Id == -3);
            storedEntity.Name.ShouldBe("Updated Spring Bundle");
        }

        [Fact]
        public void Deletes()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<PaymentsContext>();

            // Act
            var result = controller.Delete(-1);

            // Assert
            result.ShouldBeOfType<OkResult>();
            dbContext.Bundles.FirstOrDefault(b => b.Id == -1).ShouldBeNull();
        }

        [Fact]
        public void Publish_fails_if_less_than_two_published_tours()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);

            // Act & Assert
            Should.Throw<InvalidOperationException>(() => controller.Publish(-4));
        }

        [Fact]
        public void Archives_bundle()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<PaymentsContext>();

            // Act
            var result = ((ObjectResult)controller.Archive(-2).Result)?.Value as BundleDto;

            // Assert
            result.ShouldNotBeNull();
            result.Status.ShouldBe("Archived");

            var storedEntity = dbContext.Bundles.FirstOrDefault(b => b.Id == -2);
            storedEntity.Status.ShouldBe(Core.Domain.BundleStatus.Archived);
        }

        private static BundleController CreateController(IServiceScope scope)
        {
            return new BundleController(scope.ServiceProvider.GetRequiredService<IBundleService>())
            {
                ControllerContext = BuildContext("-11")
            };
        }
    }
}