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
                Images = new List<string> { Convert.ToBase64String(new byte[] { 1, 2, 3 }) },
                CreatorId = 1 
            };

            var created = service.Create(dto);

            created.ShouldNotBeNull();
            created.Id.ShouldNotBe(0);

            dbContext.ChangeTracker.Clear();
            var stored = dbContext.Clubs.Find(created.Id);
            stored.ShouldNotBeNull();
            stored.Name.ShouldBe(dto.Name);
            stored.Description.ShouldBe(dto.Description);
            stored.Images.Count.ShouldBe(1);
        }

        [Fact]
        public void Fails_when_description_missing()
        {
            using var scope = Factory.Services.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IClubService>();

            var dto = new ClubDto
            {
                Id = -1515,
                Name = "Planinari",
                Description = "",
                Images = new List<string> { Convert.ToBase64String(new byte[] { 1 }) },
                CreatorId = 1
            };

            Should.Throw<EntityValidationException>(() => service.Create(dto));
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
                Images = new List<string> { Convert.ToBase64String(new byte[] { 4 }) },
                CreatorId = 1
            };

            Should.Throw<EntityValidationException>(() => service.Create(dto));
        }



        [Fact]
        public void Fails_when_no_images()
        {
            using var scope = Factory.Services.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IClubService>();


            var dto = new ClubDto
            {
                Id = -1515,
                Name = "Planinari",
                Description = "Opis",
                Images = new List<string>(),
                CreatorId = 1
            };
        }
        public void Fails_when_creator_does_not_exist()
        {
            using var scope = Factory.Services.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IClubService>();

            var dto = new ClubDto
            {
                Id = -1556,
                Name = "Planinari",
                Description = "Opis",
                Images = new List<string> { Convert.ToBase64String(new byte[] { 1 }) },
                CreatorId = 9999    
            };

            Should.Throw<NotFoundException>(() => service.Create(dto));
        }

        [Fact]
        public void Fails_when_image_is_not_valid_base64()
        {
            using var scope = Factory.Services.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IClubService>();

            var dto = new ClubDto
            {
                Name = "Planinari",
                Description = "Opis",
                Images = new List<string> { "not-base64" },
                CreatorId = 1
            };

            Should.Throw<EntityValidationException>(() => service.Create(dto));
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
                Images = new List<string>
        {
            Convert.ToBase64String(new byte[] { 1 }),
            Convert.ToBase64String(new byte[] { 2 }),
            Convert.ToBase64String(new byte[] { 3 })
        },
                CreatorId = 1
            };

            var created = service.Create(dto);

            created.ShouldNotBeNull();
            created.Images.Count.ShouldBe(3);

            dbContext.ChangeTracker.Clear();
            var stored = dbContext.Clubs.Find(created.Id);
            stored.Images.Count.ShouldBe(3);
        }
    }
}
