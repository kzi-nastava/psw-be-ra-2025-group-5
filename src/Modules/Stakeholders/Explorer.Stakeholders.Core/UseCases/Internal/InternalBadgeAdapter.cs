using Explorer.Stakeholders.API.Internal;
using Explorer.Stakeholders.API.Public.Badges;

namespace Explorer.Stakeholders.Core.UseCases.Internal;

public class InternalBadgeAdapter : IInternalBadgeService
{
    private readonly IBadgeAwardService _badgeAwardService;

    public InternalBadgeAdapter(IBadgeAwardService badgeAwardService)
    {
        _badgeAwardService = badgeAwardService;
    }

    public void OnTourCompleted(long userId)
    {
        _badgeAwardService.OnTourCompleted(userId);
    }

    public void OnChallengeCompleted(long userId, int challengeType)
    {
        _badgeAwardService.OnChallengeCompleted(userId, challengeType);
    }

    public void OnTourPublished(long authorId)
    {
        _badgeAwardService.OnTourPublished(authorId);
    }

    public void OnTourSold(long authorId)
    {
        _badgeAwardService.OnTourSold(authorId);
    }

    public void OnBlogPublished(long authorId)
    {
        _badgeAwardService.OnBlogPublished(authorId);
    }

    public void OnClubJoined(long userId)
    {
        _badgeAwardService.OnClubJoined(userId);
    }

    public void OnLevelUp(long userId, int newLevel)
    {
        _badgeAwardService.OnLevelUp(userId, newLevel);
    }
}

