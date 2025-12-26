using Explorer.API.Controllers.Administrator;
using Explorer.Stakeholders.API.Dtos.Users;
using Explorer.Stakeholders.Core.UseCases.Administration.Users;
using Explorer.Stakeholders.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Tests.Integration.Authentication
{
    [Collection("Sequential")]
    public class UnblockUserTests : BaseStakeholdersIntegrationTest
    {
        public UnblockUserTests(StakeholdersTestFactory factory) : base(factory) { }
        [Fact]
        public void UnblockAuthor_Succeeds()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();
            var controller = CreateController(scope);
            // Act
            var result = ((ObjectResult)controller.UnblockUser(-12).Result).Value as UserDto;
            // Assert
            result.ShouldNotBeNull();
            result.IsActive.ShouldBeTrue();
            dbContext.ChangeTracker.Clear();
            var user = dbContext.Users.First(u => u.Id == -12);
            user.IsActive.ShouldBeTrue();
        }
        [Fact]
        public void UnblockTourist_Succeeds()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();
            var controller = CreateController(scope);
            // Act
            var result = ((ObjectResult)controller.UnblockUser(-22).Result).Value as UserDto;
            // Assert
            result.ShouldNotBeNull();
            result.IsActive.ShouldBeTrue();
            dbContext.ChangeTracker.Clear();
            var user = dbContext.Users.First(u => u.Id == -22);
            user.IsActive.ShouldBeTrue();
        }

        private static AdministrationController CreateController(IServiceScope scope)
        {
            return new AdministrationController(scope.ServiceProvider.GetRequiredService<IUserService>());
        }

    }
}
