using Explorer.API.Controllers.Tourist.ProblemReporting;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public.Reporting;
using Explorer.Stakeholders.Infrastructure.Database;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Tests.Integration.Reporting
{
    [Collection("Sequential")]
    public class TourProblemCommentTests : BaseStakeholdersIntegrationTest
    {
        public TourProblemCommentTests(StakeholdersTestFactory factory) : base(factory) { }

        [Fact]
        public void Successfully_adds_comment()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();
            var createCommentDto = new CreateCommentDto
            {
                Content = "Novi komentar na problem"
            };

            // Act
            var result = ((ObjectResult)controller.AddComment(-11, createCommentDto).Result)?.Value as CommentDto;

            // Assert - Response
            result.ShouldNotBeNull();
            result.CommentId.ShouldNotBe(0);
            result.AuthorId.ShouldBe(-21);
            result.Content.ShouldBe(createCommentDto.Content);
            result.CreatedAt.ShouldNotBe(DateTimeOffset.MinValue);

            // Assert - Database
            dbContext.ChangeTracker.Clear();
            var storedComment = dbContext.Comments.FirstOrDefault(c => c.CommentId == result.CommentId);
            storedComment.ShouldNotBeNull();
            storedComment.AuthorId.ShouldBe(-21);
            storedComment.Content.ShouldBe(createCommentDto.Content);
        }

        [Fact]
        public void Add_comment_fails_empty_content()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var createCommentDto = new CreateCommentDto
            {
                Content = ""
            };

            // Act & Assert
            Should.Throw<ArgumentException>(() => controller.AddComment(-11, createCommentDto));
        }

        [Fact]
        public void Add_comment_fails_null_content()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var createCommentDto = new CreateCommentDto
            {
                Content = null
            };

            // Act & Assert
            Should.Throw<ArgumentException>(() => controller.AddComment(-11, createCommentDto));
        }

        [Fact]
        public void Add_comment_fails_invalid_problem_id()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var createCommentDto = new CreateCommentDto
            {
                Content = "Komentar"
            };

            // Act & Assert
            Should.Throw<Exception>(() => controller.AddComment(-9999, createCommentDto));
        }

        [Fact]
        public void Successfully_retrieves_comments()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);

            // Act
            var result = ((ObjectResult)controller.GetComments(-11).Result)?.Value as List<CommentDto>;

            // Assert
            result.ShouldNotBeNull();
            result.Count.ShouldBeGreaterThanOrEqualTo(1);
            result.First().CommentId.ShouldBe(11);
            result.First().Content.ShouldBe("Problem je rešen");
        }

        [Fact]
        public void Get_comments_returns_empty_list_when_no_comments()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);

            // Act
            var result = ((ObjectResult)controller.GetComments(-22).Result)?.Value as List<CommentDto>;

            // Assert
            result.ShouldNotBeNull();
            result.Count.ShouldBe(0);
        }

        [Fact]
        public void Get_comments_fails_invalid_problem_id()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);

            // Act & Assert
            Should.Throw<Exception>(() => controller.GetComments(-9999));
        }

        private static TourProblemController CreateController(IServiceScope scope)
        {
            return new TourProblemController(scope.ServiceProvider.GetRequiredService<ITourProblemService>(), scope.ServiceProvider.GetRequiredService<ITourRepository>())
            {
                ControllerContext = BuildContext("-21")
            };
        }
    }
}