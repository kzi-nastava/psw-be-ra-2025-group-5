using AutoMapper;
using Explorer.Payments.API.Dtos.PurchaseToken;
using Explorer.Payments.API.Internal;
using Explorer.Payments.API.Public;
using Explorer.Payments.Core.Domain;
using Explorer.Payments.Core.Domain.RepositoryInterfaces;

namespace Explorer.Payments.Core.UseCases;

public class TourPurchaseTokenService : ITourPurchaseTokenService, ITourPurchaseTokenSharedService
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

    public List<TourPurchaseTokenDto> GetByTourist(long touristId)
    {
        var result = _TourPurchaseTokenRepository.GetByTourist(touristId);
        return _mapper.Map<List<TourPurchaseTokenDto>>(result);
    }

    public TourPurchaseTokenDto Create(CreateTourPurchaseTokenDto entity)
    {
        if (GetAll().Any(token => token.TouristId == entity.TouristId && token.TourId == entity.TourId))
            throw new InvalidOperationException($"Tour {entity.TourId} purchase token already exists for tourist {entity.TouristId}");

        var result = _TourPurchaseTokenRepository.Create(_mapper.Map<TourPurchaseToken>(entity));
        return _mapper.Map<TourPurchaseTokenDto>(result);
    }

    public bool HasUsedPremiumWheelThisMonth(long touristId)
    {
        var oneMonthAgo = DateTime.UtcNow.AddMonths(-1);

        return _TourPurchaseTokenRepository
            .GetByTourist(touristId)
            .Any(t =>
                t.IsFree &&
                t.PurchasedAt >= oneMonthAgo
            );
    }

    public TourPurchaseTokenDto CreateFreeTokenFromWheel(long tourId, long touristId)
    {
        var dto = new CreateTourPurchaseTokenDto
        {
            TourId = tourId,
            TouristId = touristId,
            IsFree = true
        };

        var token = _TourPurchaseTokenRepository.Create(
            _mapper.Map<TourPurchaseToken>(dto)
        );

        return _mapper.Map<TourPurchaseTokenDto>(token);
    }


}
