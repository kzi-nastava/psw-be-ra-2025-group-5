using Explorer.Tours.Core.Domain.Tours;

namespace Explorer.Tours.Core.Domain.RepositoryInterfaces.Tours;

public interface ITourSponsorshipRepository
{
    TourSponsorship Create(TourSponsorship sponsorship);
    TourSponsorship? GetActiveByTourId(long tourId);
    List<long> GetActiveSponsoredTourIds();
    List<TourSponsorship> GetByAuthorId(long authorId);
}
