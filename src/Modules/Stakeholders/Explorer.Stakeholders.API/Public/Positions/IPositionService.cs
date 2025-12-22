using Explorer.Stakeholders.API.Dtos.Locations;

namespace Explorer.Stakeholders.API.Public.Positions;

public interface IPositionService
{
    PositionDto? GetByTourist(long id);
    IEnumerable<PositionDto> GetAll();
    PositionDto Create(PositionDto entity);
    PositionDto Update(PositionDto entity);
}
