using Explorer.Stakeholders.API.Dtos;

namespace Explorer.Stakeholders.API.Public;

public interface IPositionService
{
    PositionDto? GetByTourist(long id);
    IEnumerable<PositionDto> GetAll();
    PositionDto Create(PositionDto entity);
    PositionDto Update(PositionDto entity);
}
