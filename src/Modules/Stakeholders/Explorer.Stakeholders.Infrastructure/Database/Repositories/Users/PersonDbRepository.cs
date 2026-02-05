using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces.Users;
using Explorer.Stakeholders.Core.Domain.Social;
using Explorer.Stakeholders.Core.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Explorer.Stakeholders.Infrastructure.Database.Repositories.Users;

public class PersonDbRepository : IPersonRepository
{
    protected readonly StakeholdersContext DbContext;
    private readonly DbSet<Person> _dbSet;

    public PersonDbRepository(StakeholdersContext dbContext)
    {
        DbContext = dbContext;
        _dbSet = DbContext.Set<Person>();
    }

    public Person Create(Person entity)
    {
        _dbSet.Add(entity);
        DbContext.SaveChanges();
        return entity;
    }

    public Person? GetByUserId(long userId)
    {
        return _dbSet.FirstOrDefault(p => p.UserId == userId);
    }

    public Person Update(Person entity)
    {
        _dbSet.Update(entity);
        DbContext.SaveChanges();
        return entity;
    }

    public Person? Get(long id)
    {
        return _dbSet.FirstOrDefault(p => p.Id == id);
    }

    public PagedResult<Person> GetPaged(int page, int pageSize)
    {
        var task = _dbSet.GetPagedById(page, pageSize);
        task.Wait();
        return task.Result;
    }

    public List<Person> GetAll()
    {
        return _dbSet.ToList();
    }

    public PagedResult<Person> GetFollowingPaged(long userId, int page, int pageSize)
    {
        var person = _dbSet.FirstOrDefault(p => p.UserId == userId)
                     ?? throw new KeyNotFoundException("Person not found for this user.");

        var followsQuery = DbContext.Set<ProfileFollow>()
            .Include(f => f.Following)
            .Where(f => f.FollowerId == person.Id);

        var totalCount = followsQuery.Count();

        var results = followsQuery
            .OrderBy(f => f.Following.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(f => f.Following)
            .ToList();

        return new PagedResult<Person>(results, totalCount);
    }
}
