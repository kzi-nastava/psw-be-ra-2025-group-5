using AutoMapper;
using Explorer.BuildingBlocks.Core.FileStorage;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Payments.API.Internal;
using Explorer.Tours.API.Dtos.KeyPoints;
using Explorer.Tours.API.Dtos.Tours;
using Explorer.Tours.API.Dtos.Tours.Reviews;
using Explorer.Tours.API.Internal;
using Explorer.Tours.API.Public.Tour;
using Explorer.Tours.Core.Domain.RepositoryInterfaces.Equipments;
using Explorer.Tours.Core.Domain.RepositoryInterfaces.Tours;
using Explorer.Tours.Core.Domain.Tours;
using Explorer.Tours.Core.Domain.Tours.Entities;
using Explorer.Tours.Core.Domain.Tours.ValueObjects;
using Explorer.Tours.Core.Domain.Shared;
using Explorer.Tours.API.Dtos;
using Microsoft.AspNetCore.Http;
using Explorer.Stakeholders.API.Internal;


namespace Explorer.Tours.Core.UseCases.Tours;

public class TourService : ITourService, ITourSharedService
{
    private readonly ITourRepository _tourRepository;
    private readonly ITourExecutionRepository _tourExecutionRepository;
    private readonly ITourPurchaseTokenSharedService _purchaseTokenService;
    private readonly IImageStorage _imageStorage;
    private readonly IMapper _mapper;
    private readonly IInternalBadgeService _badgeService;


