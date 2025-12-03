using AutoMapper;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Shopping;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;

namespace Explorer.Tours.Core.UseCases.Shopping;

public class ShoppingCartService : IShoppingCartService
{
    private readonly IShoppingCartRepository _ShoppingCartRepository;
    private readonly ITourRepository _TourRepository;
    private readonly IMapper _mapper;

    public ShoppingCartService(IShoppingCartRepository repository, ITourRepository tourRepository, IMapper mapper)
    {
        _ShoppingCartRepository = repository;
        _TourRepository = tourRepository;
        _mapper = mapper;
    }

    public ShoppingCartDto GetByTourist(long touristId)
    {
        var result = _ShoppingCartRepository.GetByTourist(touristId);
        return _mapper.Map<ShoppingCartDto>(result);
    }

    public ShoppingCartDto Create(CreateShoppingCartDto entity)
    {
        if(_ShoppingCartRepository.GetByTourist(entity.TouristId) is not null)
            throw new InvalidOperationException($"Shopping cart already exists for tourist {entity.TouristId}");

        var result = _ShoppingCartRepository.Create(_mapper.Map<ShoppingCart>(entity));
        return _mapper.Map<ShoppingCartDto>(result);
    }

    public ShoppingCartDto AddOrderItem(long touristId, long tourId)
    {
        var cart = _ShoppingCartRepository.GetByTourist(touristId);
        var tour = _TourRepository.Get(tourId);
        cart.AddItem(tour.Id, tour.Name, tour.Price);

        var result = _ShoppingCartRepository.Update(cart);
        return _mapper.Map<ShoppingCartDto>(result);
    }

    public ShoppingCartDto RemoveOrderItem(long touristId, long tourId)
    {
        var cart = _ShoppingCartRepository.GetByTourist(touristId);
        cart.RemoveItem(tourId);

        var result = _ShoppingCartRepository.Update(cart);
        return _mapper.Map<ShoppingCartDto>(result);
    }
}
