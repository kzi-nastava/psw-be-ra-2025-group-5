using AutoMapper;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Shopping;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;

namespace Explorer.Tours.Core.UseCases.Shopping;

public class ShoppingCartService : IShoppingCartService
{
    private readonly IShoppingCartRepository _ShoppingCartRepository;
    private readonly IMapper _mapper;

    public ShoppingCartService(IShoppingCartRepository repository, IMapper mapper)
    {
        _ShoppingCartRepository = repository;
        _mapper = mapper;
    }

    public ShoppingCartDto GetByTourist(long touristId)
    {
        var result = _ShoppingCartRepository.GetByTourist(touristId);
        return _mapper.Map<ShoppingCartDto>(result);
    }

    public ShoppingCartDto Create(ShoppingCartDto entity)
    {
        var result = _ShoppingCartRepository.Create(_mapper.Map<ShoppingCart>(entity));
        return _mapper.Map<ShoppingCartDto>(result);
    }

    public ShoppingCartDto Update(ShoppingCartDto entity)
    {
        var result = _ShoppingCartRepository.Update(_mapper.Map<ShoppingCart>(entity));
        return _mapper.Map<ShoppingCartDto>(result);
    }
}
