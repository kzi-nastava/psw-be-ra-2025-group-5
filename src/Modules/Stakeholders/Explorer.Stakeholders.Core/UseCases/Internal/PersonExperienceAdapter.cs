using Explorer.Encounters.API.Internal;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces.Users;

namespace Explorer.Stakeholders.Core.UseCases.Internal;

public class PersonExperienceAdapter : IInternalPersonExperienceService
{
    private readonly IPersonRepository _personRepository;

    public PersonExperienceAdapter(IPersonRepository personRepository)
    {
        _personRepository = personRepository;
    }

    public void AddExperience(long userId, int xpAmount)
    {
        var person = _personRepository.GetByUserId(userId);
        if (person == null)
        {
            Console.WriteLine($"Warning: Person not found for userId: {userId}. XP not added.");
            return;
        }

        bool leveledUp = person.AddExperience(xpAmount);
        _personRepository.Update(person);

        if (leveledUp)
        {
            Console.WriteLine($"User {userId} reached level {person.Level}!");
        }
    }
}
