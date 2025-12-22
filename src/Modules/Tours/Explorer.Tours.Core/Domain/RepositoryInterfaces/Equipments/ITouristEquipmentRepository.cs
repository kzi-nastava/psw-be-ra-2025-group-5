using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.Core.Domain.Equipments.Entities;

namespace Explorer.Tours.Core.Domain.RepositoryInterfaces.Equipments
{
    public interface ITouristEquipmentRepository
    {
        PagedResult<TouristEquipment> GetPagedByTouristId(long touristId, int page, int pageSize);
        TouristEquipment Get(long id);

        TouristEquipment Create(TouristEquipment map);
        TouristEquipment Update(TouristEquipment map);
        void Delete(long id);
    }
}
