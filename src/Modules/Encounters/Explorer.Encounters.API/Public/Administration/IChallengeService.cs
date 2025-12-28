using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Encounters.API.Dtos;

namespace Explorer.Encounters.API.Public.Administration;

public interface IChallengeService
{
    PagedResult<ChallengeDto> GetPaged(int page, int pageSize);
    ChallengeDto Create(ChallengeDto challenge);
    ChallengeDto Update(ChallengeDto challenge);
    void Delete(long id);
    List<ChallengeDto> GetAllActive();
    ChallengeDto GetById(long challengeId);
}
