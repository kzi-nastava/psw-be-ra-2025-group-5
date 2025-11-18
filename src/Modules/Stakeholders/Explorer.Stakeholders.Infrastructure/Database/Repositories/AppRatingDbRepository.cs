using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Explorer.Stakeholders.Infrastructure.Database.Repositories
{
    public class AppRatingDbRepository : IAppRatingRepository
    {
        private readonly StakeholdersContext _dbContext;
        private readonly DbSet<AppRating> _dbSet;

        public AppRatingDbRepository(StakeholdersContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<AppRating>();
        }

        public AppRating Create(AppRating rating)
        {
            _dbSet.Add(rating);
            _dbContext.SaveChanges();
            return rating;
        }

        public AppRating Update(AppRating rating)
        {
            _dbSet.Update(rating);
            _dbContext.SaveChanges();
            return rating;
        }

        public void Delete(long id)
        {
            var entity = _dbSet.Find(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                _dbContext.SaveChanges();
            }
        }

        public IEnumerable<AppRating> GetAll()
        {
            return _dbSet.AsNoTracking().ToList();
        }
    }
}
