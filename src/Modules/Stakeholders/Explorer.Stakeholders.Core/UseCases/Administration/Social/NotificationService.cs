using AutoMapper;
using Explorer.BuildingBlocks.Core.Exceptions;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos.Notifications;
using Explorer.Stakeholders.API.Public.Notifications;
using Explorer.Stakeholders.Core.Domain.Notifications;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces.Notifications;

namespace Explorer.Stakeholders.Core.UseCases.Administration.Social
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _repository;
        private readonly IMapper _mapper;

        public NotificationService(INotificationRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public NotificationDto Create(NotificationDto dto)
        {
            var notification = _mapper.Map<Notification>(dto);
            var created = _repository.Create(notification);
            return _mapper.Map<NotificationDto>(created);
        }

        public NotificationDto GetById(long id)
        {
            var notification = _repository.Get(id);
            if (notification == null)
                throw new NotFoundException($"Notification {id} not found");

            return _mapper.Map<NotificationDto>(notification);
        }

        public PagedResult<NotificationDto> GetPagedByUserId(long userId, int page, int pageSize)
        {
            var result = _repository.GetPagedByUserId(userId, page, pageSize);
            var items = result.Results.Select(_mapper.Map<NotificationDto>).ToList();
            return new PagedResult<NotificationDto>(items, result.TotalCount);
        }

        public List<NotificationDto> GetUnreadByUserId(long userId)
        {
            var notifications = _repository.GetUnreadByUserId(userId);
            return notifications.Select(_mapper.Map<NotificationDto>).ToList();
        }

        public int GetUnreadCountByUserId(long userId)
        {
            return _repository.GetUnreadCountByUserId(userId);
        }

        public NotificationDto MarkAsRead(long notificationId)
        {
            var notification = _repository.Get(notificationId);
            if (notification == null)
                throw new NotFoundException($"Notification {notificationId} not found");

            notification.MarkAsRead();
            var updated = _repository.Update(notification);
            return _mapper.Map<NotificationDto>(updated);
        }

        public NotificationDto MarkAsUnread(long notificationId)
        {
            var notification = _repository.Get(notificationId);
            if (notification == null)
                throw new NotFoundException($"Notification {notificationId} not found");

            notification.MarkAsUnread();
            var updated = _repository.Update(notification);
            return _mapper.Map<NotificationDto>(updated);
        }

        public void Delete(long id)
        {
            _repository.Delete(id);
        }

        public void CreateProblemReportedNotification(long authorId, long tourId, long problemId, string tourName)
        {
            var notification = new Notification(
                userId: authorId,
                type: NotificationType.ProblemReported,
                title: "New Problem Reported",
                message: $"A new problem has been reported on your tour '{tourName}'.",
                tourProblemId: problemId,
                tourId: tourId,
                actionUrl: $"/tour-problem/{problemId}/comments"
            );

            _repository.Create(notification);
        }

        public void CreateCommentAddedNotification(long recipientId, long problemId, string commenterName, long tourId)
        {
            var notification = new Notification(
                userId: recipientId,
                type: NotificationType.CommentAdded,
                title: "New Comment on Problem",
                message: $"{commenterName} has added a comment to a tour problem discussion.",
                tourProblemId: problemId,
                tourId: tourId,
                actionUrl: $"/tour-problem/{problemId}/comments"
            );

            _repository.Create(notification);
        }

        public void CreateDeadlineSetNotification(long authorId, long problemId, DateTimeOffset deadline, string tourName)
        {
            var notification = new Notification(
                userId: authorId,
                type: NotificationType.DeadlineSet,
                title: "Deadline Set for Problem Resolution",
                message: $"An administrator has set a deadline ({deadline:yyyy-MM-dd HH:mm}) for resolving a problem on your tour '{tourName}'.",
                tourProblemId: problemId,
                tourId: null,
                actionUrl: $"/tour-problem/{problemId}/comments"
            );

            _repository.Create(notification);
        }

        public void CreateTourClosedNotification(long authorId, long tourId, string tourName, string reason)
        {
            var notification = new Notification(
                userId: authorId,
                type: NotificationType.TourClosed,
                title: "Tour Closed",
                message: $"Your tour '{tourName}' has been closed by an administrator. Reason: {reason}",
                tourProblemId: null,
                tourId: tourId,
                actionUrl: "/tours"
            );

            _repository.Create(notification);
        }

        public void CreateProblemStatusChangedNotification(long recipientId, long problemId, bool isResolved, string changedByUsername, long tourId)
        {
            var status = isResolved ? "resolved" : "unresolved";
            var notification = new Notification(
                userId: recipientId,
                type: NotificationType.ProblemStatusChanged,
                title: "Problem Status Changed",
                message: $"{changedByUsername} has marked the problem as {status}.",
                tourProblemId: problemId,
                tourId: tourId,
                actionUrl: $"/tour-problem/{problemId}/comments"
            );

            _repository.Create(notification);
        }
    }
}
