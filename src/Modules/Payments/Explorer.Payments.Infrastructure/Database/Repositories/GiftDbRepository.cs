using Explorer.Payments.Core.Domain.RepositoryInterfaces;
using Explorer.Payments.Core.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payments.Infrastructure.Database.Repositories
{
    public class GiftDbRepository : IGiftRepository
    {
        protected readonly PaymentsContext DbContext;
        private readonly DbSet<Gift> _dbSet;

        public GiftDbRepository(PaymentsContext dbContext)
        {
            DbContext = dbContext;
            _dbSet = DbContext.Set<Gift>();
        }

        public Gift Create(Gift gift)
        {
            _dbSet.Add(gift);
            DbContext.SaveChanges();
            return gift;
        }
        public Gift GetById(long id)
        {
            return _dbSet.Find(id);
        }

        public List<Gift> GetByDonor(long donorId) => _dbSet.Where(g => g.DonorId == donorId).ToList();
        public List<Gift> GetByRecipient(long recipientId) => _dbSet.Where(g => g.RecipientId == recipientId).ToList();
    }
}
