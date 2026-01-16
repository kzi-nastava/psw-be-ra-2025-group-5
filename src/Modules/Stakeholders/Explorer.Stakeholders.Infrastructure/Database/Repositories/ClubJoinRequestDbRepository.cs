using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using Explorer.Stakeholders.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Infrastructure.Database.Repositories
{
    public class ClubJoinRequestDbRepository : IClubJoinRequestRepository
    {
        private readonly StakeholdersContext _dbContext;

        public ClubJoinRequestDbRepository(StakeholdersContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ClubJoinRequest Create(ClubJoinRequest request)
        {
            _dbContext.Add(request);
            _dbContext.SaveChanges();
            return request;
        }

        public void Delete(ClubJoinRequest request)
        {
            _dbContext.Remove(request);
            _dbContext.SaveChanges();
        }

        public ClubJoinRequest? GetById(long id)
            => _dbContext.Set<ClubJoinRequest>().Find(id);

        public ClubJoinRequest? GetByClubAndTourist(long clubId, long touristId)
            => _dbContext.Set<ClubJoinRequest>()
                .FirstOrDefault(r => r.ClubId == clubId && r.TouristId == touristId);

        public List<ClubJoinRequest> GetByClubId(long clubId)
            => _dbContext.Set<ClubJoinRequest>()
                .Where(r => r.ClubId == clubId)
                .ToList();

        public bool Exists(long clubId, long touristId)
            => _dbContext.Set<ClubJoinRequest>()
                .Any(r => r.ClubId == clubId && r.TouristId == touristId);
    }

}
