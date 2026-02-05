using Explorer.Stakeholders.Core.Domain.Badges;

namespace Explorer.Stakeholders.Core.Domain.RepositoryInterfaces.Badges;

public interface IUserBadgeRepository
{
    UserBadge Create(UserBadge userBadge);
    UserBadge? Get(long id);
    List<UserBadge> GetByUserId(long userId);
    bool HasBadge(long userId, long badgeId);
    void Delete(long id);
}
