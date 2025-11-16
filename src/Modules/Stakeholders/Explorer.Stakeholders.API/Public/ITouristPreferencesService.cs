using Explorer.Stakeholders.API.Dtos;

namespace Explorer.Stakeholders.API.Public;

public interface ITouristPreferencesService
{
    TouristPreferencesDto Get(long userId);
    TouristPreferencesDto Create(TouristPreferencesDto dto);
    TouristPreferencesDto Update(TouristPreferencesDto dto);
    void Delete(long userId);
}