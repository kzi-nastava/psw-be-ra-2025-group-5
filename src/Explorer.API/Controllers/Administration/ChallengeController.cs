using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Encounters.API.Dtos;
using Explorer.Encounters.API.Public.Administration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Administrator.Administration;

[Authorize(Policy = "administratorPolicy")]
[Route("api/administration/challenge")]
[ApiController]
public class ChallengeController : ControllerBase
{
    private readonly IChallengeService _challengeService;

    public ChallengeController(IChallengeService challengeService)
    {
        _challengeService = challengeService;
    }

    [HttpGet]
    public ActionResult<PagedResult<ChallengeDto>> GetAll([FromQuery] int page, [FromQuery] int pageSize)
    {
        return Ok(_challengeService.GetPaged(page, pageSize));
    }

    [HttpPost]
    public ActionResult<ChallengeDto> Create([FromBody] ChallengeDto challenge)
    {
        return Ok(_challengeService.Create(challenge));
    }

    [HttpPut("{id:long}")]
    public ActionResult<ChallengeDto> Update([FromBody] ChallengeDto challenge)
    {
        return Ok(_challengeService.Update(challenge));
    }

    [HttpDelete("{id:long}")]
    public ActionResult Delete(long id)
    {
        _challengeService.Delete(id);
        return Ok();
    }

    [HttpPost("{id}/approve")]
    public ActionResult ApproveChallenge(long id)
    {
        _challengeService.Approve(id);
        return Ok();
    }

    [HttpPost("{id}/reject")]
    public ActionResult RejectChallenge(long id)
    {
        _challengeService.Reject(id);
        return Ok();
    }
}
