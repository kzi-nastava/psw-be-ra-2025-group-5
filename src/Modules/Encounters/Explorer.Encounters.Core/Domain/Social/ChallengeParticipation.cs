using Explorer.BuildingBlocks.Core.Domain;

namespace Explorer.Encounters.Core.Domain.Social
{
    public class ChallengeParticipation : Entity
    {
        public long ChallengeId { get; private set; }
        public long TouristId { get; private set; }
        public DateTime LastSeenAt { get; private set; }

        public ChallengeParticipation(long challengeId, long touristId)
        {
            ChallengeId = challengeId;
            TouristId = touristId;
            LastSeenAt = DateTime.UtcNow;
        }

        public void RefreshLastSeen()
        {
            LastSeenAt = DateTime.UtcNow;
        }
    }
}
