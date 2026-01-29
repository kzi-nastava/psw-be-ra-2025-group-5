using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.Core.Domain.Badges;

namespace Explorer.Stakeholders.Core.Domain.RepositoryInterfaces.Badges;

public interface IBadgeRepository
{
    Badge Create(Badge badge);
    Badge? Get(long id);
    PagedResult<Badge> GetPaged(int page, int pageSize);
    List<Badge> GetAll();
    Badge Update(Badge badge);
    void Delete(long id);
    List<Badge> GetByType(BadgeType type);
    List<Badge> GetByRole(BadgeRole role);
    List<Badge> GetByRank(BadgeRank rank);
    List<Badge> GetByName(string name);
}

