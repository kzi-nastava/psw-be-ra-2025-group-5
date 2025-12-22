using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos.Notifications;

namespace Explorer.Stakeholders.API.Public.Notifications
{
    public interface INotificationService
    {
        NotificationDto Create(NotificationDto notification);
        NotificationDto GetById(long id);
        PagedResult<NotificationDto> GetPagedByUserId(long userId, int page, int pageSize);
        List<NotificationDto> GetUnreadByUserId(long userId);
        int GetUnreadCountByUserId(long userId);
        NotificationDto MarkAsRead(long notificationId);
        NotificationDto MarkAsUnread(long notificationId);
        void Delete(long id);
        void CreateProblemReportedNotification(long authorId, long tourId, long problemId, string tourName);
        void CreateCommentAddedNotification(long recipientId, long problemId, string commenterName, long tourId);
        void CreateDeadlineSetNotification(long authorId, long problemId, DateTimeOffset deadline, string tourName);
        void CreateTourClosedNotification(long authorId, long tourId, string tourName, string reason);
        void CreateProblemStatusChangedNotification(long recipientId, long problemId, bool isResolved, string changedByUsername, long tourId);
    }
}

