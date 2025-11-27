using Explorer.API.Controllers.Administrator.Administration;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.Core.UseCases;
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
    public class BlockUserTests : BaseStakeholdersIntegrationTest
    {
        public BlockUserTests(StakeholdersTestFactory factory) : base(factory) { }

        [Fact]
        public void BlockAuthor_Succeeds()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();

            var controller = CreateController(scope);

            // Act
            var result = ((ObjectResult)controller.BlockUser(-11).Result).Value as UserDto;

            // Assert
            result.ShouldNotBeNull();
            result.IsActive.ShouldBeFalse();

            dbContext.ChangeTracker.Clear();
            var user = dbContext.Users.First(u => u.Id == -11);
            user.IsActive.ShouldBeFalse();
        }

        [Fact]
        public void BlockTourist_Succeeds()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();

            var controller = CreateController(scope);

            // Act
            var result = ((ObjectResult)controller.BlockUser(-21).Result).Value as UserDto; 

            // Assert
            result.ShouldNotBeNull();
            result.IsActive.ShouldBeFalse();

            dbContext.ChangeTracker.Clear();
            var user = dbContext.Users.First(u => u.Id == -21);
            user.IsActive.ShouldBeFalse();
        }

        [Fact]
        public void BlockAdministrator_ThrowsException()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();

            var controller = CreateController(scope);

            // Act & Assert
            Should.Throw<InvalidOperationException>(() => controller.BlockUser(-1));
        }
        private static AdministrationController CreateController(IServiceScope scope)
        {
            return new AdministrationController(scope.ServiceProvider.GetRequiredService<IUserService>());
        }
    }
}

    
