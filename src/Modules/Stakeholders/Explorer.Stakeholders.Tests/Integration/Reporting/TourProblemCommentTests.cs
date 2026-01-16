using Explorer.Stakeholders.Core.UseCases.Reporting;
using Explorer.Stakeholders.Infrastructure.Database.Repositories.Users;
using Explorer.Stakeholders.Infrastructure.Database;
using Explorer.Stakeholders.Infrastructure.Database.Repositories.TourProblems;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using AutoMapper;
using Explorer.API.Controllers.Shared;
using Explorer.Stakeholders.API.Dtos.Comments;

namespace Explorer.Stakeholders.Tests.Integration.Reporting
{
    [Collection("Sequential")]
    public class CommentTests : BaseStakeholdersIntegrationTest
    {
        public CommentTests(StakeholdersTestFactory factory) : base(factory) { }

        [Fact]
        public void Successfully_creates_comment()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();

            var createCommentDto = new CreateCommentDto
            {
                Content = "Novi komentar iz testa"
            };

            // Act
            var actionResult = controller.Create(createCommentDto);
            var result = ((OkObjectResult)actionResult.Result)?.Value as CommentDto;

            // Assert - Response
            result.ShouldNotBeNull();
            result.CommentId.ShouldBeGreaterThan(0);
            result.AuthorId.ShouldBe(-21);
            result.Content.ShouldBe("Novi komentar iz testa");
            result.CreatedAt.ShouldNotBe(DateTimeOffset.MinValue);

            // Assert - Database
            dbContext.ChangeTracker.Clear();
            var storedComment = dbContext.Comments.FirstOrDefault(c => c.CommentId == result.CommentId);
            storedComment.ShouldNotBeNull();
            storedComment.AuthorId.ShouldBe(-21);
            storedComment.Content.ShouldBe("Novi komentar iz testa");
        }

        [Fact]
        public void Successfully_retrieves_existing_comment()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);

            // Act - Preuzmi postojeći komentar iz SQL skripte (ID: 11)
            var actionResult = controller.GetById(11);
            var result = ((OkObjectResult)actionResult.Result)?.Value as CommentDto;

            // Assert
            result.ShouldNotBeNull();
            result.CommentId.ShouldBe(11);
            result.Content.ShouldBe("Problem je resen");
            result.AuthorId.ShouldBe(-11);
        }

        [Fact]
        public void GetById_fails_for_nonexistent_comment()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            long nonExistentId = 9999;

            // Act & Assert
            Should.Throw<KeyNotFoundException>(() => controller.GetById(nonExistentId));
        }

        [Fact]
        public void Successfully_creates_multiple_comments()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();

            // Act - Kreiraj više komentara
            var comment1 = ((OkObjectResult)controller.Create(new CreateCommentDto { Content = "Prvi komentar" }).Result)?.Value as CommentDto;
            var comment2 = ((OkObjectResult)controller.Create(new CreateCommentDto { Content = "Drugi komentar" }).Result)?.Value as CommentDto;
            var comment3 = ((OkObjectResult)controller.Create(new CreateCommentDto { Content = "Treći komentar" }).Result)?.Value as CommentDto;

            // Assert
            comment1.ShouldNotBeNull();
            comment2.ShouldNotBeNull();
            comment3.ShouldNotBeNull();

            comment1.CommentId.ShouldNotBe(comment2.CommentId);
            comment2.CommentId.ShouldNotBe(comment3.CommentId);

            // Assert - Svi su u bazi
            dbContext.ChangeTracker.Clear();
            var allComments = dbContext.Comments.Where(c =>
                c.CommentId == comment1.CommentId ||
                c.CommentId == comment2.CommentId ||
                c.CommentId == comment3.CommentId).ToList();

            allComments.Count.ShouldBe(3);
        }

        private static CommentController CreateController(IServiceScope scope)
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();
            var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();
            var commentRepository = new CommentDbRepository(dbContext);
            var userRepository = new UserDbRepository(dbContext);
            var commentService = new CommentService(commentRepository, userRepository, mapper);

            return new CommentController(commentService)
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
}