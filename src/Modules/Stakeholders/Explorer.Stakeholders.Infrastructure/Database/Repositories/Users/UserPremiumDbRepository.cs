using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces.Users;
using Explorer.Stakeholders.Core.Domain.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Infrastructure.Database.Repositories.Users
{
    public class UserPremiumDbRepository: IUserPremiumRepository
    {
        private readonly StakeholdersContext _dbContext;
        private readonly DbSet<UserPremium> _dbSet;

        public UserPremiumDbRepository(StakeholdersContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<UserPremium>();
        }

        public UserPremium Get(int userId)
        {
            return _dbSet.FirstOrDefault(up => up.UserId == userId)
                   ?? throw new KeyNotFoundException("UserPremium not found.");
        }

        public UserPremium Add(UserPremium userPremium)
        {
            _dbSet.Add(userPremium);
            _dbContext.SaveChanges();
            return userPremium;
        }

        public UserPremium Update(UserPremium userPremium)
        {
            var existing = _dbSet.FirstOrDefault(up => up.UserId == userPremium.UserId)
                           ?? throw new KeyNotFoundException("UserPremium not found.");

            if (userPremium.ValidUntil != null)
            {
                existing.Extend(userPremium.ValidUntil.Value);
            }

            _dbContext.SaveChanges();
            return existing;
        }

        public void Delete(int userId)
        {
            var premium = _dbSet.FirstOrDefault(up => up.UserId == userId);
            if (premium == null) return;

            _dbSet.Remove(premium);
            _dbContext.SaveChanges();
        }
    }
}
