using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public.Reporting;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;

namespace Explorer.Stakeholders.Core.UseCases.Reporting;

public class TourProblemService : ITourProblemService
{
    private readonly ITourProblemRepository _repository;
    private readonly IMapper _mapper;

    public TourProblemService(ITourProblemRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public PagedResult<TourProblemDto> GetPaged(int page, int pageSize)
    {
        var result = _repository.GetPaged(page, pageSize);
        var items = result.Results.Select(_mapper.Map<TourProblemDto>).ToList();
        return new PagedResult<TourProblemDto>(items, result.TotalCount);
    }

    public TourProblemDto Create(TourProblemDto dto)
    {
        var created = _repository.Create(_mapper.Map<TourProblem>(dto));
        return _mapper.Map<TourProblemDto>(created);
    }

    public TourProblemDto Update(TourProblemDto dto)
    {
        var updated = _repository.Update(_mapper.Map<TourProblem>(dto));
        return _mapper.Map<TourProblemDto>(updated);
    }

    public void Delete(long id) => _repository.Delete(id);
}