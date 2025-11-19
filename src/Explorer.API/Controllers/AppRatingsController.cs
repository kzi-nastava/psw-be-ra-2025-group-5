using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers
{
    [Authorize(Roles = "tourist,author")]
    [Route("api/app-ratings")]
    [ApiController]
    public class AppRatingsController : ControllerBase
    {
        private readonly IAppRatingService _service;

        public AppRatingsController(IAppRatingService service)
        {
            _service = service;
        }

      
        [HttpGet("my")]
        public ActionResult<IEnumerable<AppRatingDto>> GetMyRating()
        {
            long userId = long.Parse(User.Claims.First(c => c.Type == "id").Value);
            return Ok(_service.GetByUserId(userId));
        }

       
        [HttpPost]
        public ActionResult<AppRatingDto> Create([FromBody] CreateAppRatingDto dto)
        {
            long userId = long.Parse(User.Claims.First(c => c.Type == "id").Value);

            var newRating = new AppRatingDto
            {
                UserId = userId,
                Rating = dto.Rating,
                Comment = dto.Comment,
                CreatedAt = DateTime.UtcNow
            };

            return Ok(_service.Create(newRating));
        }

     
        [HttpPut("{id:long}")]
        public ActionResult<AppRatingDto> Update(long id, [FromBody] UpdateAppRatingDto dto)
        {
            long userId = long.Parse(User.Claims.First(c => c.Type == "id").Value);

            var existing = _service.Get(id);
            if (existing == null) return NotFound();
            if (existing.UserId != userId) return Forbid();

            var updatedDto = new AppRatingDto
            {
                Id = id,
                UserId = userId,
                Rating = dto.Rating,
                Comment = dto.Comment,
                CreatedAt = existing.CreatedAt
            };

            return Ok(_service.Update(updatedDto));
        }

     
        [HttpDelete("{id:long}")]
        public IActionResult Delete(long id)
        {
            long userId = long.Parse(User.Claims.First(c => c.Type == "id").Value);

            var existing = _service.Get(id);
            if (existing == null) return NotFound();
            if (existing.UserId != userId) return Forbid();

            _service.Delete(id);
            return NoContent();
        }
    }
}
