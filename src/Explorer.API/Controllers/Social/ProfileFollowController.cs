using Explorer.BuildingBlocks.Core.Exceptions;
using Explorer.Stakeholders.API.Dtos.Social;
using Explorer.Stakeholders.API.Dtos.Users;
using Explorer.Stakeholders.API.Public.Social;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Social;

[Authorize(Policy = "authorTouristAdminPolicy")]
[Route("api/profile/social")]
[ApiController]
public class ProfileFollowController : ControllerBase
{
    private readonly IProfileFollowService _FollowService;

    public ProfileFollowController(IProfileFollowService followService)
    {
        _FollowService = followService;
    }

    [HttpPost("{profileId:long}/follow/{targetProfileId:long}")]
    public ActionResult<ProfileFollowDto> Follow(long profileId, long targetProfileId)
    {
        if (profileId == targetProfileId) return BadRequest("You cannot follow yourself.");

        try { return Ok(_FollowService.Follow(new ProfileFollowDto { FollowerId = profileId, FollowingId = targetProfileId })); }
        catch (NotFoundException) { return NotFound(); }
        catch (InvalidOperationException) { return Conflict("This follow relationship already exists"); }
    }

    [HttpDelete("{profileId:long}/unfollow/{targetProfileId:long}")]
    public ActionResult Unfollow(long profileId, long targetProfileId)
    {
        try
        {
            _FollowService.Unfollow(new ProfileFollowDto { FollowerId = profileId, FollowingId = targetProfileId });
            return Ok();
        }
        catch (NotFoundException)
        {
            return NotFound($"Follow relationship not found for {profileId} -> {targetProfileId}");
        }
    }

    [HttpGet("{profileId:long}/followers")]
    public async Task<ActionResult<IEnumerable<FollowerDto>>> GetFollowers(long profileId)
    {
        return Ok(await _FollowService.GetFollowers(profileId));
    }

    [HttpGet("{profileId:long}/following")]
    public async Task<ActionResult<IEnumerable<FollowingDto>>> GetFollowing(long profileId)
    {
        return Ok(await _FollowService.GetFollowing(profileId));
    }
}
