using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Infrastructure.Database.Repositories
{
    public class ClubInviteDbRepository : IClubInviteRepository
    {
        private readonly StakeholdersContext _dbContext;

        public ClubInviteDbRepository(StakeholdersContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ClubInvite Create(ClubInvite invite)
        {
            _dbContext.Add(invite);
            _dbContext.SaveChanges();
            return invite;
        }

        public void Delete(ClubInvite invite)
        {
            _dbContext.Remove(invite);
            _dbContext.SaveChanges();
        }

        public ClubInvite? GetById(long id)
        {
            return _dbContext.Set<ClubInvite>().Find(id);
        }

        public List<ClubInvite> GetByClubId(long clubId)
        {
            return _dbContext.Set<ClubInvite>().Where(i => i.ClubId == clubId).ToList();
        }

        public List<ClubInvite> GetByTouristId(long touristId)
        {
            return _dbContext.Set<ClubInvite>().Where(i => i.TouristId == touristId).ToList();
        }

        public bool Exists(long clubId, long touristId)
        {
            return _dbContext.ClubInvites
                .Any(ci => ci.ClubId == clubId && ci.TouristId == touristId);
        }
    }
}
