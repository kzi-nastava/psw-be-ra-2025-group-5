using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces.Streaks;
using Explorer.Stakeholders.Core.Domain.Streaks;
using Microsoft.EntityFrameworkCore;

namespace Explorer.Stakeholders.Infrastructure.Database.Repositories.Streaks
{
    public class StreakDbRepository : IStreakRepository
    {
        private readonly StakeholdersContext _dbContext;
        private readonly DbSet<Streak> _dbSet;

        public StreakDbRepository(StakeholdersContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<Streak>();
        }

        public Streak? GetByUserId(long userId)
        {
            return _dbContext.Streaks.FirstOrDefault(s => s.UserId == userId);
        }

        public void Add(Streak streak)
        {
            _dbSet.Add(streak);
        }

        public void Update(Streak streak)
        {
            _dbSet.Update(streak);
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }

        public bool Exists(long userId)
        {
            return _dbSet.Any(s => s.UserId == userId);
        }
    }
}
