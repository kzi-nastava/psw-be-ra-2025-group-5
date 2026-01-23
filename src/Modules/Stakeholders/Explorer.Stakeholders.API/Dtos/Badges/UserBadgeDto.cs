namespace Explorer.Stakeholders.API.Dtos.Badges;

public class UserBadgeDto
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public long BadgeId { get; set; }
    public DateTime EarnedAt { get; set; }
    public BadgeDto? Badge { get; set; }
}
