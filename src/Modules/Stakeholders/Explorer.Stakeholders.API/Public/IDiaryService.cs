using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;

namespace Explorer.Stakeholders.API.Public;

public interface IDiaryService
{
    PagedResult<DiaryDto> GetByTourist(long touristId, int page, int pageSize);
    DiaryDto Create(DiaryDto diary);
    DiaryDto Update(DiaryDto diary);
    void Delete(long id);
}
