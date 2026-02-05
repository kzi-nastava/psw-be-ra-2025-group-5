using Explorer.Stakeholders.Core.Domain.Badges;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces.Badges;
using Explorer.Stakeholders.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Explorer.Stakeholders.Infrastructure.Database.Repositories.Badges;

public class UserBadgeDbRepository : IUserBadgeRepository
{
    private readonly StakeholdersContext _dbContext;
    private readonly DbSet<UserBadge> _dbSet;

    public UserBadgeDbRepository(StakeholdersContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<UserBadge>();
    }

    public UserBadge Create(UserBadge userBadge)
    {
        _dbSet.Add(userBadge);
        _dbContext.SaveChanges();
        return userBadge;
    }

    public UserBadge? Get(long id)
    {
        return _dbSet.FirstOrDefault(ub => ub.Id == id);
    }

    public List<UserBadge> GetByUserId(long userId)
    {
        return _dbSet.Where(ub => ub.UserId == userId).ToList();
    }

    public bool HasBadge(long userId, long badgeId)
    {
        return _dbSet.Any(ub => ub.UserId == userId && ub.BadgeId == badgeId);
    }

    public void Delete(long id)
    {
        var userBadge = Get(id);
        if (userBadge != null)
        {
            _dbSet.Remove(userBadge);
            _dbContext.SaveChanges();
        }
    }
}
