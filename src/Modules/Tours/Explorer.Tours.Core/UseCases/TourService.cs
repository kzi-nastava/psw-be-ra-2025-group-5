using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;

namespace Explorer.Tours.Core.UseCases;

public class TourService : ITourService
{
    private readonly ITourRepository _tourRepository;
    private readonly IMapper _mapper;

    public TourService(ITourRepository repository, IMapper mapper)
    {
        _tourRepository = repository;
        _mapper = mapper;
    }

    public PagedResult<TourDto> GetPaged(int page, int pageSize)
    {
        var result = _tourRepository.GetPaged(page, pageSize);
        var items = result.Results.Select(_mapper.Map<TourDto>).ToList();
        return new PagedResult<TourDto>(items, result.TotalCount);
    }

    public PagedResult<TourDto> GetPagedByAuthor(long authorId, int page, int pageSize)
    {
        var result = _tourRepository.GetPagedByAuthor(authorId, page, pageSize);
        var items = result.Results.Select(_mapper.Map<TourDto>).ToList();
        return new PagedResult<TourDto>(items, result.TotalCount);
    }

    public TourDto Get(long id)
    {
        var tour = _tourRepository.Get(id);
        return _mapper.Map<TourDto>(tour);
    }

    public List<string> GetAllTags()
    {
        var result = _tourRepository.GetAll();
        var items = result.SelectMany(t => t.Tags).Distinct().ToList();
        return items;
    }

    public TourDto Create(CreateTourDto dto)
    {
        var tour = _mapper.Map<Tour>(dto);
        var result = _tourRepository.Create(tour);
        return _mapper.Map<TourDto>(result);
    }

    public TourDto Update(long id, UpdateTourDto dto)
    {
        var tour = _tourRepository.Get(id);
        var difficulty = Enum.Parse<Domain.TourDifficulty>(dto.Difficulty, true);
        
        tour.Update(dto.Name, dto.Description, difficulty, dto.Tags, dto.Price);
        
        var result = _tourRepository.Update(tour);
        return _mapper.Map<TourDto>(result);
    }

    public void Delete(long id)
    {
        _tourRepository.Delete(id);
    }

    // KeyPoint operacije
    public TourDto AddKeyPoint(long tourId, CreateKeyPointDto keyPointDto)
    {
        var tour = _tourRepository.Get(tourId);
        var location = _mapper.Map<Location>(keyPointDto.Location);

        tour.AddKeyPoint(
            keyPointDto.Name,
            keyPointDto.Description,
            location,
            keyPointDto.Image,
            keyPointDto.Secret);

        var result = _tourRepository.Update(tour);
        return _mapper.Map<TourDto>(result);
    }

    public TourDto UpdateKeyPoint(long tourId, long keyPointId, CreateKeyPointDto keyPointDto)
    {
        var tour = _tourRepository.Get(tourId);

        tour.UpdateKeyPoint(
            keyPointId,
            keyPointDto.Name,
            keyPointDto.Description,
            keyPointDto.Image,
            keyPointDto.Secret);

        var result = _tourRepository.Update(tour);
        return _mapper.Map<TourDto>(result);
    }

    public TourDto RemoveKeyPoint(long tourId, long keyPointId)
    {
        var tour = _tourRepository.Get(tourId);
        tour.RemoveKeyPoint(keyPointId);

        var result = _tourRepository.Update(tour);
        return _mapper.Map<TourDto>(result);
    }

    public TourDto ReorderKeyPoints(long tourId, ReorderKeyPointsDto reorderDto)
    {
        var tour = _tourRepository.Get(tourId);
        tour.ReorderKeyPoints(reorderDto.OrderedKeyPointIds);

        var result = _tourRepository.Update(tour);
        return _mapper.Map<TourDto>(result);
    }

    // Status operacije
    public TourDto Publish(long tourId)
    {
        var tour = _tourRepository.Get(tourId);
        tour.Publish();

        var result = _tourRepository.Update(tour);
        return _mapper.Map<TourDto>(result);
    }

    public TourDto Archive(long tourId)
    {
        var tour = _tourRepository.Get(tourId);
        tour.Archive();

        var result = _tourRepository.Update(tour);
        return _mapper.Map<TourDto>(result);
    }

    public TourDto Reactivate(long tourId)
    {
        var tour = _tourRepository.Get(tourId);
        tour.Reactivate();

        var result = _tourRepository.Update(tour);
        return _mapper.Map<TourDto>(result);
    }

    // for tourists
    public PagedResult<TourDto> GetPagedPublished(int page, int pageSize)
    {
        var result = _tourRepository.GetPagedByStatus(TourStatus.Published, page, pageSize);

        var items = result.Results.Select(tour =>
        {
            var dto = _mapper.Map<TourDto>(tour);

            foreach (var kp in dto.KeyPoints)
            {
                kp.Secret = null;
            }

            return dto;
        }).ToList();

        return new PagedResult<TourDto>(items, result.TotalCount);
    }

    public TourDto GetPublished(long id)
    {
        var tour = _tourRepository.Get(id);

        if (tour == null || tour.Status != TourStatus.Published)
            throw new InvalidOperationException("Tour not found or not published.");

        var dto = _mapper.Map<TourDto>(tour);

        foreach (var kp in dto.KeyPoints)
        {
            kp.Secret = null;
        }

        return dto;
    }

    public KeyPointDto GetKeyPoint(long tourId, long keyPointId)
    {
        var tour = _tourRepository.Get(tourId);

        var keyPoint = tour.KeyPoints.FirstOrDefault(kp => kp.Id == keyPointId);
        if (keyPoint == null)
            throw new KeyNotFoundException("Key point not found.");

        var dto = _mapper.Map<KeyPointDto>(keyPoint);
        dto.Secret = null; 

        return dto;
    }



}