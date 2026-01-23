namespace Explorer.Encounters.API.Internal;

public interface IInternalPersonExperienceService
{
    void AddExperience(long userId, int xpAmount);
}
