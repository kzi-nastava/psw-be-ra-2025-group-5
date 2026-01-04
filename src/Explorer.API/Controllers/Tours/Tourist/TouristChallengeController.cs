using Explorer.Encounters.API.Dtos;
using Explorer.Encounters.API.Public.Administration;
using Explorer.Encounters.API.Public.Tourist;
using Explorer.Stakeholders.API.Dtos.Users;
using Explorer.Stakeholders.API.Public.Users;
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
    private readonly IProfileService _profileService;
    private readonly IChallengeCreationService _challengeTouristService;

    public TouristChallengeController(IChallengeService challengeService, IChallengeExecutionService challengeExecutionService, IProfileService profileService, IChallengeCreationService challengeCreationService)
    {
        _challengeService = challengeService;
        _challengeExecutionService = challengeExecutionService;
        _profileService = profileService;
        _challengeTouristService = challengeCreationService;
    }

    [HttpGet]
    public ActionResult<List<ChallengeDto>> GetAllActive()
    {
        var result = _challengeService.GetAllActive();
        // Izbaci one koje je korisnik vec zavrsio
        var completed = _challengeExecutionService.GetByTourist(long.Parse(User.Claims.First(c => c.Type == "id").Value))
            .Where(e => e.Status == "Completed" || e.Status == "InProgress" || e.Status == "Pending")
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

    [HttpPost]
    public ActionResult<ChallengeDto> Create([FromBody] CreateTouristChallengeDto challenge)
    {
        var userClaim = User.FindFirst("Id");
        if (userClaim == null || !long.TryParse(userClaim.Value, out var userId))
            return Unauthorized("UserId not found in token");

        ProfileDto? profile;
        try
        {
            profile = _profileService.GetByUserId(userId);
        }
        catch (KeyNotFoundException)
        {
            return Forbid();
        }

        if (profile == null || profile.Level < 10)
        {
            return Forbid();
        }

        return Ok(_challengeTouristService.CreateByTourist(challenge, profile.Id));
    }

    [HttpPut("{id:long}")]
    public ActionResult<ChallengeDto> Update(long id, [FromBody] UpdateTouristChallengeDto challenge)
    {
        challenge.Id = id;

        var userClaim = User.FindFirst("Id");
        if (userClaim == null || !long.TryParse(userClaim.Value, out var userId))
            return Unauthorized("UserId not found in token");

        try
        {
            var updatedChallenge = _challengeTouristService.Update(challenge, userId);
            return Ok(updatedChallenge);
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid("You can only edit your own challenges.");
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("my")]
    public ActionResult<List<ChallengeDto>> GetMyChallenges()
    {
        var userClaim = User.FindFirst("Id");
        if (userClaim == null || !long.TryParse(userClaim.Value, out var userId))
            return Unauthorized("UserId not found in token");

        var result = _challengeTouristService.GetByTourist(userId);
        return Ok(result);
    }

}