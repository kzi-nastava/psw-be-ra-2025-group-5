using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos.Badges;
using Explorer.Stakeholders.API.Public.Badges;
using Explorer.Stakeholders.Core.Domain.Badges;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces.Badges;

namespace Explorer.Stakeholders.Core.UseCases.Administration.Badges;

public class BadgeService : IBadgeService
{
    private readonly IBadgeRepository _repository;
    private readonly IMapper _mapper;

    public BadgeService(IBadgeRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public BadgeDto Get(long id)
    {
        var entity = _repository.Get(id);
        if (entity == null)
            throw new KeyNotFoundException($"Badge with Id={id} not found.");
        return _mapper.Map<BadgeDto>(entity);
    }

    public PagedResult<BadgeDto> GetPaged(int page, int pageSize)
    {
        var pagedResult = _repository.GetPaged(page, pageSize);
        return new PagedResult<BadgeDto>(
            _mapper.Map<List<BadgeDto>>(pagedResult.Results),
            pagedResult.TotalCount
        );
    }

    public List<BadgeDto> GetAll()
    {
        var entities = _repository.GetAll();
        return _mapper.Map<List<BadgeDto>>(entities);
    }

    public List<BadgeDto> GetByType(int type)
    {
        var entities = _repository.GetByType((BadgeType)type);
        return _mapper.Map<List<BadgeDto>>(entities);
    }

    public List<BadgeDto> GetByRole(int role)
    {
        var entities = _repository.GetByRole((BadgeRole)role);
        return _mapper.Map<List<BadgeDto>>(entities);
    }

    public List<BadgeDto> GetByRank(int rank)
    {
        var entities = _repository.GetByRank((BadgeRank)rank);
        return _mapper.Map<List<BadgeDto>>(entities);
    }

    public List<BadgeDto> GetByName(string name)
    {
        var entities = _repository.GetByName(name);
        return _mapper.Map<List<BadgeDto>>(entities);
    }
}


