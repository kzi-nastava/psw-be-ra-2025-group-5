using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using Explorer.Stakeholders.Core.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Explorer.Stakeholders.Core.UseCases.ClubMembership;
using Explorer.Stakeholders.Infrastructure.Authentication;

namespace Explorer.API.Controllers.Tourist
{
    [Authorize(Policy = "touristPolicy")]
    [Route("api/clubsMembership")]
    [ApiController]
    public class ClubMembershipContoller : ControllerBase
    {
        private readonly IClubInviteService _clubInviteService;

        public ClubMembershipContoller(IClubInviteService inviteService)
        {
            _clubInviteService = inviteService;
        }

        [Authorize(Roles = "tourist")]
        [HttpPost("{clubId}/invite/{touristId}")]
        public IActionResult InviteTourist(long clubId, long touristId)
        {
            var ownerId = long.Parse(User.FindFirst("personId")!.Value);

            _clubInviteService.InviteTourist(clubId, touristId, ownerId);

            return Ok();
        }

        [Authorize(Roles = "tourist")]
        [HttpPost("invites/{inviteId}/accept")]
        public IActionResult AcceptInvite(long inviteId)
        {
            var touristId = long.Parse(User.FindFirst("personId")!.Value);

            _clubInviteService.AcceptInvite(inviteId, touristId);

            return Ok();
        }

        [Authorize(Roles = "tourist")]
        [HttpPost("invites/{inviteId}/reject")]
        public IActionResult RejectInvite(long inviteId)
        {
            var touristId = long.Parse(User.FindFirst("personId")!.Value);

            _clubInviteService.RejectInvite(inviteId, touristId);

            return Ok();
        }

    }
}

