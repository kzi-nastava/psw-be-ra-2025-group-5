using Explorer.BuildingBlocks.Core.Domain;
using Explorer.BuildingBlocks.Core.Exceptions;
using Explorer.Payments.Core.Domain;
using Explorer.Payments.Core.Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payments.Infrastructure.Database.Repositories
{
    public class CouponDbRepository: ICouponRepository
    {
        protected readonly PaymentsContext DbContext;
        private readonly DbSet<Coupon> _dbSet;

        public CouponDbRepository(PaymentsContext dbContext)
        {
            DbContext = dbContext;
            _dbSet = DbContext.Set<Coupon>();
        }

        public Coupon Get(long id)
        {
            Coupon entity = _dbSet.Find(id);
            if (entity == null) throw new NotFoundException("Not found: " + id);
            return entity;
        }

        public Coupon Create(Coupon entity)
        {
            DbContext.Add(entity);
            DbContext.SaveChanges();
            return entity;
        }

        public Coupon Update(Coupon entity)
        {
            try
            {
                DbContext.Update(entity);
                DbContext.SaveChanges();
            }
            catch (DbUpdateException e)
            {
                throw new NotFoundException(e.Message);
            }
            return entity;
        }

        public void Delete(long id)
        {
            var entity = Get(id);
            _dbSet.Remove(entity);
            DbContext.SaveChanges();
        }

        
        public Coupon GetByCode(string code)
        {
            var entity = _dbSet.FirstOrDefault(x => x.Code == code);
            return entity;
        }

        public List<Coupon> GetByAuthor(long authorId)
        {
            return _dbSet.Where(c => c.AuthorId == authorId).ToList();
        }
    }
}
