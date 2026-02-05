using Explorer.Stakeholders.API.Dtos.Badges;

namespace Explorer.Stakeholders.API.Public.Badges;

public interface IUserStatisticsService
{
    UserStatisticsDto Create(long userId);
    UserStatisticsDto Get(long id);
    UserStatisticsDto GetByUserId(long userId);
    UserStatisticsDto Update(UserStatisticsDto dto);
    void Delete(long id);
    UserStatisticsDto UpdateLevel(long userId, int level);
    UserStatisticsDto UpdateAccountAgeDays(long userId, int days);
    UserStatisticsDto IncrementCompletedTours(long userId);
    UserStatisticsDto IncrementCompletedChallenges(long userId);
    UserStatisticsDto MarkChallengeTypeCompleted(long userId, int challengeType);
    UserStatisticsDto IncrementPublishedTours(long userId);
    UserStatisticsDto IncrementSoldTours(long userId);
    UserStatisticsDto IncrementBlogPosts(long userId);
    UserStatisticsDto SetJoinedClub(long userId, bool joined);
}
