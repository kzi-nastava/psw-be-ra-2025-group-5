using Explorer.Stakeholders.API.Dtos.ClubMessages;
using Explorer.Stakeholders.API.Public.ClubMessages;
using Explorer.Stakeholders.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Explorer.API.Controllers.Social
{
    [Authorize(Policy = "touristPolicy")]
    [Route("api/clubs/{clubId}/messages")]
    [ApiController]
    public class ClubMessageController : ControllerBase
    {
        private readonly IClubMessageService _clubMessageService;

        public ClubMessageController(IClubMessageService clubMessageService)
        {
            _clubMessageService = clubMessageService;
        }

        [HttpGet]
        public ActionResult<List<ClubMessageDto>> GetByClubId(long clubId)
        {
            var result = _clubMessageService.GetByClubId(clubId);
            return Ok(result);
        }

        [HttpGet("{messageId}")]
        public ActionResult<ClubMessageDto> GetById(long messageId)
        {
            var result = _clubMessageService.GetById(messageId);
            return Ok(result);
        }

        [HttpPost]
        public ActionResult<ClubMessageDto> Create(long clubId, [FromBody] CreateClubMessageDto dto)
        {
            var result = _clubMessageService.Create(clubId, User.PersonId(), dto);
            return Ok(result);
        }

        [HttpPut("{messageId}")]
        public ActionResult<ClubMessageDto> Update(long messageId, [FromBody] UpdateClubMessageDto dto)
        {
            var result = _clubMessageService.Update(messageId, User.PersonId(), dto);
            return Ok(result);
        }

        [HttpDelete("{messageId}")]
        public ActionResult Delete(long clubId, long messageId)
        {
            _clubMessageService.Delete(messageId, User.PersonId(), false);
            return Ok();
        }
    }
}
