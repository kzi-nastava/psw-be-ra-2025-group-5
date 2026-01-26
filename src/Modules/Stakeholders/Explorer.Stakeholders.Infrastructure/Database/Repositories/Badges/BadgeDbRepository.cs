using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Stakeholders.Core.Domain.Badges;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces.Badges;
using Explorer.Stakeholders.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Explorer.Stakeholders.Infrastructure.Database.Repositories.Badges;

public class BadgeDbRepository : IBadgeRepository
{
    private readonly StakeholdersContext _dbContext;
    private readonly DbSet<Badge> _dbSet;

    public BadgeDbRepository(StakeholdersContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<Badge>();
    }

    public Badge Create(Badge badge)
    {
        _dbSet.Add(badge);
        _dbContext.SaveChanges();
        return badge;
    }

    public Badge? Get(long id)
    {
        return _dbSet.FirstOrDefault(b => b.Id == id);
    }

    public PagedResult<Badge> GetPaged(int page, int pageSize)
    {
        var task = _dbSet.GetPagedById(page, pageSize);
        task.Wait();
        return task.Result;
    }

    public List<Badge> GetAll()
    {
        return _dbSet.ToList();
    }

    public Badge Update(Badge badge)
    {
        _dbSet.Update(badge);
        _dbContext.SaveChanges();
        return badge;
    }

    public void Delete(long id)
    {
        var badge = Get(id);
        if (badge != null)
        {
            _dbSet.Remove(badge);
            _dbContext.SaveChanges();
        }
    }

    public List<Badge> GetByType(BadgeType type)
    {
        return _dbSet.Where(b => b.Type == type).ToList();
    }

    public List<Badge> GetByRole(BadgeRole role)
    {
        return _dbSet.Where(b => b.Role == role || b.Role == BadgeRole.Both).ToList();
    }

    public List<Badge> GetByRank(BadgeRank rank)
    {
        return _dbSet.Where(b => b.Rank == rank).ToList();
    }

    public List<Badge> GetByName(string name)
    {
        return _dbSet.Where(b => b.Name.ToLower().Contains(name.ToLower())).ToList();
    }
}

