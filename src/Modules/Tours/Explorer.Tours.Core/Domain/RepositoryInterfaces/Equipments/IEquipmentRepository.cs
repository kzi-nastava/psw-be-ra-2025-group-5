using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.Core.Domain.Equipments;

namespace Explorer.Tours.Core.Domain.RepositoryInterfaces.Equipments;

public interface IEquipmentRepository
{
    PagedResult<Equipment> GetPaged(int page, int pageSize);
    Equipment Create(Equipment map);
    Equipment Update(Equipment map);
    void Delete(long id);
    List<Equipment> GetByIds(List<long> ids);
}