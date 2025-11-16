using Explorer.Stakeholders.Core.Domain;

namespace Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;

public interface ITouristPreferencesRepository
{
    TouristPreferences Get(long userId);
    TouristPreferences Create(TouristPreferences entity);
    TouristPreferences Update(TouristPreferences entity);
    void Delete(long userId);
}