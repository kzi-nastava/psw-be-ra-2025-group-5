using Explorer.BuildingBlocks.Core.Exceptions;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Infrastructure.Database;
using Microsoft.AspNetCore.Http;
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

            var created = service.Create(
                new ClubDto
                {
                    Name = "Planinari",
                    Description = "Opis",
                    CreatorId = 1
                },
                new List<IFormFile> { CreateTestImage() }
            );

            var updated = service.Update(
                new ClubDto
                {
                    Id = created.Id,
                    Name = "Planinari Updated",
                    Description = "Novi opis",
                    CreatorId = 1
                },
                new List<IFormFile> { CreateTestImage() }
            );

            updated.Name.ShouldBe("Planinari Updated");
            updated.Description.ShouldBe("Novi opis");
            updated.ImagePaths.Count.ShouldBe(2); 

            dbContext.ChangeTracker.Clear();
            var stored = dbContext.Clubs.Find(created.Id);
            stored.ImagePaths.Count.ShouldBe(2);
        }

        [Fact]
        public void Fails_when_club_not_found()
        {
            using var scope = Factory.Services.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IClubService>();

            var dto = new ClubDto
            {
                Id = 9999,
                Name = "Test",
                Description = "Desc",
                CreatorId = 1
            };

            Should.Throw<NotFoundException>(() =>
                service.Update(dto, null)
            );
        }

        [Fact]
        public void Fails_when_name_empty()
        {
            using var scope = Factory.Services.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IClubService>();

            var created = service.Create(
                new ClubDto
                {
                    Name = "Valid",
                    Description = "Desc",
                    CreatorId = 1
                },
                new List<IFormFile> { CreateTestImage() }
            );

            var dto = new ClubDto
            {
                Id = created.Id,
                Name = "",
                Description = "Updated",
                CreatorId = 1
            };

            Should.Throw<ArgumentException>(() =>
                service.Update(dto, null)
            );
        }

        [Fact]
        public void Fails_when_description_empty()
        {
            using var scope = Factory.Services.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IClubService>();

            var created = service.Create(
                new ClubDto
                {
                    Name = "Name",
                    Description = "Valid",
                    CreatorId = 1
                },
                new List<IFormFile> { CreateTestImage() }
            );

            var dto = new ClubDto
            {
                Id = created.Id,
                Name = "Updated",
                Description = "",
                CreatorId = 1
            };

            Should.Throw<ArgumentException>(() =>
                service.Update(dto, null)
            );
        }

        private static IFormFile CreateTestImage()
        {
            var bytes = new byte[] { 1, 2, 3 };
            var stream = new MemoryStream(bytes);

            return new FormFile(
                stream,
                0,
                bytes.Length,
                "image",
                "test.jpg")
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/jpeg"
            };
        }

    }

}
