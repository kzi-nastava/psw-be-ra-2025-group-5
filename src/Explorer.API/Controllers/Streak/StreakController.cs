using Explorer.Stakeholders.API.Dtos.Streaks;
using Explorer.Stakeholders.API.Public.Streaks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Explorer.API.Controllers.Streak
{
    [Authorize(Policy = "touristPolicy")]
    [ApiController]
    [Route("api/streak")]
    public class StreakController : ControllerBase
    {
        private readonly IStreakService _streakService;

        public StreakController(IStreakService streakService)
        {
            _streakService = streakService;
        }

        [HttpPost]
        public ActionResult<StreakDto> RecordActivity()
        {
            long userId = long.Parse(User.Claims.First(c => c.Type == "id").Value);

            _streakService.RecordActivity(userId);
            var streak = _streakService.GetStreakForUser(userId);

            return Ok(streak);
        }



        [HttpGet]
        public ActionResult GetActivity()
        {
            long userId = long.Parse(User.Claims.First(c => c.Type == "id").Value);
            var streak = _streakService.GetStreakForUser(userId);

            return Ok(streak);
        }
    }
}
