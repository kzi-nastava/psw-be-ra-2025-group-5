using Explorer.BuildingBlocks.Core.Domain;

namespace Explorer.Stakeholders.Core.Domain.Badges;

public class UserBadge : Entity
{
    public long UserId { get; private set; }
    public long BadgeId { get; private set; }
    public DateTime EarnedAt { get; private set; }

    private UserBadge() { }

    public UserBadge(long userId, long badgeId)
    {
        UserId = userId;
        BadgeId = badgeId;
        EarnedAt = DateTime.UtcNow;
    }
}
