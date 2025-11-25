using AutoMapper;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;

namespace Explorer.Stakeholders.Core.UseCases;

public class TouristLocationService : ITouristLocationService
{
    private readonly ITouristLocationRepository _TouristLocationRepository;
    private readonly IMapper _mapper;

    public TouristLocationService(ITouristLocationRepository repository, IMapper mapper)
    {
        _TouristLocationRepository = repository;
        _mapper = mapper;
    }

    public TouristLocationDto? GetByTourist(long id)
    {
        var entity = _TouristLocationRepository.GetByTourist(id);
        return entity is null ? null : _mapper.Map<TouristLocationDto>(entity);
    }

    public IEnumerable<TouristLocationDto> GetAll()
    {
        var entities = _TouristLocationRepository.GetAll();
        return _mapper.Map<IEnumerable<TouristLocationDto>>(entities);
    }


    public TouristLocationDto Create(TouristLocationDto entity)
    {
        var result = _TouristLocationRepository.Create(_mapper.Map<TouristLocation>(entity));
        return _mapper.Map<TouristLocationDto>(result);
    }

    public TouristLocationDto Update(TouristLocationDto entity)
    {
        var result = _TouristLocationRepository.Update(_mapper.Map<TouristLocation>(entity));
        return _mapper.Map<TouristLocationDto>(result);
    }
}
