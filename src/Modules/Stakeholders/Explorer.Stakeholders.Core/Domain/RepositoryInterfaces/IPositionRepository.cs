namespace Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;

public interface IPositionRepository
{
    List<Position> GetAll();
    Position? GetByTourist(long id);
    Position Create(Position entity);
    Position Update(Position entity);
}