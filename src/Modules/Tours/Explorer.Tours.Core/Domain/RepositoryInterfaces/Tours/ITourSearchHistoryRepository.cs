using Explorer.Tours.Core.Domain.Tours.Entities;

namespace Explorer.Tours.Core.Domain.RepositoryInterfaces.Tours;

public interface ITourSearchHistoryRepository
{
    TourSearchHistory Create(TourSearchHistory searchHistory);
    List<TourSearchHistory> GetByUser(long userId, int? limit = null);
    void Delete(long id);
}

