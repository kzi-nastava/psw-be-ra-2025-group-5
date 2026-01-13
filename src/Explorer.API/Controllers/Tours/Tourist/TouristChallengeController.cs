using Explorer.Encounters.API.Dtos;
using Explorer.Encounters.API.Public.Administration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist;

[Authorize(Policy = "touristPolicy")]
[Route("api/tourist/challenges")]
[ApiController]
public class TouristChallengeController : ControllerBase
{
    private readonly IChallengeService _ChallengeService;

    public TouristChallengeController(IChallengeService ChallengeService)
    {
        _ChallengeService = ChallengeService;
    }

    [HttpGet]
    public ActionResult<List<ChallengeDto>> GetAllActive()
    {
        return Ok(_ChallengeService.GetAllActive());
    }
}