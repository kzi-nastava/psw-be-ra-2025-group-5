namespace Explorer.Stakeholders.API.Internal;

public interface IInternalBadgeService
{
    void OnTourCompleted(long userId);
    void OnChallengeCompleted(long userId, int challengeType);
    void OnTourPublished(long authorId);
    void OnTourSold(long authorId);
    void OnBlogPublished(long authorId);
    void OnClubJoined(long userId);
    void OnLevelUp(long userId, int newLevel);
}

