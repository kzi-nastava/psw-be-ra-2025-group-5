using Explorer.API.Controllers.Administrator;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos.Monuments;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Explorer.Tours.Tests.Integration.Administration
{
    [Collection("Sequential")]
    public class MonumentQueryTests: BaseToursIntegrationTest
    {
        public MonumentQueryTests(ToursTestFactory factory) : base(factory) { }

        [Fact]
        public void Retrieves_all()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ToursContext>();

            var monuments = db.Monument.ToList();
            Console.WriteLine("COUNT = " + monuments.Count);
            foreach (var m in monuments)
                Console.WriteLine($"{m.Id} - {m.Name}");
            var controller = CreateController(scope);

            // Act
            var result = ((ObjectResult)controller.GetAll(0, 10).Result)?.Value as PagedResult<MonumentDto>;

            // Assert
            result.ShouldNotBeNull();
            result.Results.Count.ShouldBe(3);
            result.TotalCount.ShouldBe(3);
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
