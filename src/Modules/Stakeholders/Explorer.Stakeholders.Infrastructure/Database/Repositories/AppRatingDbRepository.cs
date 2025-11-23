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
           
            _dbContext.Entry(rating).State = EntityState.Modified;
            _dbContext.SaveChanges();
            return rating;
        }

        public void Delete(long id)
        {
            var entity = _dbSet.Find(id);

            if (entity == null)
                throw new KeyNotFoundException($"AppRating sa Id={id} ne postoji.");

            _dbContext.Entry(entity).State = EntityState.Deleted;

            _dbContext.SaveChanges();
        }

        public IEnumerable<AppRating> GetAll()
        {
            return _dbSet.ToList();
        }
        public AppRating Get(long id)
        {
            return _dbSet.FirstOrDefault(x => x.Id == id);
        }
    }
}
