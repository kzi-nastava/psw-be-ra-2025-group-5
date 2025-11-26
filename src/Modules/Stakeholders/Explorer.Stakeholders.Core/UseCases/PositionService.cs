using AutoMapper;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;

namespace Explorer.Stakeholders.Core.UseCases;

public class PositionService : IPositionService
{
    private readonly IPositionRepository _PositionRepository;
    private readonly IMapper _mapper;

    public PositionService(IPositionRepository repository, IMapper mapper)
    {
        _PositionRepository = repository;
        _mapper = mapper;
    }

    public PositionDto? GetByTourist(long id)
    {
        var entity = _PositionRepository.GetByTourist(id);
        return entity is null ? null : _mapper.Map<PositionDto>(entity);
    }

    public IEnumerable<PositionDto> GetAll()
    {
        var entities = _PositionRepository.GetAll();
        return _mapper.Map<IEnumerable<PositionDto>>(entities);
    }

    public PositionDto Create(PositionDto entity)
    {
        var result = _PositionRepository.Create(_mapper.Map<Position>(entity));
        return _mapper.Map<PositionDto>(result);
    }

    public PositionDto Update(PositionDto entity)
    {
        var result = _PositionRepository.Update(_mapper.Map<Position>(entity));
        return _mapper.Map<PositionDto>(result);
    }
}
