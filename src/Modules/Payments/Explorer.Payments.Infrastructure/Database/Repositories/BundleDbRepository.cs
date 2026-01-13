using Explorer.BuildingBlocks.Core.Exceptions;
using Explorer.Payments.Core.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.Payments.Core.Domain.RepositoryInterfaces;

namespace Explorer.Payments.Infrastructure.Database.Repositories
{
    public class BundleDbRepository : IBundleRepository
    {
        private readonly PaymentsContext _dbContext;
        private readonly DbSet<Bundle> _dbSet;

        public BundleDbRepository(PaymentsContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<Bundle>();
        }

        public Bundle Get(long id)
        {
            var entity = _dbSet.Include(b => b.BundleItems).FirstOrDefault(b => b.Id == id);
            if (entity == null) throw new KeyNotFoundException($"Bundle with id: {id} not found.");

            return entity;
        }

        public List<Bundle> GetByAuthor(long authorId)
        {
            return _dbSet.Include(b => b.BundleItems)
                         .Where(b => b.AuthorId == authorId)
                         .ToList();
        }

        public Bundle Create(Bundle bundle)
        {
            _dbSet.Add(bundle);
            _dbContext.SaveChanges();

            return bundle;
        }

        public Bundle Update(Bundle bundle)
        {
            _dbContext.Update(bundle);
            _dbContext.SaveChanges();

            return bundle;
         
        }

        public void Delete(long id)
        {
            var entity = Get(id);
            _dbSet.Remove(entity);
            _dbContext.SaveChanges();
        }
    }
}
