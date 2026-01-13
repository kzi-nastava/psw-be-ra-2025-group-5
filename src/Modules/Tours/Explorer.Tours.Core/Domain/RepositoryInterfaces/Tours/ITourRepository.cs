using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.Core.Domain.Tours;
using Explorer.Tours.Core.Domain.Tours.Entities;

namespace Explorer.Tours.Core.Domain.RepositoryInterfaces.Tours;

public interface ITourRepository
{
    PagedResult<Tour> GetPaged(int page, int pageSize);
    PagedResult<Tour> GetPagedByAuthor(long authorId, int page, int pageSize);
    PagedResult<Tour> SearchByLocation(double latitude, double longitude, double distanceKm, int page, int pageSize);
    List<Tour> GetAll();
    Tour Get(long id);
    Tour Create(Tour map);
    Tour Update(Tour map);
    void Delete(long id);
    PagedResult<Tour> GetPagedByStatus(TourStatus status, int page, int pageSize);
    // metode za review
    TourReview AddReview(long tourId, int grade, string? comment, DateTime? reviewTime, double progress, long touristId, string username, List<ReviewImage>? images = null);
    void UpdateReview(long tourId, long reviewId, int grade, string? comment, double progress, List<ReviewImage>? images = null);
    void RemoveReview(long tourId, long reviewId);
    void Close(long tourId);
}