    public TourService(
     ITourRepository repository,
     IMapper mapper,
     ITourExecutionRepository execution,
     ITourPurchaseTokenSharedService purchaseToken,
     IEquipmentRepository equipmentRepository,
     IImageStorage imageStorage,
     IInternalBadgeService badgeService)
    {
        _tourRepository = repository;
        _mapper = mapper;
        _tourExecutionRepository = execution;
        _purchaseTokenService = purchaseToken;
        _imageStorage = imageStorage;
        _badgeService = badgeService;
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

    public List<TourDto> GetMultiple(long[] ids)
    {
        var result = _tourRepository.GetAll();
        var filtered = result.Where(t => ids.Contains(t.Id)).ToList();
        var items = filtered.Select(_mapper.Map<TourDto>).ToList();
        return items;
    }

    public PagedResult<TourDto> SearchByLocation(TourSearchDto searchDto, int page, int pageSize)
    {
        Guard.AgainstNegative(searchDto.Distance, nameof(searchDto.Distance));
        
        if (searchDto.Latitude < -90 || searchDto.Latitude > 90)
            throw new ArgumentException("Latitude must be between -90 and 90.", nameof(searchDto.Latitude));
        
        if (searchDto.Longitude < -180 || searchDto.Longitude > 180)
            throw new ArgumentException("Longitude must be between -180 and 180.", nameof(searchDto.Longitude));

        TourDifficulty? difficulty = null;
        if (!string.IsNullOrWhiteSpace(searchDto.Difficulty))
        {
            if (Enum.TryParse<TourDifficulty>(searchDto.Difficulty, true, out var parsedDifficulty))
            {
                difficulty = parsedDifficulty;
            }
        }

        var result = _tourRepository.SearchByLocation(
            searchDto.Latitude,
            searchDto.Longitude,
            searchDto.Distance,
            difficulty,
            searchDto.MinPrice,
            searchDto.MaxPrice,
            searchDto.Tags,
            searchDto.SortBy,
            searchDto.SortOrder,
            page,
            pageSize
        );

        var items = result.Results.Select(_mapper.Map<TourDto>).ToList();
        return new PagedResult<TourDto>(items, result.TotalCount);
    }
    
    public List<string> GetAllTags()
    {
        var result = _tourRepository.GetAll();
        var items = result.SelectMany(t => t.Tags).Distinct().ToList();
        return items;
    }

    private string? SaveKeyPointImage(IFormFile? image, long authorId)
    {
        if (image == null) return null;

        using var ms = new MemoryStream();
        image.CopyTo(ms);

        return _imageStorage.SaveImage(
            "keypoints",
            authorId,
            ms.ToArray(),
            image.ContentType
        );
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

        var imagePath = SaveKeyPointImage(keyPointDto.ImagePath, tour.AuthorId);

        tour.AddKeyPoint(
            keyPointDto.Name,
            keyPointDto.Description,
            location,
            imagePath,
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

        var keyPoint = tour.KeyPoints.FirstOrDefault(kp => kp.Id == keyPointId);
        if (keyPoint == null)
            throw new InvalidOperationException("KeyPoint not found");

        string? imagePath = keyPoint.ImagePath;
        if (keyPointDto.ImagePath != null)
        {
            imagePath = SaveKeyPointImage(keyPointDto.ImagePath, tour.AuthorId);
        }

        tour.UpdateKeyPoint(
            keyPointId,
            keyPointDto.Name,
            keyPointDto.Description,
            imagePath,
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
        
        _badgeService.OnTourPublished(tour.AuthorId);
        
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
        var purchaseToken = _purchaseTokenService.GetByTourAndTourist(tourId, userId);
        if (purchaseToken == null)
            throw new InvalidOperationException("User has not purchased this tour.");

        var tour = _tourRepository.Get(tourId);
        var execution = _tourExecutionRepository.GetActiveOrCompletedForUser(userId, tourId);

        var timeSinceLastActivity = DateTime.UtcNow - execution.LastActivity;
        if (timeSinceLastActivity > TimeSpan.FromDays(7))
            throw new Exception("Too much time has passed since your last activity on this tour.");

        int total = tour.KeyPoints.Count;
        int completed = execution.CompletedKeyPoints.Count;
        var progress = total == 0 ? 0 : 100.0 * completed / total;

        List<ReviewImage>? images = null;
        if (dto.Images != null && dto.Images.Any())
        {
        images = dto.Images
            .OrderBy(i => i.Order)
            .Select((img, index) =>
                new ReviewImage(0, Convert.FromBase64String(img.Data), img.ContentType, index))
            .ToList();
        }

        // Koristi repozitorijumovu AddReview i prosledi slike
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
                    new ReviewImage( reviewId, Convert.FromBase64String(img.Data), img.ContentType, index))
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

        var purchaseToken = _purchaseTokenService.GetByTourAndTourist(tourId, userId);
        if (purchaseToken == null)
            return 0;

        var execution = _tourExecutionRepository.GetActiveOrCompletedForUser(userId, tourId);
        if (execution == null || DateTime.UtcNow - execution.LastActivity > TimeSpan.FromDays(7))
            return 0;

        var tour = _tourRepository.Get(tourId);
        
        int total = tour.KeyPoints.Count;
        int completed = execution.CompletedKeyPoints.Count;
        double progress = total == 0 ? 100 : 100.0 * completed / total;
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

    public TourDto Get(long id)
    {
        var tour = _tourRepository.Get(id);
        return _mapper.Map<TourDto>(tour);
    }
    public TourDto UploadThumbnail(long tourId, IFormFile file)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("Image file is required");

        var tour = _tourRepository.Get(tourId);
        if (tour == null)
            throw new KeyNotFoundException("Tour not found");

        using var ms = new MemoryStream();
        file.CopyTo(ms);

        var imagePath = _imageStorage.SaveImage(
            "tours",
            tourId,
            ms.ToArray(),
            file.ContentType);

        tour.SetThumbnail(imagePath);

        var updated = _tourRepository.Update(tour);
        return _mapper.Map<TourDto>(updated);
    }

    public bool CanEditTour(long tourId, long userId)
    {
        var tour = _tourRepository.Get(tourId);
        if (tour == null) return false;

        return tour.AuthorId == userId;
    }

}
