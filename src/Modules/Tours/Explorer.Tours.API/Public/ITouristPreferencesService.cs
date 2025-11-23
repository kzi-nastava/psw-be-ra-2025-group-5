using Explorer.Tours.API.Dtos;

namespace Explorer.Tours.API.Public;

public interface ITouristPreferencesService
{
    TouristPreferencesDto Get(long userId);
    TouristPreferencesDto Create(TouristPreferencesDto dto);
    TouristPreferencesDto Update(TouristPreferencesDto dto);
    void Delete(long userId);
}
