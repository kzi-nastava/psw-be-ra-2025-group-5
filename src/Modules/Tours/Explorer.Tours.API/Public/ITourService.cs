using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;

namespace Explorer.Tours.API.Public;

public interface ITourService
{
    PagedResult<TourDto> GetPaged(int page, int pageSize);
    PagedResult<TourDto> GetPagedByAuthor(long authorId, int page, int pageSize);
    TourDto Get(long id);
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

    // Review operacije
    TourDto AddReview(long tourId, TourReviewDto dto);
    public TourDto UpdateReview(long tourId, long reviewId, TourReviewDto dto);
    TourDto RemoveReview(long tourId, long reviewId);

    // Duration operacije
    TourDto AddDuration(long tourId, TourDurationDto durationDto);
    TourDto RemoveDuration(long tourId, long durationId);
}