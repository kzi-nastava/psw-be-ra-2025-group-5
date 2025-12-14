using Explorer.API.Controllers.Administrator.Administration;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using System.Linq;
using System.Security.Claims;
using Xunit;

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
            var tourRepository = scope.ServiceProvider.GetRequiredService<ITourRepository>();

            // Act
            var result = controller.CloseTour(tourId);

            // Assert
            var badRequest = result as BadRequestObjectResult;
            badRequest.ShouldNotBeNull();
            badRequest.Value.ShouldBe("Cannot close tour: no unresolved problems with expired deadline.");

            var tour = tourRepository.Get(tourId);
            tour.Status.ShouldNotBe(TourStatus.Closed);
        }

        [Fact]
        public void Cannot_close_already_closed_tour()
        {
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope, "admin1", "administrator");
            long tourId = -3;
            var tourRepository = scope.ServiceProvider.GetRequiredService<ITourRepository>();

            tourRepository.Close(tourId);

            // Act
            var result = controller.CloseTour(tourId);

            // Assert
            var badRequest = result as BadRequestObjectResult;
            badRequest.ShouldNotBeNull();
            badRequest.Value.ShouldBe("Tour is already closed");
        }

        private static TourController CreateController(IServiceScope scope, string userId, string role)
        {
            var tourRepo = scope.ServiceProvider.GetRequiredService<ITourRepository>();
            var problemRepo = scope.ServiceProvider.GetRequiredService<ITourProblemRepository>();

            return new TourController(tourRepo, problemRepo)
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
