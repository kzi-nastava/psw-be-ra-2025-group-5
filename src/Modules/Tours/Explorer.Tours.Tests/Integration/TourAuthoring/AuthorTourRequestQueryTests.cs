using System;
using System.Collections.Generic;
using System.Linq;
using Explorer.Tours.API.Dtos.Tours;
using Explorer.Tours.Core.Domain.Tours.Entities;
using Explorer.Tours.Core.Domain.RepositoryInterfaces.Tours;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;
using Explorer.API.Controllers.Tours.Tourist;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Public.Tour;
using Explorer.Tours.Core.Domain.Tours.ValueObjects;
using Explorer.Tours.Core.Domain.Tours;
using Explorer.API.Controllers.Tours.Author;

namespace Explorer.Tours.Tests.Integration.Tours
{
    [Collection("Sequential")]
    public class TourRequestQueryTests : BaseToursIntegrationTest
    {
        public TourRequestQueryTests(ToursTestFactory factory) : base(factory) { }

        [Fact]
        public void GetForAuthor_returns_only_author_requests()
        {
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);
            var repo = scope.ServiceProvider.GetRequiredService<ITourRequestRepository>();

            // Arrange: kreiramo nekoliko requestova
            var request1 = repo.Create(new TourRequest(
                touristId: 200,
                authorId: -1,
                difficulty: TourDifficulty.Easy,
                location: new Location(45.0, 20.0),
                description: "Author's request 1",
                maxPrice: 50,
                tags: new List<string> { "test" }
            ));
            var request2 = repo.Create(new TourRequest(
                touristId: 201,
                authorId: -1,
                difficulty: TourDifficulty.Hard,
                location: new Location(46.0, 21.0),
                description: "Author's request 2",
                maxPrice: 150,
                tags: new List<string> { "test" }
            ));
            var request3 = repo.Create(new TourRequest(
                touristId: 202,
                authorId: 999, // drugi autor
                difficulty: TourDifficulty.Medium,
                location: new Location(47.0, 22.0),
                description: "Other author",
                maxPrice: 100,
                tags: new List<string> { "test" }
            ));

            // Act
            var result = ((ObjectResult)controller.GetForAuthor(0, 10).Result)?.Value as PagedResult<TourRequestDto>;

            // Assert
            result.ShouldNotBeNull();
            result.Results.Count.ShouldBe(2); // samo request1 i request2
            result.Results.All(r => r.AuthorId == -1).ShouldBeTrue();
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
