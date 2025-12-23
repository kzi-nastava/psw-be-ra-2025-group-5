using Explorer.API.Controllers.Tourist.ProblemReporting;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.API.Public.Reporting;
using Explorer.Tours.API.Public;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System.Security.Claims;

namespace Explorer.Stakeholders.Tests.Integration.Reporting;

[Collection("Sequential")]
public class TourProblemQueryTests : BaseStakeholdersIntegrationTest
{
    public TourProblemQueryTests(StakeholdersTestFactory factory) : base(factory) { }

    [Fact]
    public void Retrieves_all()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        // Act
        var result = ((ObjectResult)controller.GetAll(0, 0).Result)?.Value as PagedResult<TourProblemDto>;

        // Assert
        result.ShouldNotBeOfType<ForbidResult>();
        result.Results.Count.ShouldBe(3);
        result.TotalCount.ShouldBe(3);
    }

    private static TourProblemController CreateController(IServiceScope scope)
    {
        var tourProblemService = scope.ServiceProvider.GetRequiredService<ITourProblemService>();
        var tourService = scope.ServiceProvider.GetRequiredService<ITourService>();
        var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();

        return new TourProblemController(tourProblemService, tourService, notificationService)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                    {
                        new Claim("id", "-1"),
                        new Claim("personId", "-1"),
                        new Claim(ClaimTypes.Role, "administrator")
                    }))
                }
            }
        };
    }
}