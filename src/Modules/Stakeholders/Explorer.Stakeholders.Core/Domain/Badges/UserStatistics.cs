using Explorer.BuildingBlocks.Core.Domain;

namespace Explorer.Stakeholders.Core.Domain.Badges;

public class UserStatistics : Entity
{
    public long UserId { get; private set; }
    public int Level { get; private set; }
    public int AccountAgeDays { get; private set; }
    public int CompletedToursCount { get; private set; }
    public int CompletedChallengesCount { get; private set; }
    public int PublishedToursCount { get; private set; }
    public int SoldToursCount { get; private set; }
    public int BlogPostsCount { get; private set; }
    public int ChallengeTypesCompletedMask { get; private set; }
    public bool JoinedClub { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private UserStatistics() 
    {
        ChallengeTypesCompletedMask = 0;
    }

    public UserStatistics(long userId)
    {
        UserId = userId;
        Level = 0;
        AccountAgeDays = 0;
        CompletedToursCount = 0;
        CompletedChallengesCount = 0;
        PublishedToursCount = 0;
        SoldToursCount = 0;
        BlogPostsCount = 0;
        ChallengeTypesCompletedMask = 0;
        JoinedClub = false;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateLevel(int level)
    {
        if (level < 0) throw new ArgumentException("Level cannot be negative");
        Level = level;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateAccountAgeDays(int days)
    {
        if (days < 0) throw new ArgumentException("AccountAgeDays cannot be negative");
        AccountAgeDays = days;
        UpdatedAt = DateTime.UtcNow;
    }

    public void IncrementCompletedTours()
    {
        CompletedToursCount++;
        UpdatedAt = DateTime.UtcNow;
    }

    public void IncrementCompletedChallenges()
    {
        CompletedChallengesCount++;
        UpdatedAt = DateTime.UtcNow;
    }

    public void IncrementPublishedTours()
    {
        PublishedToursCount++;
        UpdatedAt = DateTime.UtcNow;
    }

    public void IncrementSoldTours()
    {
        SoldToursCount++;
        UpdatedAt = DateTime.UtcNow;
    }

    public void IncrementBlogPosts()
    {
        BlogPostsCount++;
        UpdatedAt = DateTime.UtcNow;
    }

    public void MarkChallengeTypeCompleted(int challengeType)
    {
        // challengeType: Social=0, Location=1, Misc=2
        // Postavi bit na poziciji challengeType
        int bitMask = 1 << challengeType;
        ChallengeTypesCompletedMask |= bitMask;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateChallengeTypesCompletedMask(int mask)
    {
        ChallengeTypesCompletedMask = mask;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetJoinedClub(bool joined)
    {
        JoinedClub = joined;
        UpdatedAt = DateTime.UtcNow;
    }
}
