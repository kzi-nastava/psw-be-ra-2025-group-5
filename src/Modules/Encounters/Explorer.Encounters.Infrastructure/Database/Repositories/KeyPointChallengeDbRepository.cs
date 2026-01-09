using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.Encounters.Core.Domain;
using Explorer.Encounters.Core.Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Explorer.Encounters.Infrastructure.Database.Repositories
{
    public class KeyPointChallengeDbRepository : IKeyPointChallengeRepository
    {
        private readonly EncountersContext _dbContext;
        private readonly DbSet<KeyPointChallenge> _dbSet;

        public KeyPointChallengeDbRepository(EncountersContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<KeyPointChallenge>();
        }

        public KeyPointChallenge Create(KeyPointChallenge entity)
        {
            _dbSet.Add(entity);
            _dbContext.SaveChanges();
            return entity;
        }
    }
}
