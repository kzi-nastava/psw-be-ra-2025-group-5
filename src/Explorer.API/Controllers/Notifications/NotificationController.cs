using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos.Notifications;
using Explorer.Stakeholders.API.Public.Notifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Notifications
{
    [Route("api/notifications")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet("my")]
        [Authorize(Policy = "authorTouristAdminPolicy")]
        public ActionResult<PagedResult<NotificationDto>> GetMyNotifications([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            long userId = long.Parse(User.Claims.First(c => c.Type == "id").Value);
            var result = _notificationService.GetPagedByUserId(userId, page, pageSize);
            return Ok(result);
        }

        [HttpGet("my/unread")]
        [Authorize(Policy = "authorTouristAdminPolicy")]
        public ActionResult<List<NotificationDto>> GetMyUnreadNotifications()
        {
            long userId = long.Parse(User.Claims.First(c => c.Type == "id").Value);
            var result = _notificationService.GetUnreadByUserId(userId);
            return Ok(result);
        }

        [HttpGet("my/unread-count")]
        [Authorize(Policy = "authorTouristAdminPolicy")]
        public ActionResult<int> GetMyUnreadCount()
        {
            long userId = long.Parse(User.Claims.First(c => c.Type == "id").Value);
            var count = _notificationService.GetUnreadCountByUserId(userId);
            return Ok(count);
        }

        [HttpPut("{id}/mark-as-read")]
        [Authorize(Policy = "authorTouristAdminPolicy")]
        public ActionResult<NotificationDto> MarkAsRead(long id)
        {
            long userId = long.Parse(User.Claims.First(c => c.Type == "id").Value);
            
            var notification = _notificationService.GetById(id);
            if (notification.UserId != userId)
                return Forbid();

            var result = _notificationService.MarkAsRead(id);
            return Ok(result);
        }

        [HttpPut("{id}/mark-as-unread")]
        [Authorize(Policy = "authorTouristAdminPolicy")]
        public ActionResult<NotificationDto> MarkAsUnread(long id)
        {
            long userId = long.Parse(User.Claims.First(c => c.Type == "id").Value);
            
            var notification = _notificationService.GetById(id);
            if (notification.UserId != userId)
                return Forbid();

            var result = _notificationService.MarkAsUnread(id);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "authorTouristAdminPolicy")]
        public ActionResult Delete(long id)
        {
            long userId = long.Parse(User.Claims.First(c => c.Type == "id").Value);
            
            var notification = _notificationService.GetById(id);
            if (notification.UserId != userId)
                return Forbid();

            _notificationService.Delete(id);
            return Ok();
        }
    }
}
