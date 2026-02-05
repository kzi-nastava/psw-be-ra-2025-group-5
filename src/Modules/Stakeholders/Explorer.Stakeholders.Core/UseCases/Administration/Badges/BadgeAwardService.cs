using Explorer.Stakeholders.API.Public.Badges;
using Explorer.Stakeholders.Core.Domain.Badges;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces.Users;

namespace Explorer.Stakeholders.Core.UseCases.Administration.Badges;

public class BadgeAwardService : IBadgeAwardService
{
    private readonly IBadgeService _badgeService;
    private readonly IUserBadgeService _userBadgeService;
    private readonly IUserStatisticsService _userStatisticsService;
    private readonly IPersonRepository _personRepository;

    public BadgeAwardService(
        IBadgeService badgeService,
        IUserBadgeService userBadgeService,
        IUserStatisticsService userStatisticsService,
        IPersonRepository personRepository)
    {
        _badgeService = badgeService;
        _userBadgeService = userBadgeService;
        _userStatisticsService = userStatisticsService;
        _personRepository = personRepository;
    }

    // TOUR COMPLETED
    public void OnTourCompleted(long userId)
    {
        try
        {
            _userStatisticsService.IncrementCompletedTours(userId);
            CheckAndAwardCompletedToursBadges(userId);
        }
        catch { }
    }

    // CHALLENGE COMPLETED
    public void OnChallengeCompleted(long userId, int challengeType)
    {
        try
        {
            _userStatisticsService.IncrementCompletedChallenges(userId);
            _userStatisticsService.MarkChallengeTypeCompleted(userId, challengeType);
            CheckAndAwardCompletedChallengesBadges(userId);
            CheckAndAwardAllChallengeTypesBadge(userId);
        }
        catch { }
    }

    // TOUR PUBLISHED
    public void OnTourPublished(long authorId)
    {
        try
        {
            _userStatisticsService.IncrementPublishedTours(authorId);
            CheckAndAwardPublishedToursBadges(authorId);
        }
        catch { }
    }

    // TOUR SOLD
    public void OnTourSold(long authorId)
    {
        try
        {
            _userStatisticsService.IncrementSoldTours(authorId);
            CheckAndAwardSoldToursBadges(authorId);
        }
        catch { }
    }

    // BLOG PUBLISHED
    public void OnBlogPublished(long authorId)
    {
        try
        {
            _userStatisticsService.IncrementBlogPosts(authorId);
            CheckAndAwardBlogPostsBadges(authorId);
        }
        catch { }
    }

    // CLUB JOINED
    public void OnClubJoined(long userId)
    {
        try
        {
            _userStatisticsService.SetJoinedClub(userId, true);
            CheckAndAwardClubMemberBadge(userId);
        }
        catch { }
    }

    // LEVEL UP
    public void OnLevelUp(long userId, int newLevel)
    {
        try
        {
            _userStatisticsService.UpdateLevel(userId, newLevel);
            CheckAndAwardLevelBadges(userId);
        }
        catch { }
    }

    // Privatne metode za proveru
    private void CheckAndAwardLevelBadges(long userId)
    {
        var stats = _userStatisticsService.GetByUserId(userId);
        var badges = _badgeService.GetByType((int)BadgeType.Level);

        foreach (var badge in badges)
        {
            if (stats.Level >= badge.RequiredValue && !_userBadgeService.HasBadge(userId, badge.Id))
            {
                _userBadgeService.AwardBadge(userId, badge.Id);
            }
        }
    }

    private void CheckAndAwardCompletedToursBadges(long userId)
    {
        var stats = _userStatisticsService.GetByUserId(userId);
        var badges = _badgeService.GetByType((int)BadgeType.CompletedTours);

        foreach (var badge in badges)
        {
            if (stats.CompletedToursCount >= badge.RequiredValue && !_userBadgeService.HasBadge(userId, badge.Id))
            {
                _userBadgeService.AwardBadge(userId, badge.Id);
            }
        }
    }

    private void CheckAndAwardCompletedChallengesBadges(long userId)
    {
        var stats = _userStatisticsService.GetByUserId(userId);
        var badges = _badgeService.GetByType((int)BadgeType.CompletedChallenges);

        foreach (var badge in badges)
        {
            if (stats.CompletedChallengesCount >= badge.RequiredValue && !_userBadgeService.HasBadge(userId, badge.Id))
            {
                _userBadgeService.AwardBadge(userId, badge.Id);
            }
        }
    }

    private void CheckAndAwardPublishedToursBadges(long userId)
    {
        var stats = _userStatisticsService.GetByUserId(userId);
        var badges = _badgeService.GetByType((int)BadgeType.PublishedTours);

        foreach (var badge in badges)
        {
            if (stats.PublishedToursCount >= badge.RequiredValue && !_userBadgeService.HasBadge(userId, badge.Id))
            {
                _userBadgeService.AwardBadge(userId, badge.Id);
            }
        }
    }

    private void CheckAndAwardSoldToursBadges(long userId)
    {
        var stats = _userStatisticsService.GetByUserId(userId);
        var badges = _badgeService.GetByType((int)BadgeType.SoldTours);

        foreach (var badge in badges)
        {
            if (stats.SoldToursCount >= badge.RequiredValue && !_userBadgeService.HasBadge(userId, badge.Id))
            {
                _userBadgeService.AwardBadge(userId, badge.Id);
            }
        }
    }

    private void CheckAndAwardBlogPostsBadges(long userId)
    {
        var stats = _userStatisticsService.GetByUserId(userId);
        var badges = _badgeService.GetByType((int)BadgeType.BlogPosts);

        foreach (var badge in badges)
        {
            if (stats.BlogPostsCount >= badge.RequiredValue && !_userBadgeService.HasBadge(userId, badge.Id))
            {
                _userBadgeService.AwardBadge(userId, badge.Id);
            }
        }
    }

    private void CheckAndAwardClubMemberBadge(long userId)
    {
        var stats = _userStatisticsService.GetByUserId(userId);
        var badges = _badgeService.GetByType((int)BadgeType.ClubMember);

        foreach (var badge in badges)
        {
            if (stats.JoinedClub && !_userBadgeService.HasBadge(userId, badge.Id))
            {
                _userBadgeService.AwardBadge(userId, badge.Id);
            }
        }
    }

    private void CheckAndAwardAllChallengeTypesBadge(long userId)
    {
        var stats = _userStatisticsService.GetByUserId(userId);
        var badges = _badgeService.GetByType((int)BadgeType.AllChallengeTypesCompleted);

        int allTypesMask = 0b111;
        
        foreach (var badge in badges)
        {
            if ((stats.ChallengeTypesCompletedMask & allTypesMask) == allTypesMask && !_userBadgeService.HasBadge(userId, badge.Id))
            {
                _userBadgeService.AwardBadge(userId, badge.Id);
            }
        }
    }
}

