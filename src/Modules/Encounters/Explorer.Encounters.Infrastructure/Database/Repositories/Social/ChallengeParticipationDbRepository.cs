using Explorer.Encounters.Core.Domain.RepositoryInterfaces.Social;
using Explorer.Encounters.Core.Domain.Social;
using Microsoft.EntityFrameworkCore;

namespace Explorer.Encounters.Infrastructure.Database.Repositories.Social
{
    public class ChallengeParticipationDbRepository : IChallengeParticipationRepository
    {
        protected readonly EncountersContext DbContext;
        private readonly DbSet<ChallengeParticipation> _dbSet;

        public ChallengeParticipationDbRepository(EncountersContext dbContext)
        {
            DbContext = dbContext;
            _dbSet = DbContext.Set<ChallengeParticipation>();
        }

        public void MarkActive(long challengeId, long touristId)
        {
            var participation = _dbSet
                .FirstOrDefault(cp => cp.ChallengeId == challengeId && cp.TouristId == touristId);

            if (participation == null)
            {
                _dbSet.Add(new ChallengeParticipation(challengeId, touristId));
            }
            else
            {
                participation.RefreshLastSeen();
            }

            DbContext.SaveChanges();
        }


        public void Remove(long challengeId, long touristId)
        {
            var participation = _dbSet
                .FirstOrDefault(p => p.ChallengeId == challengeId && p.TouristId == touristId);

            if (participation == null) return;

            _dbSet.Remove(participation);
            DbContext.SaveChanges();
        }

        public List<long> GetActiveTourists(long challengeId)
        {
            // Korišćeno za testiranje sa kraćim intervalom
            // var cutoff = DateTime.UtcNow.AddSeconds(-20);

            return _dbSet
                .Where(p => p.ChallengeId == challengeId /* && p.LastSeenAt >= cutoff */)
                .Select(p => p.TouristId)
                .ToList();
        }

        public void Clear(long challengeId)
        {
            var participations = _dbSet
                .Where(p => p.ChallengeId == challengeId)
                .ToList();

            if (participations.Count == 0) return;

            _dbSet.RemoveRange(participations);
            DbContext.SaveChanges();
        }
    }
}
