using Explorer.Stakeholders.API.Dtos;

namespace Explorer.Stakeholders.API.Public;

public interface ITouristLocationService
{
    TouristLocationDto? Get(long id);
    IEnumerable<TouristLocationDto> GetAll();
    TouristLocationDto Create(TouristLocationDto entity);
    TouristLocationDto Update(TouristLocationDto entity);
}
