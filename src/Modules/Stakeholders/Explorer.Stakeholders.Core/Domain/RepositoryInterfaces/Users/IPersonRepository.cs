using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.Core.Domain.Users.Entities;

namespace Explorer.Stakeholders.Core.Domain.RepositoryInterfaces.Users;

public interface IPersonRepository
{
    Person Create(Person person);
    Person? GetByUserId(long userId);
    Person Update(Person person);
    Person? Get(long id);
    PagedResult<Person> GetPaged(int page, int pageSize);

}