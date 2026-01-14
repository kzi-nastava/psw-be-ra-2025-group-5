using Explorer.Encounters.API.Public.Author;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Explorer.Encounters.API.Dtos;
using Explorer.Stakeholders.API.Dtos.Users;
using Explorer.Stakeholders.API.Public.Users;

namespace Explorer.API.Controllers.Tours.Author
{
    [Authorize(Policy = "authorPolicy")]
    [Route("api/author/challenges")]
    [ApiController]
    public class AuthorChallengeController : ControllerBase
    {
        private readonly IKeyPointChallengeCreationService _keyPointChallengeCreationService;
        private readonly IProfileService _profileService;

        public AuthorChallengeController(IKeyPointChallengeCreationService keyPointChallengeCreationService, IProfileService profileService)
        {
            _keyPointChallengeCreationService = keyPointChallengeCreationService;
            _profileService = profileService;
        }

        [HttpPost]
        public ActionResult<KeyPointChallengeDto> Create([FromBody] CreateAuthorChallengeDto challenge)
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

            if (profile == null)
            {
                return Forbid();
            }

            return Ok(_keyPointChallengeCreationService.CreateByAuthor(challenge, profile.Id));
        }
    }
}
