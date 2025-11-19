using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;

namespace Explorer.Tours.API.Public.Administration
{
    public interface ITouristEquipmentService
    {
        PagedResult<TouristEquipmentDto> GetPaged(long touristId, int page, int pageSize);
        TouristEquipmentDto Create(TouristEquipmentDto touristEquipment);
        TouristEquipmentDto Update(TouristEquipmentDto touristEquipment, long touristId);
        void Delete(long id, long touristId);
    }
}