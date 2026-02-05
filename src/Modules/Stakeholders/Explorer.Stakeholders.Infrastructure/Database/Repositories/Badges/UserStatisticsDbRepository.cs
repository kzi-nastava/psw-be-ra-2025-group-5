using Explorer.Stakeholders.Core.Domain.Badges;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces.Badges;
using Explorer.Stakeholders.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Explorer.Stakeholders.Infrastructure.Database.Repositories.Badges;

public class UserStatisticsDbRepository : IUserStatisticsRepository
{
    private readonly StakeholdersContext _dbContext;
    private readonly DbSet<UserStatistics> _dbSet;

    public UserStatisticsDbRepository(StakeholdersContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<UserStatistics>();
    }

    public UserStatistics Create(UserStatistics userStatistics)
    {
        _dbSet.Add(userStatistics);
        _dbContext.SaveChanges();
        return userStatistics;
    }

    public UserStatistics? Get(long id)
    {
        return _dbSet.FirstOrDefault(us => us.Id == id);
    }

    public UserStatistics? GetByUserId(long userId)
    {
        return _dbSet.FirstOrDefault(us => us.UserId == userId);
    }

    public UserStatistics Update(UserStatistics userStatistics)
    {
        _dbSet.Update(userStatistics);
        _dbContext.SaveChanges();
        return userStatistics;
    }

    public void Delete(long id)
    {
        var userStatistics = Get(id);
        if (userStatistics != null)
        {
            _dbSet.Remove(userStatistics);
            _dbContext.SaveChanges();
        }
    }
}
