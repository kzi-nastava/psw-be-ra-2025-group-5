using Explorer.Stakeholders.API.Dtos.Badges;
using Explorer.Stakeholders.API.Public.Badges;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Badges;

[Authorize(Policy = "authorOrTouristPolicy")]
[Route("api/user-badges")]
[ApiController]
public class UserBadgeController : ControllerBase
{
    private readonly IUserBadgeService _userBadgeService;

    public UserBadgeController(IUserBadgeService userBadgeService)
    {
        _userBadgeService = userBadgeService;
    }

    [HttpGet("my")]
    public ActionResult<List<UserBadgeDto>> GetMyBadges()
    {
        var userId = long.Parse(User.Claims.First(c => c.Type == "id").Value);
        var badges = _userBadgeService.GetByUserId(userId);
        return Ok(badges);
    }

    [HttpGet("my/best")]
    public ActionResult<List<UserBadgeDto>> GetMyBestBadges()
    {
        var userId = long.Parse(User.Claims.First(c => c.Type == "id").Value);
        var badges = _userBadgeService.GetBestBadgesByUserId(userId);
        return Ok(badges);
    }

    [HttpGet("user/{userId:long}")]
    public ActionResult<List<UserBadgeDto>> GetUserBadges(long userId)
    {
        var badges = _userBadgeService.GetByUserId(userId);
        return Ok(badges);
    }

    [HttpGet("user/{userId:long}/best")]
    public ActionResult<List<UserBadgeDto>> GetUserBestBadges(long userId)
    {
        var badges = _userBadgeService.GetBestBadgesByUserId(userId);
        return Ok(badges);
    }

    [HttpGet("{id:long}")]
    public ActionResult<UserBadgeDto> Get(long id)
    {
        try
        {
            var result = _userBadgeService.Get(id);
            return Ok(result);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}
