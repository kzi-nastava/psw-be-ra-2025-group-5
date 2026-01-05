using AutoMapper;
using Explorer.Payments.API.Dtos.Pricing;
using Explorer.Payments.API.Public;
using Explorer.Payments.Core.Domain;
using Explorer.Payments.Core.Domain.RepositoryInterfaces;
using Explorer.Tours.API.Internal;

namespace Explorer.Payments.Core.UseCases;

public class TourSaleService : ITourSaleService
{
    private readonly IMapper _mapper;
    private readonly ITourSaleRepository _TourSaleRepository;
    private readonly ITourSharedService _TourService;

    public TourSaleService(IMapper mapper, ITourSaleRepository tourSaleRepository, ITourSharedService tourService)
    {
        _mapper = mapper;
        _TourSaleRepository = tourSaleRepository;
        _TourService = tourService;
    }

    public List<TourSaleDto> GetAll()
    {
        var entities = _TourSaleRepository.GetAll();
        return _mapper.Map<List<TourSaleDto>>(entities);
    }

    public TourSaleDto? Get(long id) => _mapper.Map<TourSaleDto>(_TourSaleRepository.Get(id));

    public TourSaleDto? GetActiveSaleForTour(long tourId) => _mapper.Map<TourSaleDto>(_TourSaleRepository.GetActiveSaleForTour(tourId));

    public List<TourSaleDto> GetByAuthor(long authorId, bool onlyActive) => _mapper.Map<List<TourSaleDto>>(_TourSaleRepository.GetByAuthor(authorId, onlyActive));

    public TourPriceDto GetFinalPrice(long tourId)
    {
        var tour = _TourService.Get(tourId);
        var sale = _TourSaleRepository.GetActiveSaleForTour(tourId);

        return sale == null
            ? TourPriceDto.NoDiscount(tour.Price)
            : TourPriceDto.WithDiscount(tour.Price, sale.DiscountPercentage);
    }

    public TourSaleDto Create(CreateTourSaleDto entity)
    {
        if (entity.TourIds.Any(tour => _TourSaleRepository.GetActiveSaleForTour(tour) != null))
            throw new InvalidOperationException($"One or more tours are already on sale.");

        if (entity.TourIds.Except(_TourService.GetPagedByAuthor(entity.AuthorId, 0, 0).Results.Select(t => t.Id)).Any())
            throw new InvalidOperationException($"Only self-authored tours can be put on sale.");

        var result = _TourSaleRepository.Create(_mapper.Map<TourSale>(entity));
        return _mapper.Map<TourSaleDto>(result);
    }

    public TourSaleDto Update(long id, TourSaleDto entity)
    {
        var sale = _TourSaleRepository.Get(id);
        if (sale == null) return null;

        if (entity.TourIds.Except(sale.TourIds).Any(tour => _TourSaleRepository.GetActiveSaleForTour(tour) != null))
            throw new InvalidOperationException($"One or more tours are already on sale.");

        if (entity.TourIds.Except(_TourService.GetPagedByAuthor(entity.AuthorId, 0, 0).Results.Select(t => t.Id)).Any())
            throw new InvalidOperationException($"Only self-authored tours can be put on sale.");

        _mapper.Map(entity, sale, opt => opt.Items["ignoreTourIds"] = true);
        sale.TourIds.Clear();
        entity.TourIds.ForEach(id => sale.TourIds.Add(id));

        var result = _TourSaleRepository.Update(sale);
        return _mapper.Map<TourSaleDto>(result);
    }

    public void Delete(long id)
    {
        _TourSaleRepository.Delete(id);
    }
}
