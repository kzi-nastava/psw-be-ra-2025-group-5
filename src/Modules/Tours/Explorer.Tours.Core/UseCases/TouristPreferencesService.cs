using AutoMapper;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;

namespace Explorer.Tours.Core.UseCases;

public class TouristPreferencesService : ITouristPreferencesService
{
    private readonly ITouristPreferencesRepository _repository;
    private readonly IMapper _mapper;

    public TouristPreferencesService(ITouristPreferencesRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public TouristPreferencesDto Get(long userId)
    {
        var entity = _repository.Get(userId);
        return _mapper.Map<TouristPreferencesDto>(entity);
    }

    public TouristPreferencesDto Create(TouristPreferencesDto dto)
    {
        var entity = _mapper.Map<TouristPreferences>(dto);
        var result = _repository.Create(entity);
        return _mapper.Map<TouristPreferencesDto>(result);
    }

    public TouristPreferencesDto Update(TouristPreferencesDto dto)
    {
        var entity = _mapper.Map<TouristPreferences>(dto);
        var result = _repository.Update(entity);
        return _mapper.Map<TouristPreferencesDto>(result);
    }

    public void Delete(long userId)
    {
        _repository.Delete(userId);
    }
}
