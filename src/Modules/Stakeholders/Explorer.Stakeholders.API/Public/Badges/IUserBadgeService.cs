using Explorer.Stakeholders.API.Dtos.Badges;

namespace Explorer.Stakeholders.API.Public.Badges;

public interface IUserBadgeService
{
    UserBadgeDto AwardBadge(long userId, long badgeId);
    UserBadgeDto Get(long id);
    List<UserBadgeDto> GetByUserId(long userId);
    List<UserBadgeDto> GetBestBadgesByUserId(long userId);
    List<BadgeDto> GetNotOwnedByUser(long userId);
    bool HasBadge(long userId, long badgeId);
}
