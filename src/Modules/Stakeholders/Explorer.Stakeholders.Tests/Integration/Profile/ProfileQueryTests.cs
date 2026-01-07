using Explorer.API.Controllers.Profile;
using Explorer.BuildingBlocks.Core.Exceptions;
using Explorer.Stakeholders.API.Dtos.Users;
using Explorer.Stakeholders.API.Public.Users;
using Explorer.Stakeholders.Infrastructure.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System.Security.Claims;

namespace Explorer.Stakeholders.Tests.Integration.Profile;

[Collection("Sequential")]
public class ProfileQueryTests : BaseStakeholdersIntegrationTest
{
    public ProfileQueryTests(StakeholdersTestFactory factory) : base(factory) { }

    [Fact]
    public void Successfully_retrieves_profile()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        // Act
        var result = ((ObjectResult)controller.GetProfile(-21).Result)?.Value as ProfileDto;

        // Assert
        result.ShouldNotBeNull();
        result.Id.ShouldBe(-21);
        result.Name.ShouldBe("Pera");
        result.Surname.ShouldBe("Perić");
        result.Email.ShouldBe("turista1@gmail.com");
        result.Biography.ShouldBe("Biografija Pere Perića");
        result.Motto.ShouldBe("Moto Pere");
    }

    [Fact]
    public void Get_profile_fails_for_unauthorized_user()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        // Act - Pokušaj da dobiješ profil usera -22 dok je ulogovan -21
        var result = controller.GetProfile(-22).Result;

        // Assert
        result.ShouldBeOfType<ForbidResult>();
    }

    [Fact]
    public void Successfully_retrieves_public_profile_for_regular_user()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        // Act
        var result = ((ObjectResult)controller.GetPublicProfile(-21).Result)?.Value as ProfileDto;

        // Assert
        result.ShouldNotBeNull();
        result.Id.ShouldBe(-21);
        result.Name.ShouldBe("Pera");
        result.Surname.ShouldBe("Perić");
        result.Email.ShouldBe("turista1@gmail.com");
        result.Biography.ShouldBe("Biografija Pere Perića");
        result.Motto.ShouldBe("Moto Pere");
    }

    [Fact]
    public void Get_public_profile_returns_not_found_for_nonexistent_user()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        // Act
        var result = controller.GetPublicProfile(-999).Result;

        // Assert
        result.ShouldBeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public void Successfully_retrieves_paged_profiles()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IProfileService>();

        int page = 1;
        int pageSize = 2;

        // Act
        var pagedResult = service.GetPaged(page, pageSize);

        // Assert
        pagedResult.ShouldNotBeNull();
        pagedResult.Results.Count.ShouldBeLessThanOrEqualTo(pageSize);
        pagedResult.TotalCount.ShouldBeGreaterThan(0);

        foreach (var profile in pagedResult.Results)
        {
            profile.ShouldNotBeNull();
            profile.Id.ShouldBeLessThan(0);
            profile.Name.ShouldNotBeNullOrEmpty();
            profile.Surname.ShouldNotBeNullOrEmpty();

            profile.Statistics.ShouldNotBeNull();
        }

        var firstProfile = pagedResult.Results.First();
        firstProfile.Name.ShouldBe("Ana");
    }



    private static ProfileController CreateController(IServiceScope scope)
    {
        return new ProfileController(scope.ServiceProvider.GetRequiredService<IProfileService>())
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                    {
                        new Claim("id", "-21")
                    }))
                }
            }
        };
    }
}