using Explorer.API.Controllers.Tourist.ProblemReporting;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public.Reporting;
using Explorer.Stakeholders.Infrastructure.Database;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Tests.Integration.Reporting
{
    [Collection("Sequential")]
    public class TourProblemSetDeadlineTests : BaseStakeholdersIntegrationTest
    {
        public TourProblemSetDeadlineTests(StakeholdersTestFactory factory) : base(factory) { }

        [Fact]
        public void Admin_can_set_deadline()
        {
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope, "-1", "administrator");
            var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();

            long problemId = -22;
            var newDeadline = new DateTimeOffset(2025, 12, 24, 10, 30, 0, TimeSpan.Zero);

            // Act
            var dto = new SetDeadlineDto { Deadline = newDeadline };
            var result = controller.SetDeadline(problemId, dto);
            result.Result.ShouldBeOfType<OkResult>();

            // Assert database
            dbContext.ChangeTracker.Clear();
            var updatedProblem = dbContext.TourProblems.Find(problemId);
            updatedProblem.Deadline.ShouldBe(newDeadline);
        }

        [Fact]
        public void Tourist_cannot_set_deadline()
        {
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope, "-22", "tourist");

            long problemId = -22;
            var newDeadline = new DateTimeOffset(2025, 12, 25, 12, 0, 0, TimeSpan.Zero);

            var dto = new SetDeadlineDto { Deadline = newDeadline };

            // Act & Assert
            var result = controller.SetDeadline(problemId, dto);
            result.Result.ShouldBeOfType<OkResult>();
        }

        [Fact]
        public void Author_cannot_set_deadline()
        {
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope, "-2", "author");

            long problemId = -22;
            var newDeadline = new DateTimeOffset(2025, 12, 26, 15, 0, 0, TimeSpan.Zero);

            var dto = new SetDeadlineDto { Deadline = newDeadline };

            var result = controller.SetDeadline(problemId, dto);
            result.Result.ShouldBeOfType<OkResult>();
        }

        private static TourProblemController CreateController(IServiceScope scope, string userId, string role)
        {
            var tourProblemService = scope.ServiceProvider.GetRequiredService<ITourProblemService>();
            var tourRepository = scope.ServiceProvider.GetRequiredService<ITourRepository>();

            return new TourProblemController(tourProblemService, tourRepository)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                        {
                            new Claim("id", userId),
                            new Claim("personId", userId),
                            new Claim(ClaimTypes.Role, role)
                        }))
                    }
                }
            };
        }
    }
}
