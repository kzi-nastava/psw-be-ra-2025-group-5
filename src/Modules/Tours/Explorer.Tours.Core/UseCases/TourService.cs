using AutoMapper;
using Explorer.BuildingBlocks.Core.Exceptions;
using Explorer.BuildingBlocks.Core.FileStorage;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using TourDifficulty = Explorer.Tours.Core.Domain.TourDifficulty;

namespace Explorer.Tours.Core.UseCases;

public class TourService : ITourService
{
    private readonly ITourRepository _tourRepository;
    private readonly IImageStorage _imageStorage;
    private readonly ITourExecutionRepository _tourExecutionRepository;
    private readonly ITourPurchaseTokenRepository _purchaseTokenRepository;
    private readonly IEquipmentRepository _equipmentRepository;
    private readonly IMapper _mapper;

    public TourService(ITourRepository repository, IImageStorage imageStorage, IMapper mapper, ITourExecutionRepository execution, ITourPurchaseTokenRepository purchaseToken, IEquipmentRepository equipmentRepository)
    {
        _tourRepository = repository;
        _imageStorage = imageStorage;
        _mapper = mapper;
        _tourExecutionRepository = execution;
        _purchaseTokenRepository = purchaseToken;
        _equipmentRepository = equipmentRepository;
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

    public TourDto Create(CreateTourDto dto, long authorId)
    {
        var tour = new Tour(
            (int)authorId,
            dto.Name,
            dto.Description,
            Enum.Parse<TourDifficulty>(dto.Difficulty),
            dto.Tags,
            dto.Price
        );

        var created = _tourRepository.Create(tour);
        return _mapper.Map<TourDto>(created);
    }

    public TourDto Update(long id, UpdateTourDto dto, long authorId)
    {
        var tour = _tourRepository.Get(id);
        if (tour == null)
            throw new NotFoundException("Tour not found.");

        var difficulty = Enum.Parse<TourDifficulty>(dto.Difficulty, true);
        tour.Update(dto.Name, dto.Description, difficulty, dto.Tags, dto.Price);

        // Sync Durations
        var existingDurations = tour.Durations.ToList();
        var newDurationsDto = dto.Durations ?? new List<TourDurationDto>();
        var newDurations = newDurationsDto.Select(d => _mapper.Map<TourDuration>(d)).ToList();

        // Add new durations
        foreach (var duration in newDurations)
        {
            if (!existingDurations.Contains(duration))
            {
                tour.AddDuration(duration);
            }
        }

        // Remove old durations
        foreach (var existingDuration in existingDurations)
        {
            if (!newDurations.Contains(existingDuration))
            {
                tour.RemoveDuration(existingDuration);
            }
        }

        var updated = _tourRepository.Update(tour);
        return _mapper.Map<TourDto>(updated);
    }

    public TourThumbnailDto AddThumbnail(long tourId, byte[] image, string contentType)
    {
        var tour = _tourRepository.Get(tourId);
        if (tour == null)
            throw new NotFoundException("Tour not found.");

        if (image == null || image.Length == 0)
            throw new ArgumentException("Image cannot be empty.", nameof(image));

        var thumbnailPath = _imageStorage.SaveImage("tours", tourId, image, contentType);
        tour.SetThumbnail(thumbnailPath, contentType);
        _tourRepository.Update(tour);

        return new TourThumbnailDto
        {
            TourId = tourId,
            Url = thumbnailPath,
            ContentType = contentType
        };
    }

    public TourThumbnailDto GetThumbnail(long tourId)
    {
        var tour = _tourRepository.Get(tourId);
        if (tour == null)
            throw new NotFoundException("Tour not found.");

        return new TourThumbnailDto
        {
            TourId = tourId,
            Url = tour.ThumbnailPath,
            ContentType = tour.ThumbnailContentType
        };
    }

    public void Delete(long id)
    {
        var tour = _tourRepository.Get(id);
        if (tour != null && !string.IsNullOrEmpty(tour.ThumbnailPath))
        {
            _imageStorage.Delete(tour.ThumbnailPath);
        }
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

        if (tour.KeyPoints.Count > 1)
        {
            tour.UpdateTourLength(keyPointDto.TourLength);
        }

        var result = _tourRepository.Update(tour);
        return _mapper.Map<TourDto>(result);
    }

    public TourDto UpdateKeyPoint(long tourId, long keyPointId, CreateKeyPointDto keyPointDto)
    {
        var tour = _tourRepository.Get(tourId);
        var location = _mapper.Map<Location>(keyPointDto.Location);

        tour.UpdateKeyPoint(
            keyPointId,
            keyPointDto.Name,
            keyPointDto.Description,
            keyPointDto.Image,
            keyPointDto.Secret,
            location);

        if (tour.KeyPoints.Count > 1)
        {
            tour.UpdateTourLength(keyPointDto.TourLength);
        }

        var result = _tourRepository.Update(tour);
        return _mapper.Map<TourDto>(result);
    }

    public TourDto RemoveKeyPoint(long tourId, long keyPointId, double tourLength)
    {
        var tour = _tourRepository.Get(tourId);
        tour.RemoveKeyPoint(keyPointId);
        if (tour.KeyPoints.Count < 2)
        {
            tour.UpdateTourLength(0.0);
        }
        else
        {
            tour.UpdateTourLength(tourLength);
        }

        var result = _tourRepository.Update(tour);
        return _mapper.Map<TourDto>(result);
    }

    public TourDto ReorderKeyPoints(long tourId, ReorderKeyPointsDto reorderDto)
    {
        var tour = _tourRepository.Get(tourId);
        tour.ReorderKeyPoints(reorderDto.OrderedKeyPointIds);
        tour.UpdateTourLength(reorderDto.TourLength);

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

    public TourDto AddReview(long tourId, long userId, string username, TourReviewDto dto)
    {
        var purchaseToken = _purchaseTokenRepository.GetByTourAndTourist(tourId, userId);
        if (purchaseToken == null)
            throw new InvalidOperationException("User has not purchased this tour.");

        var tour = _tourRepository.Get(tourId);
        var execution = _tourExecutionRepository.GetActiveOrCompletedForUser(userId, tourId);

        var timeSinceLastActivity = DateTime.UtcNow - execution.LastActivity;
        if (timeSinceLastActivity > TimeSpan.FromDays(7))
            throw new Exception("Too much time has passed since your last activity on this tour.");

        int total = tour.KeyPoints.Count;
        int completed = execution.CompletedKeyPoints.Count;
        var progress = total == 0 ? 0 : (100.0 * completed / total);

        List<ReviewImage>? images = null;
        if (dto.Images != null && dto.Images.Any())
        {
            images = dto.Images
                .OrderBy(i => i.Order)
                .Select((img, index) =>
                    new ReviewImage(0, Convert.FromBase64String(img.Data), img.ContentType, index))
                .ToList();
        }

        tour.AddReview(dto.Grade, dto.Comment, DateTime.UtcNow, progress, userId, username, images);

        var updatedTour = _tourRepository.Update(tour);
        return _mapper.Map<TourDto>(updatedTour);
    }

    public TourDto UpdateReview(long tourId, long userId, long reviewId, TourReviewDto dto)
    {
        var tour = _tourRepository.Get(tourId);
        var execution = _tourExecutionRepository.GetActiveOrCompletedForUser(userId, tourId);

        var timeSinceLastActivity = DateTime.UtcNow - execution.LastActivity;
        if (timeSinceLastActivity > TimeSpan.FromDays(7))
            throw new Exception("Too much time has passed since your last activity on this tour.");

        List<ReviewImage>? images = null;

        if (dto.Images != null && dto.Images.Any())
        {
            images = dto.Images
                .OrderBy(i => i.Order)
                .Select((img, index) =>
                    new ReviewImage(reviewId, Convert.FromBase64String(img.Data), img.ContentType, index))
                .ToList();
        }

        tour.UpdateReview(reviewId, dto.Grade, dto.Comment, dto.Progress, images);

        var updatedTour = _tourRepository.Update(tour);
        return _mapper.Map<TourDto>(updatedTour);
    }

    public TourDto RemoveReview(long tourId, long reviewId)
    {
        var tour = _tourRepository.Get(tourId);

        tour.RemoveReview(reviewId);

        var updatedTour = _tourRepository.Update(tour);
        return _mapper.Map<TourDto>(updatedTour);
    }

    // Duration operacije
    public TourDto AddDuration(long tourId, TourDurationDto durationDto)
    {
        var tour = _tourRepository.Get(tourId);
        var duration = _mapper.Map<TourDuration>(durationDto);
        tour.AddDuration(duration);
        var result = _tourRepository.Update(tour);
        return _mapper.Map<TourDto>(result);
    }

    public TourDto RemoveDuration(long tourId, TourDurationDto durationDto)
    {
        var tour = _tourRepository.Get(tourId);
        var duration = _mapper.Map<TourDuration>(durationDto);
        tour.RemoveDuration(duration);
        var result = _tourRepository.Update(tour);
        return _mapper.Map<TourDto>(result);
    }

    public int GetReviewButtonState(long tourId, long userId)
    {
        // 0 = ne ispunjava uslove
        // 1 = može da ostavi recenziju (Leave)
        // 2 = može da izmeni recenziju (Edit)
  
        var purchaseToken = _purchaseTokenRepository.GetByTourAndTourist(tourId, userId);
        if (purchaseToken == null)
            return 0;

        var execution = _tourExecutionRepository.GetActiveOrCompletedForUser(userId, tourId);
        if (execution == null || (DateTime.UtcNow - execution.LastActivity) > TimeSpan.FromDays(7))
            return 0;

        var tour = _tourRepository.Get(tourId);

        int total = tour.KeyPoints.Count;
        int completed = execution.CompletedKeyPoints.Count;
        double progress = total == 0 ? 100 : (100.0 * completed / total);
        if (progress < 35)
            return 0;

        bool hasReview = tour.Reviews.Any(r => r.TouristID == userId);

        return hasReview ? 2 : 1;
    }

    public TourDto AddRequiredEquipment(long tourId, long equipmentId)
    {
        var tour = _tourRepository.Get(tourId);

        tour.AddRequiredEquipment(equipmentId);

        var result = _tourRepository.Update(tour);
        return _mapper.Map<TourDto>(result);
    }

    public TourDto RemoveRequiredEquipment(long tourId, long equipmentId)
    {
        var tour = _tourRepository.Get(tourId);

        tour.RemoveRequiredEquipment(equipmentId);

        var result = _tourRepository.Update(tour);
        return _mapper.Map<TourDto>(result);
    }

    public void CloseTour(long tourId)
    {
        _tourRepository.Close(tourId);
    }

    public TourDto GetById(long id)
    {
        var tour = _tourRepository.Get(id);
        return _mapper.Map<TourDto>(tour);
    }
}