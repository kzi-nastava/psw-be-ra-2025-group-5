using Explorer.API.Controllers;
using Explorer.API.Controllers.Administrator;
using Explorer.Stakeholders.API.Dtos.Users;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Core.Domain.Users;
using Explorer.Stakeholders.Core.UseCases.Administration.Users;
using Explorer.Stakeholders.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Tests.Integration.Authentication
{
    [Collection("Sequential")]
    public class CreateNewUsersByAdminTests : BaseStakeholdersIntegrationTest
    {
        public CreateNewUsersByAdminTests(StakeholdersTestFactory factory) : base(factory) { }

        [Fact]
        public void Successfully_creates_admin()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();
            var controller = CreateController(scope);
            controller.ControllerContext = BuildContext("-1");

            var newAdmin = new CreateUserDto
            {
                Username = "admin542",
                Password = "admin555",
                Email = "admin5555@gmail.com",
                Role = "Administrator"
            };

            // Act
            var result = ((ObjectResult)controller.Create(newAdmin).Result).Value as UserDto;

            // Assert - Response
            result.ShouldNotBeNull();
            result.Id.ShouldNotBe(0);
            result.Role.ShouldBe("Administrator");

            // Assert - Database
            var storedUser = dbContext.Users.FirstOrDefault(u => u.Username == newAdmin.Username);
            storedUser.ShouldNotBeNull();
            storedUser.Role.ShouldBe(UserRole.Administrator);

        }

        private static AdministrationController CreateController(IServiceScope scope)
        {
            return new AdministrationController(scope.ServiceProvider.GetRequiredService<IUserService>());
        }
    }
}
