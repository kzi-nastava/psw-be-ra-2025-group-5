using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Encounters.API.Dtos;
using Explorer.Encounters.API.Public.Administration;
using Explorer.Stakeholders.API.Dtos.Users;
using Explorer.Stakeholders.Core.Domain.Users;
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
    public ActionResult<PagedResult<ChallengeResponseDto>> GetAll([FromQuery] int page, [FromQuery] int pageSize)
    {
        return Ok(_challengeService.GetPaged(page, pageSize));
    }

    [HttpPost]
    [Consumes("multipart/form-data")]
    public ActionResult<ChallengeResponseDto> Create([FromForm] ChallengeCreateDto challenge)
    {
        try
        {
            var challengeDto = new ChallengeResponseDto
            {
                Name = challenge.Name,
                Description = challenge.Description,
                Latitude = challenge.Latitude,
                Longitude = challenge.Longitude,
                ExperiencePoints = challenge.ExperiencePoints,
                Status = challenge.Status,
                Type = challenge.Type,
                CreatedById = challenge.CreatedById,
                RequiredParticipants = challenge.RequiredParticipants,
                RadiusInMeters = challenge.RadiusInMeters
            };

            return Ok(_challengeService.Create(challengeDto, challenge.Image));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Internal server error", details = ex.Message });
        }
    }

    [HttpPut("{id:long}")]
    [Consumes("multipart/form-data")]
    public ActionResult<ChallengeResponseDto> Update([FromForm] ChallengeCreateDto challenge)
    {
        try
        {
            var challengeDto = new ChallengeResponseDto
            {
                Id = challenge.Id,
                Name = challenge.Name,
                Description = challenge.Description,
                Latitude = challenge.Latitude,
                Longitude = challenge.Longitude,
                ExperiencePoints = challenge.ExperiencePoints,
                Status = challenge.Status,
                Type = challenge.Type,
                CreatedById = challenge.CreatedById,
                RequiredParticipants = challenge.RequiredParticipants,
                RadiusInMeters = challenge.RadiusInMeters
            };

            return Ok(_challengeService.Update(challengeDto, challenge.Image));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Internal server error", details = ex.Message });
        }
        
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

    [AllowAnonymous]
    [HttpGet("{chId:long}/images/{*fileName}")]
    public IActionResult GetImage(long chId, string fileName)
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "UserUploads", "challenges", chId.ToString(), fileName);

        if (!System.IO.File.Exists(filePath))
            return NotFound();

        var ext = Path.GetExtension(fileName).ToLower();
        var mime = ext switch
        {
            ".png" => "image/png",
            ".jpg" => "image/jpeg",
            ".jpeg" => "image/jpeg",
            _ => "application/octet-stream"
        };
        return PhysicalFile(filePath, mime);
    }
}
