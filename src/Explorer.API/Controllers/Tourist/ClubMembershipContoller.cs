using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using Explorer.Stakeholders.Core.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Explorer.Stakeholders.Core.UseCases.ClubMembership;
using Explorer.Stakeholders.Infrastructure.Authentication;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.Core.UseCases;

namespace Explorer.API.Controllers.Tourist
{
    [Authorize(Policy = "touristPolicy")]
    [Route("api/clubsMembership")]
    [ApiController]
    public class ClubMembershipContoller : ControllerBase
    {
        private readonly IClubInviteService _clubInviteService;
        private readonly IClubService _clubService;

        public ClubMembershipContoller(IClubInviteService inviteService, IClubService clubService)
        {
            _clubInviteService = inviteService;
            _clubService = clubService;
        }

        [HttpPost("{clubId}/invite/{touristId}")]
        public IActionResult InviteTourist(long clubId, long touristId)
        {
            var ownerId = long.Parse(User.FindFirst("personId")!.Value);

            _clubInviteService.InviteTourist(clubId, touristId, ownerId);

            return Ok();
        }

        [HttpPost("invites/{inviteId}/accept")]
        public IActionResult AcceptInvite(long inviteId)
        {
            var touristId = long.Parse(User.FindFirst("personId")!.Value);

            _clubInviteService.AcceptInvite(inviteId, touristId);

            return Ok();
        }

        [HttpPost("invites/{inviteId}/reject")]
        public IActionResult RejectInvite(long inviteId)
        {
            var touristId = long.Parse(User.FindFirst("personId")!.Value);

            _clubInviteService.RejectInvite(inviteId, touristId);

            return Ok();
        }

        [HttpGet("{clubId}/invites")]
        public ActionResult<List<ClubInviteDto>> GetInvitesForClub(long clubId)
        {
            var ownerId = long.Parse(User.FindFirst("personId")!.Value);

            var result = _clubInviteService.GetInvitesForClub(clubId, ownerId);
            return Ok(result);
        }

        [HttpGet("{clubId}/invite/available-tourists")]
        public IActionResult GetAvailableTourists(long clubId)
        {
            var ownerId = User.PersonId();

            var result = _clubInviteService.GetAvailableTourists(clubId, ownerId);
            return Ok(result);
        }

        [Authorize(Roles = "tourist")]
        [HttpGet("{clubId}/my-invite")]
        public ActionResult<ClubInviteDto?> GetMyInvite(long clubId)
        {
            var touristId = User.PersonId();

            var invite = _clubInviteService.GetInviteForUserAndClub(clubId, touristId);

            return Ok(invite);
        }

    }
}

