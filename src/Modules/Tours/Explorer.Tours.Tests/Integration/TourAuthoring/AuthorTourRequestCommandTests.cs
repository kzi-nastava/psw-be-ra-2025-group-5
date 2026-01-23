using System;
using System.Collections.Generic;
using System.Linq;
using Explorer.BuildingBlocks.Core.Exceptions;
using Explorer.Tours.API.Dtos.Tours;
using Explorer.Tours.Core.Domain.Tours.Entities;
using Explorer.Tours.Core.Domain.RepositoryInterfaces.Tours;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;
using Explorer.API.Controllers.Tours.Author;
using Explorer.Tours.Core.Domain.Tours;
using Explorer.Tours.Core.Domain.Tours.ValueObjects;
using Explorer.Tours.API.Public.Tour;

namespace Explorer.Tours.Tests.Integration.Tours
{
    [Collection("Sequential")]
    public class TourRequestCommandTests : BaseToursIntegrationTest
    {
        public TourRequestCommandTests(ToursTestFactory factory) : base(factory) { }

        [Fact]
        public void Accept_request_succeeds()
        {
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var repo = scope.ServiceProvider.GetRequiredService<ITourRequestRepository>();

            var request = repo.Create(new TourRequest(
                touristId: 100,
                authorId: -1,
                difficulty: TourDifficulty.Medium,
                location: new Location(45.0, 20.0),
                description: "Test Accept",
                maxPrice: 200,
                tags: new List<string> { "test" }
            ));

            var result = ((ObjectResult)controller.Accept(request.Id).Result)?.Value as TourRequestDto;

            result.ShouldNotBeNull();
            result.Status.ShouldBe("Accepted");

            var updated = repo.Get(request.Id);
            updated.Status.ShouldBe(TourRequestStatus.Accepted);
        }

        [Fact]
        public void Decline_request_succeeds()
        {
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var repo = scope.ServiceProvider.GetRequiredService<ITourRequestRepository>();

            var request = repo.Create(new TourRequest(
                touristId: 101,
                authorId: -1,
                difficulty: TourDifficulty.Medium,
                location: new Location(46.0, 21.0),
                description: "Test Decline",
                maxPrice: 150,
                tags: new List<string> { "test" }
            ));

            var result = ((ObjectResult)controller.Decline(request.Id).Result)?.Value as TourRequestDto;

            result.ShouldNotBeNull();
            result.Status.ShouldBe("Rejected");

            var updated = repo.Get(request.Id);
            updated.Status.ShouldBe(TourRequestStatus.Rejected);
        }

        [Fact]
        public void Accept_request_fails_for_wrong_author()
        {
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var repo = scope.ServiceProvider.GetRequiredService<ITourRequestRepository>();

            var request = repo.Create(new TourRequest(
                touristId: 102,
                authorId: 999, // nije trenutni autor
                difficulty: TourDifficulty.Medium,
                location: new Location(47.0, 22.0),
                description: "Unauthorized request",
                maxPrice: 300,
                tags: new List<string> { "test" }
            ));

            Should.Throw<UnauthorizedAccessException>(() => controller.Accept(request.Id));
        }

        [Fact]
        public void Accept_request_fails_if_already_processed()
        {
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var repo = scope.ServiceProvider.GetRequiredService<ITourRequestRepository>();

            var request = repo.Create(new TourRequest(
                touristId: 103,
                authorId: -1,
                difficulty: TourDifficulty.Medium,
                location: new Location(48.0, 23.0),
                description: "Already processed",
                maxPrice: 100,
                tags: new List<string> { "test" }
            ));

            request.Accept();
            repo.Update(request);

            Should.Throw<InvalidOperationException>(() => controller.Accept(request.Id));
        }

        private static AuthorRequestController CreateController(IServiceScope scope)
        {
            return new AuthorRequestController(
                scope.ServiceProvider.GetRequiredService<ITourRequestService>())
            {
                ControllerContext = BuildContext("-1") // author id = -1
            };
        }
    }
}
