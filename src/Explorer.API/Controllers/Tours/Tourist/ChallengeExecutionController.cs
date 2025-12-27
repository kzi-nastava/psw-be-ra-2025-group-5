using Explorer.Encounters.API.Dtos;
using Explorer.Encounters.API.Public.Tourist;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist;

[Authorize(Policy = "touristPolicy")]
[Route("api/tourist/challenge-execution")]
[ApiController]
public class ChallengeExecutionController : ControllerBase
{
    private readonly IChallengeExecutionService _challengeExecutionService;

    public ChallengeExecutionController(IChallengeExecutionService challengeExecutionService)
    {
        _challengeExecutionService = challengeExecutionService;
    }

    [HttpPost("start/{challengeId:long}")]
    public ActionResult<ChallengeExecutionDto> StartChallenge(long challengeId)
    {
        var touristId = long.Parse(User.Claims.First(c => c.Type == "id").Value);
        
        var result = _challengeExecutionService.StartChallenge(challengeId, touristId);
        return Ok(result);
    }

    [HttpPut("complete/{executionId:long}")]
    public ActionResult<ChallengeExecutionDto> CompleteChallenge(long executionId)
    {
        var touristId = long.Parse(User.Claims.First(c => c.Type == "id").Value);
        
        var result = _challengeExecutionService.CompleteChallenge(executionId, touristId);
        return Ok(result);
    }

    [HttpPut("abandon/{executionId:long}")]
    public ActionResult<ChallengeExecutionDto> AbandonChallenge(long executionId)
    {
        var touristId = long.Parse(User.Claims.First(c => c.Type == "id").Value);
        
        var result = _challengeExecutionService.AbandonChallenge(executionId, touristId);
        return Ok(result);
    }

    [HttpGet("{id:long}")]
    public ActionResult<ChallengeExecutionDto> GetById(long id)
    {
        var result = _challengeExecutionService.GetById(id);
        return Ok(result);
    }

    [HttpGet("my-executions")]
    public ActionResult<List<ChallengeExecutionDto>> GetMyExecutions()
    {
        var touristId = long.Parse(User.Claims.First(c => c.Type == "id").Value);
        
        var result = _challengeExecutionService.GetByTourist(touristId);
        return Ok(result);
    }
}

