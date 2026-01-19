using Explorer.Stakeholders.API.Dtos.Badges;
using Explorer.Stakeholders.API.Public.Badges;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Badges;

[Authorize(Policy = "authorOrTouristPolicy")]
[Route("api/user-statistics")]
[ApiController]
public class UserStatisticsController : ControllerBase
{
    private readonly IUserStatisticsService _userStatisticsService;

    public UserStatisticsController(IUserStatisticsService userStatisticsService)
    {
        _userStatisticsService = userStatisticsService;
    }

    [HttpGet("my")]
    public ActionResult<UserStatisticsDto> GetMyStatistics()
    {
        var userId = long.Parse(User.Claims.First(c => c.Type == "id").Value);
        var statistics = _userStatisticsService.GetByUserId(userId);
        return Ok(statistics);
    }

    [HttpGet("user/{userId:long}")]
    public ActionResult<UserStatisticsDto> GetUserStatistics(long userId)
    {
        var statistics = _userStatisticsService.GetByUserId(userId);
        return Ok(statistics);
    }

    [HttpGet("{id:long}")]
    public ActionResult<UserStatisticsDto> Get(long id)
    {
        try
        {
            var result = _userStatisticsService.Get(id);
            return Ok(result);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}
