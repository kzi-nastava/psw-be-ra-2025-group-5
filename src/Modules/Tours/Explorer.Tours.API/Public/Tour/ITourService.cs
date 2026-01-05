using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos.KeyPoints;
using Explorer.Tours.API.Dtos.Tours;
using Explorer.Tours.API.Dtos.Tours.Reviews;

namespace Explorer.Tours.API.Public.Tour;

public interface ITourService
{
    PagedResult<TourDto> GetPaged(int page, int pageSize);
    PagedResult<TourDto> GetPagedByAuthor(long authorId, int page, int pageSize);
    List<TourDto> GetMultiple(long[] ids);
    List<string> GetAllTags();
    TourDto Create(CreateTourDto tour);
    TourDto Update(long id, UpdateTourDto tour);
    void Delete(long id);

    // KeyPoint operacije
    TourDto AddKeyPoint(long tourId, CreateKeyPointDto keyPoint);
    TourDto UpdateKeyPoint(long tourId, long keyPointId, CreateKeyPointDto keyPoint);
    TourDto RemoveKeyPoint(long tourId, long keyPointId, double tourLength);
    TourDto ReorderKeyPoints(long tourId, ReorderKeyPointsDto reorderDto);

    // Status operacije
    TourDto Publish(long tourId);
    TourDto Archive(long tourId);
    TourDto Reactivate(long tourId);

    // For tourists
    PagedResult<TourDto> GetPagedPublished(int page, int pageSize);
    TourDto GetPublished(long id);
    KeyPointDto GetKeyPoint(long tourId, long keyPointId);

    // Review 
    TourDto AddReview(long tourId, long userId, string username, TourReviewDto dto);
    public TourDto UpdateReview(long tourId, long userId, long reviewId, TourReviewDto dto);
    TourDto RemoveReview(long tourId, long reviewId);
    public int GetReviewButtonState(long tourId, long userId);

    // Duration operacije
    TourDto AddDuration(long tourId, TourDurationDto durationDto);
    TourDto RemoveDuration(long tourId, TourDurationDto durationDto);
    TourDto AddRequiredEquipment(long tourId, long equipmentId);
    TourDto RemoveRequiredEquipment(long tourId, long equipmentId);
    void CloseTour(long tourId);
    TourDto Get(long id);
}