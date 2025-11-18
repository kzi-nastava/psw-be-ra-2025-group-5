using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;

namespace Explorer.Tours.Core.UseCases.Administration;

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