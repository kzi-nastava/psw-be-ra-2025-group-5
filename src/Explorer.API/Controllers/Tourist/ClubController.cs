using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Explorer.API.Controllers.Tourist
{
    [Authorize(Policy = "touristPolicy")]
    //[Authorize(Roles = "Tourist")]
    [Route("api/clubs")]
    [ApiController]
    public class ClubController : ControllerBase
    {
        private readonly IClubService _clubService;

        public ClubController(IClubService clubService)
        {
            _clubService = clubService;
        }

        [HttpGet]
        public ActionResult<List<ClubDto>> GetAll()
        {
            var result = _clubService.GetAll();
            return Ok(result);
        }

        [HttpGet("{id:long}")]
        public ActionResult<ClubDto> GetById(int id)
        {
            var result = _clubService.GetById(id);
            return Ok(result);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public IActionResult Create([FromForm] CreateClubDto dto)
        {
            var clubDto = new ClubDto
            {
                Name = dto.Name,
                Description = dto.Description,
                CreatorId = User.PersonId()
            };

            var result = _clubService.Create(clubDto, dto.Images);
            return Ok(result);
        }

        [HttpPut("{id:long}")]
        [Consumes("multipart/form-data")]
        public IActionResult Update(long id, [FromForm] CreateClubDto dto)
        {

            var clubDto = new ClubDto
            {
                Id = id,
                Name = dto.Name,
                Description = dto.Description,
                CreatorId = User.PersonId()
            };

            var result = _clubService.Update(clubDto, dto.Images);
            return Ok(result);
        }

        [HttpDelete("{id:long}")]
        public ActionResult Delete(long id)
        { 
            _clubService.Delete(User.PersonId(), id);
            return Ok();
        }

        [HttpDelete("{clubId:long}/images")]
        public IActionResult RemoveImage(long clubId, [FromBody] string imagePath)
        {
            var result = _clubService.RemoveImage(User.PersonId(), clubId, imagePath);
            return Ok(result);
        }
    }
}
