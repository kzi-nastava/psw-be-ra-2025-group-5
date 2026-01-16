using Explorer.BuildingBlocks.Core.Exceptions;
using Explorer.Stakeholders.API.Dtos.Clubs;
using Explorer.Stakeholders.API.Public.Clubs;
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

    public class CreateClubTest : BaseStakeholdersIntegrationTest
    {
        public CreateClubTest(StakeholdersTestFactory factory) : base(factory) { }

        [Fact]
        public void Successfully_creates_club()
        {
            using var scope = Factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();
            var service = scope.ServiceProvider.GetRequiredService<IClubService>();

            var dto = new ClubDto
            {
                Name = "Planinari Balkan",
                Description = "Klub za sve ljubitelje planinarenja",
                CreatorId = 1
            };

            var created = service.Create(dto, new List<IFormFile> { CreateTestImage() });

            created.ShouldNotBeNull();
            created.Id.ShouldNotBe(0);

            dbContext.ChangeTracker.Clear();
            var stored = dbContext.Clubs.Find(created.Id);
            stored.ShouldNotBeNull();
            stored.Name.ShouldBe(dto.Name);
            stored.Description.ShouldBe(dto.Description);
            stored.ImagePaths.Count.ShouldBe(1);
        }


        [Fact]
        public void Fails_when_description_missing()
        {
            using var scope = Factory.Services.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IClubService>();

            var dto = new ClubDto
            {
                Name = "Planinari",
                Description = "",
                CreatorId = 1
            };

            Should.Throw<ArgumentException>(() =>
                service.Create(dto, new List<IFormFile> { CreateTestImage() })
            );
        }


        [Fact]
        public void Fails_when_missing_name()
        {
            using var scope = Factory.Services.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IClubService>();

            var dto = new ClubDto
            {
                Name = "",
                Description = "Opis",
                CreatorId = 1
            };

            Should.Throw<ArgumentException>(() =>
                service.Create(dto, new List<IFormFile> { CreateTestImage() })
            );
        }


        [Fact]
        public void Fails_when_no_images()
        {
            using var scope = Factory.Services.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IClubService>();

            var dto = new ClubDto
            {
                Name = "Planinari",
                Description = "Opis",
                CreatorId = 1
            };

            Should.Throw<ArgumentException>(() =>
                service.Create(dto, new List<IFormFile>())
            );
        }

        [Fact]
        public void Successfully_creates_club_with_multiple_images()
        {
            using var scope = Factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();
            var service = scope.ServiceProvider.GetRequiredService<IClubService>();

            var dto = new ClubDto
            {
                Name = "Planinari Multi",
                Description = "Opis",
                CreatorId = 1
            };

            var created = service.Create(
                dto,
                new List<IFormFile>
                {
            CreateTestImage(),
            CreateTestImage(),
            CreateTestImage()
                });

            created.ShouldNotBeNull();
            created.ImagePaths.Count.ShouldBe(3);

            dbContext.ChangeTracker.Clear();
            var stored = dbContext.Clubs.Find(created.Id);
            stored.ImagePaths.Count.ShouldBe(3);
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
