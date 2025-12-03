using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;

namespace Explorer.Tours.Core.UseCases;

public class TourService : ITourService
{
    private readonly ITourRepository _TourRepository;
    private readonly IMapper _mapper;

    public TourService(ITourRepository repository, IMapper mapper)
    {
        _TourRepository = repository;
        _mapper = mapper;
    }

    public PagedResult<TourDto> GetPaged(int page, int pageSize)
    {
        var result = _TourRepository.GetPaged(page, pageSize);

        var items = result.Results.Select(_mapper.Map<TourDto>).ToList();
        return new PagedResult<TourDto>(items, result.TotalCount);
    }

    public PagedResult<TourDto> GetPagedByAuthor(long authorId, int page, int pageSize)
    {
        var result = _TourRepository.GetPagedByAuthor(authorId, page, pageSize);

        var items = result.Results.Select(_mapper.Map<TourDto>).ToList();
        return new PagedResult<TourDto>(items, result.TotalCount);
    }

    public TourDto Get(long id)
    {
        var tour = _TourRepository.Get(id);
        return _mapper.Map<TourDto>(tour);
    }

    public List<string> GetAllTags()
    {
        var result = _TourRepository.GetAll();

        var items = result.SelectMany(t => t.Tags).Distinct().ToList();
        return items;
    }

    public TourDto Create(TourDto entity)
    {
        var result = _TourRepository.Create(_mapper.Map<Tour>(entity));
        return _mapper.Map<TourDto>(result);
    }

    public TourDto Update(TourDto entity)
    {
        var result = _TourRepository.Update(_mapper.Map<Tour>(entity));
        return _mapper.Map<TourDto>(result);
    }

    public void Delete(long id)
    {
        _TourRepository.Delete(id);
    }
}