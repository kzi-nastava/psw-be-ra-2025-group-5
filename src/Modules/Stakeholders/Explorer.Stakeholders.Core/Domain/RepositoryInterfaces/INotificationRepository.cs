using Explorer.BuildingBlocks.Core.UseCases;

namespace Explorer.Stakeholders.Core.Domain.RepositoryInterfaces
{
    public interface INotificationRepository
    {
        Notification Create(Notification entity);
        Notification Get(long id);
        PagedResult<Notification> GetPagedByUserId(long userId, int page, int pageSize);
        List<Notification> GetUnreadByUserId(long userId);
        Notification Update(Notification entity);
        void Delete(long id);
        int GetUnreadCountByUserId(long userId);
    }
}
