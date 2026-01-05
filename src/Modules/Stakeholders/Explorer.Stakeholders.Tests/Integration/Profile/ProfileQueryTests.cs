using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Explorer.Stakeholders.Infrastructure.Database;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Explorer.API.Controllers.Profile;
using Explorer.Stakeholders.API.Dtos.Users;
using Explorer.Stakeholders.API.Public.Users;

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