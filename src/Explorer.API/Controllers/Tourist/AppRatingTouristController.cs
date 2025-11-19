using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist
{
    [Authorize(Policy = "touristPolicy")]
    [Route("api/tourist/app-ratings")]
    [ApiController]
    public class TouristAppRatingsController : ControllerBase
    {
        private readonly IAppRatingService _service;

        public TouristAppRatingsController(IAppRatingService service)
        {
            _service = service;
        }

       
        [HttpPost]
        public ActionResult<AppRatingDto> Create([FromBody] UpdateAppRatingDto dto)
        {
            var appRatingDto = new AppRatingDto
            {
                UserId = User.PersonId(),
                Rating = dto.Rating,
                Comment = dto.Comment,
                CreatedAt = DateTime.UtcNow
            };

            var created = _service.Create(appRatingDto);
            return Ok(created);
        }

       
        [HttpGet("my")]
        public ActionResult<IEnumerable<AppRatingDto>> GetMy()
        {
            var personId = User.PersonId();
            var ratings = _service.GetByUserId(personId);
            return Ok(ratings);
        }

      
        [HttpPut("{id:long}")]
        public ActionResult<AppRatingDto> Update(long id, [FromBody] UpdateAppRatingDto dto)
        {
            var existing = _service.Get(id);
            if (existing == null)
                return NotFound();

           
            if (existing.UserId != User.PersonId())
                return Forbid();

            var updatedDto = new AppRatingDto
            {
                Id = id,
                UserId = existing.UserId,
                Rating = dto.Rating,
                Comment = dto.Comment,
                CreatedAt = existing.CreatedAt
            };

            var updated = _service.Update(updatedDto);
            return Ok(updated);
        }

      
        [HttpDelete("{id:long}")]
        public IActionResult Delete(long id)
        {
            var existing = _service.Get(id);
            if (existing == null)
                return NotFound();

            
            if (existing.UserId != User.PersonId())
                return Forbid();

            _service.Delete(id);
            return NoContent();
        }
    }
}