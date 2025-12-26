using AutoMapper;
using Explorer.Payments.API.Dtos;
using Explorer.Payments.API.Public;
using Explorer.Payments.Core.Domain;
using Explorer.Payments.Core.Domain.RepositoryInterfaces;
using Explorer.Tours.API.Internal;

namespace Explorer.Payments.Core.UseCases;

public class ShoppingCartService : IShoppingCartService
{
    private readonly IShoppingCartRepository _ShoppingCartRepository;
    private readonly ITourSharedService _TourService;
    private readonly ITourPurchaseTokenService _TokenService;
    private readonly IMapper _mapper;

    public ShoppingCartService(IShoppingCartRepository repository, ITourSharedService tourService, IMapper mapper, ITourPurchaseTokenService tokenService)
    {
        _ShoppingCartRepository = repository;
        _TourService = tourService;
        _mapper = mapper;
        _TokenService = tokenService;
    }

    public List<ShoppingCartDto> GetAll()
    {
        var entities = _ShoppingCartRepository.GetAll();
        return _mapper.Map<List<ShoppingCartDto>>(entities);
    }

    public ShoppingCartDto GetByTourist(long touristId)
    {
        var result = _ShoppingCartRepository.GetByTourist(touristId);
        return _mapper.Map<ShoppingCartDto>(result);
    }

    public ShoppingCartDto Create(CreateShoppingCartDto entity)
    {
        if(GetAll().Any(cart => cart.TouristId == entity.TouristId))
            throw new InvalidOperationException($"Shopping cart already exists for tourist {entity.TouristId}");

        var result = _ShoppingCartRepository.Create(_mapper.Map<ShoppingCart>(entity));
        return _mapper.Map<ShoppingCartDto>(result);
    }

    public ShoppingCartDto AddOrderItem(long touristId, long tourId)
    {
        var cart = _ShoppingCartRepository.GetByTourist(touristId) ?? _ShoppingCartRepository.Create(new ShoppingCart(touristId));
        var tour = _TourService.Get(tourId);
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

    public ShoppingCartDto Checkout(long touristId)
    {
        var cart = _ShoppingCartRepository.GetByTourist(touristId);

        foreach (var item in cart.Items)
        {
            if (_TourService.Get(item.TourId).Status != "Published") continue;
            _TokenService.Create(new CreateTourPurchaseTokenDto { TourId = item.TourId, TouristId = touristId });
        }

        cart.ClearShoppingCart();

        var result = _ShoppingCartRepository.Update(cart);
        return _mapper.Map<ShoppingCartDto>(result);
    }
}
