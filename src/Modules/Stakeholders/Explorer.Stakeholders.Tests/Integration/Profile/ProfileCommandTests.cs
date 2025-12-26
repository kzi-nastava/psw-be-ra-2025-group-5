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
public class ProfileCommandTests : BaseStakeholdersIntegrationTest
{
    public ProfileCommandTests(StakeholdersTestFactory factory) : base(factory) { }

    [Fact]
    public void Successfully_updates_profile()
    {
        using var scope = Factory.Services.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IProfileService>();
        var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();

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
        var result = service.Update(updatedProfile, profileImage: null);

        // Assert - Response
        result.ShouldNotBeNull();
        result.Name.ShouldBe("UpdatedPera");
        result.Surname.ShouldBe("UpdatedPerić");
        result.Biography.ShouldBe("Nova biografija");
        result.Motto.ShouldBe("Novi moto");

        // Assert - Database
        dbContext.ChangeTracker.Clear();
        var storedPerson = dbContext.People.Find((long)-21);
        storedPerson.ShouldNotBeNull();
        storedPerson.Name.ShouldBe("UpdatedPera");
        storedPerson.Surname.ShouldBe("UpdatedPerić");
        storedPerson.Biography.ShouldBe("Nova biografija");
        storedPerson.Motto.ShouldBe("Novi moto");
    }

    [Fact]
    public void Successfully_updates_profile_with_image()
    {
        using var scope = Factory.Services.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IProfileService>();
        var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();

        var imageBytes = Convert.FromBase64String("iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mNkYPhfDwAChwGA60e6kgAAAABJRU5ErkJggg==");

        var stream = new MemoryStream(imageBytes);
        var formFile = new Microsoft.AspNetCore.Http.FormFile(stream, 0, imageBytes.Length, "ProfileImage", "profile.png")
        {
            Headers = new Microsoft.AspNetCore.Http.HeaderDictionary(),
            ContentType = "image/png"
        };

        var updatedProfile = new ProfileDto
        {
            Id = -21,
            Name = "Pera",
            Surname = "Perić",
            Email = "turista1@gmail.com"
        };

        // Act
        var result = service.Update(updatedProfile, profileImage: formFile);

        // Assert
        result.ShouldNotBeNull();

        // Assert - Database
        dbContext.ChangeTracker.Clear();
        var storedPerson = dbContext.People.Find((long)-21);
        storedPerson.ShouldNotBeNull();
        storedPerson.ProfileImagePath.ShouldNotBeNull(); 
    }
}