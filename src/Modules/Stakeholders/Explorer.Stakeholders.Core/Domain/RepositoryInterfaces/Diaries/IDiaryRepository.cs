using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.Core.Domain.Diaries;

namespace Explorer.Stakeholders.Core.Domain.RepositoryInterfaces.Diaries;

public interface IDiaryRepository
{
    PagedResult<Diary> GetByTourist(long touristId, int page, int pageSize);
    Diary? GetById(long id);
    Diary Create(Diary diary);
    Diary Update(Diary diary);
    void Delete(long id);
}
