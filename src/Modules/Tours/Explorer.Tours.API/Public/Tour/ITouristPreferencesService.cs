using Explorer.Tours.API.Dtos.Preferences;

namespace Explorer.Tours.API.Public.Tour;

public interface ITouristPreferencesService
{
    TouristPreferencesDto Get(long userId);
    TouristPreferencesDto Create(TouristPreferencesDto dto);
    TouristPreferencesDto Update(TouristPreferencesDto dto);
    void Delete(long userId);
}
