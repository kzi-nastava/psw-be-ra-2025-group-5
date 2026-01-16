using Explorer.BuildingBlocks.Core.Exceptions;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Dtos.Users;
using Explorer.Stakeholders.API.Public.Users;
using Explorer.Stakeholders.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Profile;


[ApiController]
[Route("api/[controller]")]
public class ProfileController : ControllerBase
{
    private readonly IProfileService _profileService;

    public ProfileController(IProfileService profileService)
    {
        _profileService = profileService;
    }

    // GET api/profile/{userId}
    [HttpGet("{userId}")]
    public ActionResult<ProfileDto> GetProfile(long userId)
    {
        var loggedInUserId = long.Parse(User.FindFirst("id").Value);

        if (loggedInUserId != userId)
        {
            return Forbid();
        }


        try
        {
            var profile = _profileService.GetByUserId(userId);
            return Ok(profile);
        }
        catch (ForbiddenException)
        {
            return Forbid();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPut("{userId}")]
    [Consumes("multipart/form-data")]
    public ActionResult<ProfileDto> UpdateProfile(long userId, [FromForm] UpdateProfileDto dto)
    {
        var loggedInUserId = User.PersonId();
        if (loggedInUserId != userId)
            return Forbid();

        try
        {
            var profileDto = new ProfileDto
            {
                Id = userId,
                Name = dto.Name,
                Surname = dto.Surname,
                Email = dto.Email,
                Biography = dto.Biography,
                Motto = dto.Motto
            };

            var result = _profileService.Update(profileDto, dto.ProfileImage);
            return Ok(result);
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

    [AllowAnonymous]
    [HttpGet("{tourId:long}/thumbnail/{*fileName}")]
    public IActionResult GetThumbnail(long tourId, string fileName)
    {
        var filePath = Path.Combine(
            Directory.GetCurrentDirectory(),
            "UserUploads",
            "tours",
            tourId.ToString(),
            fileName);

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

    [Authorize]
    [HttpGet("public/profile/{userId}")]
    public ActionResult<ProfileDto> GetPublicProfile(long userId)
    {
        try
        {
            var profile = _profileService.GetPublicProfile(userId); 
            return Ok(profile);
        }
        catch (ForbiddenException)
        {
            return Forbid();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [Authorize]
    [HttpGet("all-profiles")]
    public ActionResult<PagedResult<ProfileDto>> GetAllProfiles([FromQuery] int page, [FromQuery] int pageSize)
    {
        var result = _profileService.GetPaged(page, pageSize);
        return Ok(result);
    }

}

