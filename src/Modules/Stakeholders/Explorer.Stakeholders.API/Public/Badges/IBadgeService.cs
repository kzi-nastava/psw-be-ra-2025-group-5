using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos.Badges;

namespace Explorer.Stakeholders.API.Public.Badges;

public interface IBadgeService
{
    BadgeDto Get(long id);
    PagedResult<BadgeDto> GetPaged(int page, int pageSize);
    List<BadgeDto> GetAll();
    List<BadgeDto> GetByType(int type);
    List<BadgeDto> GetByRole(int role);
    List<BadgeDto> GetByRank(int rank);
    List<BadgeDto> GetByName(string name);
}


