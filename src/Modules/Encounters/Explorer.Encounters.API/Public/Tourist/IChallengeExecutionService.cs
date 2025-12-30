using Explorer.Encounters.API.Dtos;

namespace Explorer.Encounters.API.Public.Tourist;

public interface IChallengeExecutionService
{
    ChallengeExecutionDto StartChallenge(long challengeId, long touristId);
    ChallengeExecutionDto CompleteChallenge(long executionId, long touristId);
    ChallengeExecutionDto AbandonChallenge(long executionId, long touristId);
    ChallengeExecutionDto GetById(long id);
    List<ChallengeExecutionDto> GetByTourist(long touristId);
    void UpdateTouristLocation(long challengeId, long touristId, double latitude, double longitude);
}


