using Explorer.Encounters.API.Dtos;
using Explorer.Encounters.API.Public.Administration;
using Explorer.Encounters.API.Public.Tourist;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist;

[Authorize(Policy = "touristPolicy")]
[Route("api/tourist/challenges")]
[ApiController]
public class TouristChallengeController : ControllerBase
{
    private readonly IChallengeService _challengeService;
    private readonly IChallengeExecutionService _challengeExecutionService;

    public TouristChallengeController(IChallengeService challengeService, IChallengeExecutionService challengeExecutionService)
    {
        _challengeService = challengeService;
        _challengeExecutionService = challengeExecutionService;
    }

    [HttpGet]
    public ActionResult<List<ChallengeDto>> GetAllActive()
    {
        var result = _challengeService.GetAllActive();
        // Izbaci one koje je korisnik vec zavrsio
        var completed = _challengeExecutionService.GetByTourist(long.Parse(User.Claims.First(c => c.Type == "id").Value))
            .Where(e => e.Status == "Completed")
            .Select(e => e.ChallengeId)
            .ToHashSet();
        
        result = result.Where(challenge => !completed.Contains(challenge.Id)).ToList();

        return Ok(result);
    }

    [HttpGet("{challengeId:long}")]
    public ActionResult<ChallengeDto> GetById(long challengeId)
    {
        return Ok(_challengeService.GetById(challengeId));
    }
}