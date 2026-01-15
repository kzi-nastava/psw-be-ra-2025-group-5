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
    public class PaymentDbRepository : IPaymentRepository
    {
        protected readonly PaymentsContext DbContext;
        private readonly DbSet<Payment> _dbSet;

        public PaymentDbRepository(PaymentsContext dbContext)
        {
            DbContext = dbContext;
            _dbSet = DbContext.Set<Payment>();
        }

        public Payment Create(Payment payment)
        {
            _dbSet.Add(payment);
            DbContext.SaveChanges();
            return payment;
        }

        public List<Payment> GetByTourist(long touristId)
        {
            return _dbSet.Where(p => p.TouristId == touristId).ToList();
        }
    }
}
