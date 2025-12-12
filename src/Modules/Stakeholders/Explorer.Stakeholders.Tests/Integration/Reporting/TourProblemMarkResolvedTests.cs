using Explorer.API.Controllers.Tourist.ProblemReporting;
using Explorer.BuildingBlocks.Core.Exceptions;
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
    public class TourProblemMarkResolvedTests : BaseStakeholdersIntegrationTest
    {
        public TourProblemMarkResolvedTests(StakeholdersTestFactory factory) : base(factory) { }

        [Fact]
        public void Reporter_tourist_can_mark_problem_as_resolved_and_unresolved()
        {
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope, "-22", "tourist");
            var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();

            long problemId = -22;
            var problemBefore = dbContext.TourProblems.Find(problemId);
            problemBefore.IsResolved.ShouldBeTrue();

            // Mark as resolved
            controller.MarkResolved(problemId, false);
            dbContext.ChangeTracker.Clear();
            var problemAfter = dbContext.TourProblems.Find(problemId);
            problemAfter.IsResolved.ShouldBeFalse();

            // Mark back as unresolved
            controller.MarkResolved(problemId, true);
            dbContext.ChangeTracker.Clear();
            var problemFinal = dbContext.TourProblems.Find(problemId);
            problemFinal.IsResolved.ShouldBeTrue();
        }

        [Fact]
        public void Non_reporter_tourist_can_mark_problem_without_exception()
        {
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope, "-21", "tourist");
            long problemId = -22;

            controller.MarkResolved(problemId, true);
            var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();
            dbContext.ChangeTracker.Clear();
            var problemAfter = dbContext.TourProblems.Find(problemId);
            problemAfter.IsResolved.ShouldBeTrue();
        }


        [Fact]
        public void Author_or_admin_can_mark_problem_without_exception()
        {
            using var scope = Factory.Services.CreateScope();

            // Author of the tour
            var authorController = CreateController(scope, "-2", "author");
            authorController.MarkResolved(-22, true);

            // Admin
            var adminController = CreateController(scope, "-1", "administrator");
            adminController.MarkResolved(-22, true);

            var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();
            dbContext.ChangeTracker.Clear();
            var problemAfter = dbContext.TourProblems.Find(-22L);
            problemAfter.IsResolved.ShouldBeTrue();
        }


        [Fact]
        public void MarkResolved_fails_for_nonexistent_problem()
        {
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope, "-22", "tourist");
            long invalidId = 9999;

            Should.Throw<NotFoundException>(() => controller.MarkResolved(invalidId, true));
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
