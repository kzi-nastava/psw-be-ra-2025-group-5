using Explorer.Encounters.API.Internal;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces.Users;
using Explorer.Stakeholders.API.Public.Badges;

namespace Explorer.Stakeholders.Core.UseCases.Internal;

public class PersonExperienceAdapter : IInternalPersonExperienceService
{
    private readonly IPersonRepository _personRepository;
    private readonly IBadgeAwardService _badgeService;

    public PersonExperienceAdapter(IPersonRepository personRepository, IBadgeAwardService badgeService)
    {
        _personRepository = personRepository;
        _badgeService = badgeService;
    }

    public void AddExperience(long userId, int xpAmount)
    {
        var person = _personRepository.GetByUserId(userId);
        if (person == null)
        {
            Console.WriteLine($"Warning: Person not found for userId: {userId}. XP not added.");
            return;
        }

        var oldLevel = person.Level;
        bool leveledUp = person.AddExperience(xpAmount);
        _personRepository.Update(person);

        if (leveledUp)
        {
            Console.WriteLine($"User {userId} reached level {person.Level}!");
            _badgeService.OnLevelUp(userId, person.Level);
        }
    }
}
