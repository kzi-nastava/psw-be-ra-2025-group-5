using AutoMapper;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Shopping;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;

namespace Explorer.Tours.Core.UseCases.Shopping;

public class TourPurchaseTokenService : ITourPurchaseTokenService
{
    private readonly ITourPurchaseTokenRepository _TourPurchaseTokenRepository;
    private readonly IMapper _mapper;

    public TourPurchaseTokenService(ITourPurchaseTokenRepository repository, IMapper mapper)
    {
        _TourPurchaseTokenRepository = repository;
        _mapper = mapper;
    }

    public List<TourPurchaseTokenDto> GetAll()
    {
        var entities = _TourPurchaseTokenRepository.GetAll();
        return _mapper.Map<List<TourPurchaseTokenDto>>(entities);
    }

    public TourPurchaseTokenDto GetByTourAndTourist(long tourId, long touristId)
    {
        var result = _TourPurchaseTokenRepository.GetByTourAndTourist(tourId, touristId);
        return _mapper.Map<TourPurchaseTokenDto>(result);
    }

    public TourPurchaseTokenDto Create(CreateTourPurchaseTokenDto entity)
    {
        if (GetAll().Any(token => token.TouristId == entity.TouristId && token.TourId == entity.TourId))
            throw new InvalidOperationException($"Tour {entity.TourId} purchase token already exists for tourist {entity.TouristId}");

        var result = _TourPurchaseTokenRepository.Create(_mapper.Map<TourPurchaseToken>(entity));
        return _mapper.Map<TourPurchaseTokenDto>(result);
    }
}
