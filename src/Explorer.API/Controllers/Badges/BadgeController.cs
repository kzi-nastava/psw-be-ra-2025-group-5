using Explorer.Stakeholders.API.Dtos.Badges;
using Explorer.Stakeholders.API.Public.Badges;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Badges;

[Route("api/badges")]
[ApiController]
public class BadgeController : ControllerBase
{
    private readonly IBadgeService _badgeService;
    private readonly IUserBadgeService _userBadgeService;

    public BadgeController(IBadgeService badgeService, IUserBadgeService userBadgeService)
    {
        _badgeService = badgeService;
        _userBadgeService = userBadgeService;
    }

    [HttpGet]
    public ActionResult<List<BadgeDto>> GetAll()
    {
        var result = _badgeService.GetAll();
        return Ok(result);
    }

    [HttpGet("filter/role/{role:int}")]
    public ActionResult<List<BadgeDto>> GetByRole(int role)
    {
        var result = _badgeService.GetByRole(role);
        return Ok(result);
    }

    [HttpGet("filter/rank/{rank:int}")]
    public ActionResult<List<BadgeDto>> GetByRank(int rank)
    {
        var result = _badgeService.GetByRank(rank);
        return Ok(result);
    }

    [HttpGet("filter/name/{name}")]
    public ActionResult<List<BadgeDto>> GetByName(string name)
    {
        var result = _badgeService.GetByName(name);
        return Ok(result);
    }

    [HttpGet("filter/owned")]
    public ActionResult<List<BadgeDto>> GetOwned()
    {
        var userId = long.Parse(User.Claims.First(c => c.Type == "id").Value);
        var result = _userBadgeService.GetByUserId(userId);
        return Ok(result);
    }

    [HttpGet("filter/not-owned")]
    public ActionResult<List<BadgeDto>> GetNotOwned()
    {
        var userId = long.Parse(User.Claims.First(c => c.Type == "id").Value);
        var result = _userBadgeService.GetNotOwnedByUser(userId);
        return Ok(result);
    }
}

