using Explorer.BuildingBlocks.Core.Exceptions;
using Explorer.Payments.Core.Domain;
using Explorer.Payments.Core.Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payments.Infrastructure.Database.Repositories
{
    public class WalletDbRepository : IWalletRepository
    {
        protected readonly PaymentsContext DbContext;
        private readonly DbSet<Wallet> _dbSet;

        public WalletDbRepository(PaymentsContext dbContext)
        {
            DbContext = dbContext;
            _dbSet = DbContext.Set<Wallet>();
        }

        public List<Wallet> GetAll()
        {
            return _dbSet.ToList();
        }

        public Wallet GetByUserId(long userId)
        {
            return _dbSet.FirstOrDefault(w => w.UserId == userId);
        }

        public Wallet Create(Wallet entity)
        {
            _dbSet.Add(entity);
            DbContext.SaveChanges();
            return entity;
        }

        public Wallet Update(Wallet entity)
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

        public Wallet GetByTouristId(long touristId)
        {
            return _dbSet.FirstOrDefault(w => w.UserId == touristId);
        }
    }
}
