namespace Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;

public interface ITouristLocationRepository
{
    List<TouristLocation> GetAll();
    TouristLocation? GetByTourist(long id);
    TouristLocation Create(TouristLocation entity);
    TouristLocation Update(TouristLocation entity);
}