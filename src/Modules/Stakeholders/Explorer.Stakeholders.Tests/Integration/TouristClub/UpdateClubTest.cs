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
    public class UpdateClubTest : BaseStakeholdersIntegrationTest
    {
        public UpdateClubTest(StakeholdersTestFactory factory) : base(factory) { }

        [Fact]
        public void Successfully_updates_club()
        {
            using var scope = Factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();
            var service = scope.ServiceProvider.GetRequiredService<IClubService>();

            // Arrange – create a club
            var created = service.Create(new ClubDto
            {
                Name = "Planinari",
                Description = "Opis",
                Images = new List<string> { Convert.ToBase64String(new byte[] { 1 }) },
                CreatorId = 1
            });

            var dto = new ClubDto
            {
                Id = created.Id,
                Name = "Planinari Updated",
                Description = "Novi opis",
                Images = new List<string> { Convert.ToBase64String(new byte[] { 2 }) },
                CreatorId = 1
            };

            // Act
            var updated = service.Update(dto);

            // Assert
            updated.ShouldNotBeNull();
            updated.Name.ShouldBe("Planinari Updated");
            updated.Description.ShouldBe("Novi opis");

            dbContext.ChangeTracker.Clear();
            var stored = dbContext.Clubs.Find(created.Id);
            stored.ShouldNotBeNull();
            stored.Name.ShouldBe("Planinari Updated");
            stored.Description.ShouldBe("Novi opis");
            stored.Images.Count.ShouldBe(1);
        }

        [Fact]
        public void Fails_when_club_not_found()
        {
            using var scope = Factory.Services.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IClubService>();

            var dto = new ClubDto
            {
                Name = "Test",
                Description = "Desc",
                Images = new List<string> { Convert.ToBase64String(new byte[] { 1 }) },
                CreatorId = 1
            };

            Should.Throw<NotFoundException>(() => service.Update(dto));
        }

        [Fact]
        public void Fails_when_name_empty()
        {
            using var scope = Factory.Services.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IClubService>();

            var created = service.Create(new ClubDto
            {
                Name = "Valid",
                Description = "Desc",
                Images = new List<string> { Convert.ToBase64String(new byte[] { 1 }) },
                CreatorId = 1
            });

            var dto = new ClubDto
            {
                Id = created.Id,
                Name = "",
                Description = "Updated Desc",
                Images = new List<string> { Convert.ToBase64String(new byte[] { 2 }) },
                CreatorId = 1
            };

            Should.Throw<EntityValidationException>(() => service.Update(dto));
        }
        [Fact]
        public void Fails_when_description_empty()
        {
            using var scope = Factory.Services.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IClubService>();

            var created = service.Create(new ClubDto
            {
                Name = "Name",
                Description = "Valid",
                Images = new List<string> { Convert.ToBase64String(new byte[] { 1 }) },
                CreatorId = 1
            });

            var dto = new ClubDto
            {
                Id = created.Id,
                Name = "NameNew",
                Description = "",
                Images = new List<string> { Convert.ToBase64String(new byte[] { 2 }) },
                CreatorId = 1
            };

            Should.Throw<EntityValidationException>(() => service.Update(dto));
        }
        [Fact]
        public void Fails_when_no_images()
        {
            using var scope = Factory.Services.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IClubService>();

            var created = service.Create(new ClubDto
            {
                Name = "Test",
                Description = "Desc",
                Images = new List<string> { Convert.ToBase64String(new byte[] { 1 }) },
                CreatorId = 1
            });

            var dto = new ClubDto
            {
                Id = created.Id,
                Name = "Updated",
                Description = "Updated Desc",
                Images = new List<string>(), 
                CreatorId = 1
            };

            Should.Throw<EntityValidationException>(() => service.Update(dto));
        }

        [Fact]
        public void Fails_when_image_invalid_base64()
        {
            using var scope = Factory.Services.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IClubService>();

            var created = service.Create(new ClubDto
            {
                Name = "Test",
                Description = "Opis",
                Images = new List<string> { Convert.ToBase64String(new byte[] { 1 }) },
                CreatorId = 1
            });

            var dto = new ClubDto
            {
                Id = created.Id,
                Name = "Test",
                Description = "Opis",
                Images = new List<string> { "invalid-base64" },
                CreatorId = 1
            };

            Should.Throw<EntityValidationException>(() => service.Update(dto));
        }
    }
}
