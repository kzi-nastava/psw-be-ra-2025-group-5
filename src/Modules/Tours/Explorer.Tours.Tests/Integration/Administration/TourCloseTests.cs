using Explorer.API.Controllers.Administrator.Administration;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using Explorer.Tours.API.Public;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System.Security.Claims;

namespace Explorer.Tours.Tests.Integration.Administration
{
    [Collection("Sequential")]
    public class TourCloseTests : BaseToursIntegrationTest
    {
        public TourCloseTests(ToursTestFactory factory) : base(factory) { }

        [Fact]
        public void Cannot_close_tour_if_no_expired_unresolved_problems()
        {
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope, "admin1", "administrator");
            long tourId = -1;
            var tourService = scope.ServiceProvider.GetRequiredService<ITourService>();

            // Act
            var result = controller.CloseTour(tourId);

            // Assert
            var badRequest = result as BadRequestObjectResult;
            badRequest.ShouldNotBeNull();
            badRequest.Value.ShouldBe("Cannot close tour: no unresolved problems with expired deadline.");

            var tour = tourService.GetById(tourId);
            tour.Status.ShouldNotBe("Closed");
        }

        [Fact]
        public void Cannot_close_already_closed_tour()
        {
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope, "admin1", "administrator");
            long tourId = -3;
            var tourService = scope.ServiceProvider.GetRequiredService<ITourService>();

            tourService.CloseTour(tourId);

            // Act
            var result = controller.CloseTour(tourId);

            // Assert
            var badRequest = result as BadRequestObjectResult;
            badRequest.ShouldNotBeNull();
            badRequest.Value.ShouldBe("Tour is already closed");
        }

        private static TourController CreateController(IServiceScope scope, string userId, string role)
        {
            var tourService = scope.ServiceProvider.GetRequiredService<ITourService>();
            var problemRepo = scope.ServiceProvider.GetRequiredService<ITourProblemRepository>();
            var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();

            return new TourController(tourService, problemRepo, notificationService)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                        {
                            new Claim("id", userId),
                            new Claim("personId", userId),
                            new Claim(ClaimTypes.Role, role)
                        }))
                    }
                }
            };
        }
    }
}
