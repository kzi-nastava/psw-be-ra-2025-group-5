
namespace Explorer.Payments.Core.Domain.RepositoryInterfaces;

public interface ITourSaleRepository
{
    List<TourSale> GetAll();
    TourSale? Get(long id);
    TourSale? GetActiveSaleForTour(long tourId);
    List<TourSale> GetByAuthor(long authorId, bool onlyActive);
    TourSale Create(TourSale entity);
    TourSale Update(TourSale entity);
    void Delete(long id);
}
