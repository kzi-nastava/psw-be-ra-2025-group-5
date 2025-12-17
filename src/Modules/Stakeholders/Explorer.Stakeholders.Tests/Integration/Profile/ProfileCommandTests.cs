using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Infrastructure.Database;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Explorer.API.Controllers.Profile;

namespace Explorer.Stakeholders.Tests.Integration.Profile;

[Collection("Sequential")]
public class ProfileCommandTests : BaseStakeholdersIntegrationTest
{
    public ProfileCommandTests(StakeholdersTestFactory factory) : base(factory) { }

    [Fact]
    public void Successfully_updates_profile()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();
        var controller = CreateController(scope);

        var updatedProfile = new ProfileDto
        {
            Id = -21,
            Name = "UpdatedPera",
            Surname = "UpdatedPerić",
            Email = "turista1@gmail.com",
            Biography = "Nova biografija",
            Motto = "Novi moto"
        };

        // Act
        var result = ((ObjectResult)controller.UpdateProfile(-21, updatedProfile).Result)?.Value as ProfileDto;

        // Assert - Response
        result.ShouldNotBeNull();
        result.Name.ShouldBe("UpdatedPera");
        result.Surname.ShouldBe("UpdatedPerić");
        result.Biography.ShouldBe("Nova biografija");
        result.Motto.ShouldBe("Novi moto");

        // Assert - Database
        dbContext.ChangeTracker.Clear();
        var storedPerson = dbContext.People.FirstOrDefault(i => i.Id == -21);
        storedPerson.ShouldNotBeNull();
        storedPerson.Name.ShouldBe("UpdatedPera");
        storedPerson.Surname.ShouldBe("UpdatedPerić");
        storedPerson.Biography.ShouldBe("Nova biografija");
        storedPerson.Motto.ShouldBe("Novi moto");
    }

    [Fact]
    public void Successfully_updates_profile_with_image()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();
        var controller = CreateController(scope);

        var updatedProfile = new ProfileDto
        {
            Id = -21,
            Name = "Pera",
            Surname = "Perić",
            Email = "turista1@gmail.com",
            ProfileImageBase64 = "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mNkYPhfDwAChwGA60e6kgAAAABJRU5ErkJggg=="
        };

        // Act
        var result = ((ObjectResult)controller.UpdateProfile(-21, updatedProfile).Result)?.Value as ProfileDto;

        // Assert
        result.ShouldNotBeNull();

        // Assert - Database
        dbContext.ChangeTracker.Clear();
        var storedPerson = dbContext.People.FirstOrDefault(i => i.Id == -21);
        storedPerson.ShouldNotBeNull();
        storedPerson.ProfileImage.ShouldNotBeNull();
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