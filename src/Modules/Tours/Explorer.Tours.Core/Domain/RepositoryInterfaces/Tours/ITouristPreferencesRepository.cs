using Explorer.Tours.Core.Domain.Preferences;

namespace Explorer.Tours.Core.Domain.RepositoryInterfaces.Tours;

public interface ITouristPreferencesRepository
{
    TouristPreferences Get(long userId);
    TouristPreferences Create(TouristPreferences entity);
    TouristPreferences Update(TouristPreferences entity);
    void Delete(long userId);
}
