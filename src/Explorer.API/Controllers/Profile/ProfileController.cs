using Explorer.Stakeholders.API.Dtos.Users;
using Explorer.Stakeholders.API.Public.Users;
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
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    // PUT api/profile/{userId}
    [HttpPut("{userId}")]
    public ActionResult<ProfileDto> UpdateProfile(long userId, [FromBody] ProfileDto profile)
    {
        try
        {
            profile.Id = userId; 
            var updated = _profileService.Update(profile);
            return Ok(updated);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }   
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}

