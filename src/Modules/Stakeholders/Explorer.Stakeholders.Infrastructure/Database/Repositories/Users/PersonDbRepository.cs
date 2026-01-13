using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces.Users;
using Explorer.Stakeholders.Core.Domain.Users.Entities;
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
}