using Explorer.Stakeholders.API.Dtos.ProfileMessages;
using Explorer.Stakeholders.API.Public.ProfileMessages;
using Explorer.Stakeholders.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Social
{
    [Authorize(Policy = "authorTouristAdminPolicy")]
    [Route("api/profile/messages")]
    [ApiController]
    public class ProfileMessageController : ControllerBase
    {
        private readonly IProfileMessageService _profileMessageService;

        public ProfileMessageController(IProfileMessageService profileMessageService)
        {
            _profileMessageService = profileMessageService;
        }

        [HttpPost("{receiverId}")]
        public ActionResult<ProfileMessageDto> Create(long receiverId, [FromBody] CreateMessageDto dto)
        {
            var result = _profileMessageService.Create(receiverId, User.PersonId(), dto);
            return Ok(result);
        }

        [HttpPut("{messageId}")]
        public ActionResult<ProfileMessageDto> Update(long messageId, [FromBody] ProfileMessageDto dto)
        {
            var result = _profileMessageService.Update(messageId, User.PersonId(), dto);
            return Ok(result);
        }

        [HttpDelete("{messageId}")]
        public ActionResult Delete(long messageId)
        {
            _profileMessageService.Delete(messageId, User.PersonId());
            return Ok();
        }

        [HttpGet]
        public ActionResult<List<ProfileMessageDto>> GetByReceiverId()
        {
            var result = _profileMessageService.GetByReceiverId(User.PersonId());
            return Ok(result);
        }

        [HttpGet("{messageId}")]
        public ActionResult<ProfileMessageDto> GetById(long messageId)
        {
            var result = _profileMessageService.GetById(messageId);
            return Ok(result);
        }
    }
}
