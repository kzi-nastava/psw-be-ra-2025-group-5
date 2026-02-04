using Explorer.Tours.API.Dtos.Tours;

namespace Explorer.Tours.API.Public.Tour;

public interface ITourSponsorshipService
{
    TourSponsorshipDto PurchaseSponsorship(long tourId, long authorId, CreateTourSponsorshipDto dto);
    TourSponsorshipDto? GetSponsorshipStatus(long tourId);
    List<TourSponsorshipDto> GetSponsorshipHistory(long authorId);
}
