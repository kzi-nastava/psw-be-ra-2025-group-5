namespace Explorer.Encounters.Core.Domain.RepositoryInterfaces.Social
{
    public interface IChallengeParticipationRepository
    {
        void MarkActive(long challengeId, long touristId);
        void Remove(long challengeId, long touristId);
        List<long> GetActiveTourists(long challengeId);
        void Clear(long challengeId);
    }
}
