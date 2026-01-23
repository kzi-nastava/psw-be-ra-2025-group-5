using Explorer.BuildingBlocks.Core.UseCases;

namespace Explorer.Encounters.Core.Domain.RepositoryInterfaces;

public interface IChallengeExecutionRepository
{
    ChallengeExecution Create(ChallengeExecution execution);
    ChallengeExecution Update(ChallengeExecution execution);
    ChallengeExecution Get(long id);
    List<ChallengeExecution> GetByTourist(long touristId);
    ChallengeExecution? GetActiveByChallengeAndTourist(long challengeId, long touristId);
    ChallengeExecution? GetCompletedByChallengeAndTourist(long challengeId, long touristId);
}
