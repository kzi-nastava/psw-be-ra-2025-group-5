using Explorer.Stakeholders.Core.Domain.Positions;

namespace Explorer.Stakeholders.Core.Domain.RepositoryInterfaces.Positions;

public interface IPositionRepository
{
    List<Position> GetAll();
    Position? GetByTourist(long id);
    Position Create(Position entity);
    Position Update(Position entity);
}