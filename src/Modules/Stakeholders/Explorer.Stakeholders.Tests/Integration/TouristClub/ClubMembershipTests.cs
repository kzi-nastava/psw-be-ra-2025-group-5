using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Dtos.Clubs;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.API.Public.Clubs;
using Explorer.Stakeholders.Infrastructure.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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
    public class ClubMembershipTests : BaseStakeholdersIntegrationTest
    {
        public ClubMembershipTests(StakeholdersTestFactory factory) : base(factory) { }

        [Fact]
        public void Owner_can_close_club()
        {
            using var scope = Factory.Services.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IClubService>();
            var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();

            var club = service.Create(new ClubDto
            {
                Name = "Zatvoreni Klub",
                Description = "Test zatvaranja",
                CreatorId = -21
            }, new List<IFormFile> { CreateTestImage() });

            service.CloseClub(club.Id, -21);

            dbContext.ChangeTracker.Clear();
            var stored = dbContext.Clubs.Find(club.Id);
            stored.IsActive().ShouldBeFalse();
        }

        [Fact]
        public void Owner_can_remove_member()
        {
            using var scope = Factory.Services.CreateScope();

            var clubService = scope.ServiceProvider.GetRequiredService<IClubService>();
            var inviteService = scope.ServiceProvider.GetRequiredService<IClubInviteService>();
            var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();

            var club = clubService.Create(new ClubDto
            {
                Name = "Klub Test",
                Description = "Opis",
                CreatorId = -21
            }, new List<IFormFile> { CreateTestImage() });

            inviteService.InviteTourist(club.Id, -22, -21);

            var invite = dbContext.ClubInvites.First(i => i.ClubId == club.Id && i.TouristId == -22);

            inviteService.AcceptInvite(invite.Id, -22);

            clubService.RemoveMember(club.Id, -21, -22);

            dbContext.ChangeTracker.Clear();

            var storedClub = dbContext.Clubs
                .Include(c => c.Members)
                .First(c => c.Id == club.Id);

            storedClub.Members.Any(m => m.TouristId == -22).ShouldBeFalse();
        }

        [Fact]
        public void Owner_can_send_invite_and_tourist_can_accept()
        {
            using var scope = Factory.Services.CreateScope();
            var inviteService = scope.ServiceProvider.GetRequiredService<IClubInviteService>();
            var clubService = scope.ServiceProvider.GetRequiredService<IClubService>();
            var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();

            var club = clubService.Create(new ClubDto
            {
                Name = "Pozivni Klub",
                Description = "Test invite",
                CreatorId = -21
            }, new List<IFormFile> { CreateTestImage() });

            inviteService.InviteTourist(club.Id, -22, -21);

            var invites = dbContext.ClubInvites.Where(i => i.ClubId == club.Id && i.TouristId == -22).ToList();
            invites.Count.ShouldBe(1);

            var inviteId = invites.First().Id;
            inviteService.AcceptInvite(inviteId, -22);

            dbContext.ChangeTracker.Clear();
            var stored = dbContext.Clubs
                    .Include(c => c.Members)
                    .First(c => c.Id == club.Id);

            stored.IsMember(-22).ShouldBeTrue();
        }

        [Fact]
        public void Tourist_can_reject_invite()
        {
            using var scope = Factory.Services.CreateScope();
            var inviteService = scope.ServiceProvider.GetRequiredService<IClubInviteService>();
            var clubService = scope.ServiceProvider.GetRequiredService<IClubService>();
            var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();

            var club = clubService.Create(new ClubDto
            {
                Name = "Reject Klub",
                Description = "Test reject invite",
                CreatorId = -21
            }, new List<IFormFile> { CreateTestImage() });

            inviteService.InviteTourist(club.Id, -22, -21);

            var inviteId = dbContext.ClubInvites.First(i => i.ClubId == club.Id && i.TouristId == -22).Id;

            inviteService.RejectInvite(inviteId, -22);

            dbContext.ChangeTracker.Clear();
            dbContext.ClubInvites.Any(i => i.Id == inviteId).ShouldBeFalse();

            var stored = dbContext.Clubs.Find(club.Id);
            stored.IsMember(3).ShouldBeFalse();
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


        [Fact]
        public void Tourist_can_request_to_join_and_cancel_request()
        {
            using var scope = Factory.Services.CreateScope();
            var clubService = scope.ServiceProvider.GetRequiredService<IClubService>();
            var joinRequestService = scope.ServiceProvider.GetRequiredService<IClubJoinRequestService>();
            var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();


            var club = clubService.Create(new ClubDto
            {
                Name = "Tourist Klub",
                Description = "Test za turiste",
                CreatorId = -21
            }, new List<IFormFile> { CreateTestImage() });

            joinRequestService.RequestToJoin(club.Id, -22);

            dbContext.ChangeTracker.Clear();
            var pendingRequest = dbContext.ClubJoinRequests
                .FirstOrDefault(r => r.ClubId == club.Id && r.TouristId == -22);
            pendingRequest.ShouldNotBeNull();

            joinRequestService.CancelRequest(club.Id, -22);

            dbContext.ChangeTracker.Clear();
            dbContext.ClubJoinRequests
                .Any(r => r.ClubId == club.Id && r.TouristId == -22)
                .ShouldBeFalse();
        }

        [Fact]
        public void Tourist_membership_status_reflects_correctly()
        {
            using var scope = Factory.Services.CreateScope();
            var clubService = scope.ServiceProvider.GetRequiredService<IClubService>();
            var joinRequestService = scope.ServiceProvider.GetRequiredService<IClubJoinRequestService>();
            var dbContext = scope.ServiceProvider.GetRequiredService<StakeholdersContext>();

            var club = clubService.Create(new ClubDto
            {
                Name = "Status Klub",
                Description = "Test statusa",
                CreatorId = -21
            }, new List<IFormFile> { CreateTestImage() });

            joinRequestService.GetMembershipStatus(club.Id, -22).ShouldBe("None");

            joinRequestService.RequestToJoin(club.Id, -22);
            joinRequestService.GetMembershipStatus(club.Id, -22).ShouldBe("Pending");

            var joinRequest = dbContext.ClubJoinRequests
                .First(r => r.ClubId == club.Id && r.TouristId == -22);
            joinRequestService.AcceptRequest(club.Id, -22, -21);

            joinRequestService.GetMembershipStatus(club.Id, -22).ShouldBe("Member");
        }





    }
}
