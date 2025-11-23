using Explorer.BuildingBlocks.Core.Exceptions;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Infrastructure.Database;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Tests.Integration.TouristClub
{
    [Collection("Sequential")]
    public class DeleteClubTest : BaseStakeholdersIntegrationTest
    {
        public DeleteClubTest(StakeholdersTestFactory factory) : base(factory) { }

        [Fact]
        public void Successfully_deletes_club()
        {
            using var scope = Factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();
            var service = scope.ServiceProvider.GetRequiredService<IClubService>();

            // Arrange — create club
            var created = service.Create(new ClubDto
            {
                Id = -5151,
                Name = "Planinari",
                Description = "Opis",
                Images = new List<string> { Convert.ToBase64String(new byte[] { 1 }) },
                CreatorId = 1
            });

            // Act
            service.Delete(1, created.Id);

            // Assert
            dbContext.ChangeTracker.Clear();
            var stored = dbContext.Clubs.Find(created.Id);
            stored.ShouldBeNull();
        }

        [Fact]
        public void Fails_when_club_not_found()
        {
            using var scope = Factory.Services.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IClubService>();

            Should.Throw<NotFoundException>(() => service.Delete(1, 9999));
        }

        [Fact]
        public void Fails_when_creator_attempts_to_delete_other_club()
        {
            using var scope = Factory.Services.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IClubService>();

            var created = service.Create(new ClubDto
            {
                Id = -5151,
                Name = "Planinari",
                Description = "Opis",
                Images = new List<string> { Convert.ToBase64String(new byte[] { 1 }) },
                CreatorId = 1
            });

            Should.Throw<UnauthorizedAccessException>(() =>
            {
                service.Delete(2, created.Id);
            });
        }
    }
}
