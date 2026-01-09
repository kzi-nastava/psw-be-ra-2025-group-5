using Explorer.Encounters.API.Public.Author;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Explorer.Encounters.API.Dtos;

namespace Explorer.API.Controllers.Tours.Author
{
    [Authorize(Policy = "authorPolicy")]
    [Route("api/author/challenges")]
    [ApiController]
    public class AuthorChallengeController : ControllerBase
    {
        private readonly IKeyPointChallengeCreationService _keyPointChallengeCreationService;

        public AuthorChallengeController(IKeyPointChallengeCreationService keyPointChallengeCreationService)
        {
            _keyPointChallengeCreationService = keyPointChallengeCreationService;
        }

        [HttpPost]
        public ActionResult<CreateAuthorChallengeDto> Create([FromBody] CreateAuthorChallengeDto challenge)
        {
            var userClaim = User.FindFirst("Id");
            if (userClaim == null || !long.TryParse(userClaim.Value, out var userId))
                return Unauthorized("UserId not found in token");

            var createdChallenge = _keyPointChallengeCreationService.CreateByAuthor(challenge, challenge.CreatedByTouristId);
            return Ok(createdChallenge);
        }
    }
}
