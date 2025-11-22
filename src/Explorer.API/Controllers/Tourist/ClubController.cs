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
        public ActionResult<ClubDto> Create([FromBody] ClubDto clubDto)
        {
            clubDto.CreatorId = User.PersonId();
            var result = _clubService.Create(clubDto);
            return Ok(result);
        }

        [HttpPut("{id:long}")]
        public ActionResult<ClubDto> Update(long id, [FromBody] ClubDto clubDto)
        {
            clubDto.Id = id;
            clubDto.CreatorId = User.PersonId();
            var result = _clubService.Update(clubDto);
            return Ok(result);
        }

        [HttpDelete("{id:long}")]
        public ActionResult Delete(long id)
        { 
            _clubService.Delete(User.PersonId(), id);
            return Ok();
        }
    }
}
