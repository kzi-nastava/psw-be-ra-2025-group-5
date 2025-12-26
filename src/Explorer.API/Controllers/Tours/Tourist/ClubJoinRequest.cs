using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist
{
    [Authorize(Policy = "touristPolicy")]
    [Route("api/tourist/club-requests")]
    [ApiController]
    public class ClubJoinRequestController : ControllerBase
    {
        private readonly IClubJoinRequestService _service;

        public ClubJoinRequestController(IClubJoinRequestService service)
        {
            _service = service;
        }

        [HttpPost("{clubId}")]
        public IActionResult RequestToJoin(long clubId)
        {
            var touristId = User.PersonId();
            _service.RequestToJoin(clubId, touristId);
            return Ok(new { message = "Join request sent successfully." });
        }

        [HttpDelete("{clubId}")]
        public IActionResult CancelRequest(long clubId)
        {
            var touristId = User.PersonId();
            _service.CancelRequest(clubId, touristId);
            return Ok(new { message = "Join request cancelled successfully." });
        }

        [HttpGet("{clubId}/status")]
        public IActionResult GetMembershipStatus(long clubId)
        {
            var touristId = User.PersonId();
            var status = _service.GetMembershipStatus(clubId, touristId);
            return Ok(new { Status = status });
        }

        [HttpPost("{clubId}/accept/{touristId}")]
        public IActionResult AcceptRequest(long clubId, long touristId)
        {
            var ownerId = User.PersonId();
            _service.AcceptRequest(clubId, touristId, ownerId);
            return Ok(new { message = "Request accepted successfully." });
        }

        [HttpPost("{clubId}/reject/{touristId}")]
        public IActionResult RejectRequest(long clubId, long touristId)
        {
            var ownerId = User.PersonId();
            _service.RejectRequest(clubId, touristId, ownerId);
            return Ok(new { message = "Request rejected successfully." });
        }

        [HttpGet("club/{clubId}/pending")]
        public IActionResult GetPendingRequests(long clubId)
        {
            var ownerId = User.PersonId();
            var requests = _service.GetPendingRequests(clubId, ownerId);
            return Ok(requests);
        }
    }
}
