using Explorer.Stakeholders.Core.Domain.Badges;

namespace Explorer.Stakeholders.Core.Domain.RepositoryInterfaces.Badges;

public interface IUserStatisticsRepository
{
    UserStatistics Create(UserStatistics userStatistics);
    UserStatistics? Get(long id);
    UserStatistics? GetByUserId(long userId);
    UserStatistics Update(UserStatistics userStatistics);
    void Delete(long id);
}
