using Explorer.BuildingBlocks.Core.UseCases;

namespace Explorer.Encounters.Core.Domain.RepositoryInterfaces;

public interface IChallengeRepository
{
    PagedResult<Challenge> GetPaged(int page, int pageSize);
    Challenge Create(Challenge challenge);
    Challenge Update(Challenge challenge);
    void Delete(long id);
}
