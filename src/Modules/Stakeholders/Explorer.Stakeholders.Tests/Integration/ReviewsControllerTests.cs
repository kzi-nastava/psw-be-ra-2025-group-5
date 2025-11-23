using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Explorer.API.Controllers;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Infrastructure.Database;
using Explorer.Stakeholders.Core.Domain;
using Explorer.API.Controllers.Administration;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace Explorer.Stakeholders.Tests.Integration.AppRatings;

[Collection("Sequential")]
public class AppRatingsTests : BaseStakeholdersIntegrationTest
{
    public AppRatingsTests(StakeholdersTestFactory factory) : base(factory) { }


    [Fact]
    public void Tourist_can_create_rating()
    {
        using var scope = Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();
       // ResetDatabase(db);

        var controller = CreateController(scope, "-23", "tourist");

        var dto = new CreateAppRatingDto
        {
            Rating = 5,
            Comment = "Sjajna aplikacija!"
        };

        var result = ((ObjectResult)controller.Create(dto).Result)?.Value as AppRatingDto;

        result.ShouldNotBeNull();
        result.Id.ShouldNotBe(0);
        result.Rating.ShouldBe(5);
        result.Comment.ShouldBe("Sjajna aplikacija!");
        result.UserId.ShouldBe(-23);

        var stored = db.AppRatings.FirstOrDefault(r => r.Id == result.Id);
        stored.ShouldNotBeNull();
        stored.UserId.ShouldBe(-23);
        stored.Comment.ShouldBe("Sjajna aplikacija!");
    }

    [Fact]
    public void Author_can_update_own_rating()
    {
        using var scope = Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();
       // ResetDatabase(db);

        var controller = CreateController(scope, "-21", "author");

        var updateDto = new UpdateAppRatingDto
        {
            Rating = 3,
            Comment = "Izmenjena ocena"
        };

        var result = ((ObjectResult)controller.Update(-1, updateDto).Result)?.Value as AppRatingDto;

        result.ShouldNotBeNull();
        result.Id.ShouldBe(-1);
        result.Rating.ShouldBe(3);
        result.Comment.ShouldBe("Izmenjena ocena");

        var stored = db.AppRatings.First(r => r.Id == -1);
        stored.Rating.ShouldBe(3);
        stored.Comment.ShouldBe("Izmenjena ocena");
    }

    [Fact]
    public void User_can_delete_own_rating()
    {
        using var scope = Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();
       // ResetDatabase(db);

        var controller = CreateController(scope, "-22", "tourist");

        var result = controller.Delete(-2);

        result.ShouldBeOfType<NoContentResult>();

        var stored = db.AppRatings.FirstOrDefault(r => r.Id == -2);
        stored.ShouldBeNull();
    }

    [Fact]
    public void Admin_can_get_all_ratings()
    {
        using var scope = Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();
        //ResetDatabase(db);

        var controller = CreateAdminController(scope);

        var actionResult = controller.GetAll();
        var result = actionResult.Result as OkObjectResult;
        result.ShouldNotBeNull();

        var ratings = result.Value as List<AppRatingDto>;
        ratings.ShouldNotBeNull();
        ratings.Count.ShouldBeGreaterThan(0);

        var first = ratings.First();
        first.UserId.ShouldBe(-21);
        first.Rating.ShouldBe(5); 

        var second = ratings[1];
        second.UserId.ShouldBe(-22);
        second.Rating.ShouldBe(4);
    }


    private static AdminAppRatingsController CreateAdminController(IServiceScope scope)
    {
        return new AdminAppRatingsController(
            scope.ServiceProvider.GetRequiredService<IAppRatingService>());
    }

    private static AppRatingsController CreateController(IServiceScope scope, string userId, string role)
    {
        var controller = new AppRatingsController(
            scope.ServiceProvider.GetRequiredService<IAppRatingService>());

        controller.ControllerContext = BuildContext(userId, role);
        return controller;
    }

    private static ControllerContext BuildContext(string userId, string role)
    {
        var claims = new[]
        {
            new Claim("id", userId),
            new Claim("role", role)
        };

        var identity = new ClaimsIdentity(claims, "TestAuth");
        var user = new ClaimsPrincipal(identity);

        return new ControllerContext
        {
            HttpContext = new Microsoft.AspNetCore.Http.DefaultHttpContext
            {
                User = user
            }
        };
    }

    private void ResetDatabase(StakeholdersContext db)
    {
       
        db.Database.ExecuteSqlRaw(@"DELETE FROM stakeholders.""AppRatings"";");
        db.Database.ExecuteSqlRaw(@"DELETE FROM stakeholders.""People"";");
        db.Database.ExecuteSqlRaw(@"DELETE FROM stakeholders.""Users"";");

      
        db.Database.ExecuteSqlRaw(@"
            INSERT INTO stakeholders.""Users"" (""Id"", ""Username"", ""Password"", ""Email"", ""Role"", ""IsActive"")
            VALUES
            (-1, 'admin@gmail.com', 'admin', 'admin@gmail.com', 0, true),
            (-11, 'autor1@gmail.com', 'autor1', 'autor1@gmail.com', 1, true),
            (-12, 'autor2@gmail.com', 'autor2', 'autor2@gmail.com',1, true),
            (-13, 'autor3@gmail.com', 'autor3', 'autor3@gmail.com', 1, true),
            (-21, 'turista1@gmail.com','turista1','turista1@gmail.com', 2, true),
            (-22, 'turista2@gmail.com','turista2','turista2@gmail.com', 2, true),
            (-23, 'turista3@gmail.com','turista3','turista3@gmail.com', 2, true);
        ");

      
        db.Database.ExecuteSqlRaw(@"
            INSERT INTO stakeholders.""People"" (""Id"", ""UserId"", ""Name"", ""Surname"", ""Email"")
            VALUES
            (-11, -11, 'Ana',  'Anić',   'autor1@gmail.com'),
            (-12, -12, 'Lena', 'Lenić',  'autor2@gmail.com'),
            (-13, -13, 'Sara', 'Sarić',  'autor3@gmail.com'),
            (-21, -21, 'Pera', 'Perić',  'turista1@gmail.com'),
            (-22, -22, 'Mika', 'Mikić',  'turista2@gmail.com'),
            (-23, -23, 'Steva','Stević', 'turista3@gmail.com');
        ");

       
        db.Database.ExecuteSqlRaw(@"
            INSERT INTO stakeholders.""AppRatings"" (""Id"", ""UserId"", ""Rating"", ""Comment"", ""CreatedAt"", ""UpdatedAt"")
            VALUES 
            (-1, -21, 5, 'Odlicna aplikacija', '2025-10-25T10:00:00Z', '2025-10-25T10:00:00Z'),
            (-2, -22, 4, 'Solidno', '2025-10-25T10:00:00Z', '2025-10-25T10:00:00Z');
        ");

    }
}
