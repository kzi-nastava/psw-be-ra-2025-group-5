using Explorer.Payments.API.Dtos.Pricing;

namespace Explorer.Payments.API.Public;

public interface ITourSaleService
{
    List<TourSaleDto> GetAll();
    TourSaleDto? Get(long id);
    List<TourSaleDto> GetByAuthor(long authorId, bool onlyActive);
    TourSaleDto Create(CreateTourSaleDto entity);
    TourSaleDto Update(long id, TourSaleDto entity);
    void Delete(long id);
    TourSaleDto? GetActiveSaleForTour(long tourId);
    TourPriceDto GetFinalPrice(long tourId);
}
