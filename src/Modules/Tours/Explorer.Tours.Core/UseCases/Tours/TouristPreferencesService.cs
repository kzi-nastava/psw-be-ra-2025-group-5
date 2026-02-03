using AutoMapper;
using Explorer.BuildingBlocks.Core.Exceptions;
using Explorer.Stakeholders.API.Internal;
using Explorer.Tours.API.Dtos.Preferences;
using Explorer.Tours.API.Public.Tour;
using Explorer.Tours.Core.Domain.Preferences;
using Explorer.Tours.Core.Domain.RepositoryInterfaces.Tours;

namespace Explorer.Tours.Core.UseCases.Tours;

public class TouristPreferencesService : ITouristPreferencesService
{
    private readonly ITouristPreferencesRepository _repository;
    private readonly IMapper _mapper;
    private readonly IPremiumSharedService _premiumSharedService;

    public TouristPreferencesService(ITouristPreferencesRepository repository, IMapper mapper, IPremiumSharedService premiumSharedService)
    {
        _repository = repository;
        _mapper = mapper;
        _premiumSharedService = premiumSharedService;
    }

    public TouristPreferencesDto Get(long userId)
    {
        if (!_premiumSharedService.IsPremium(userId))
            return null;

        var entity = _repository.Get(userId);
        return _mapper.Map<TouristPreferencesDto>(entity);
    }

    public TouristPreferencesDto Create(TouristPreferencesDto dto)
    {
        EnsurePremium(dto.UserId);

        var entity = _mapper.Map<TouristPreferences>(dto);
        var result = _repository.Create(entity);
        return _mapper.Map<TouristPreferencesDto>(result);
    }

    public TouristPreferencesDto Update(TouristPreferencesDto dto)
    {
        EnsurePremium(dto.UserId);

        var entity = _mapper.Map<TouristPreferences>(dto);
        var result = _repository.Update(entity);
        return _mapper.Map<TouristPreferencesDto>(result);
    }

    public void Delete(long userId)
    {
        EnsurePremium(userId);

        _repository.Delete(userId);
    }

    private void EnsurePremium(long userId)
    {
        if (!_premiumSharedService.IsPremium(userId))
            throw new ForbiddenException("Only premium tourists can access preferences.");
    }
}
