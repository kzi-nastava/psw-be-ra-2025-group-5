using Explorer.Payments.API.Internal;
using Explorer.Tours.API.Dtos.Tours;
using Explorer.Tours.API.Public.Tour;
using Explorer.Tours.Core.Domain.RepositoryInterfaces.Tours;
using Explorer.Tours.Core.Domain.Tours;

namespace Explorer.Tours.Core.UseCases.Tours;

public class TourSponsorshipService : ITourSponsorshipService
{
    private static readonly Dictionary<int, double> SponsorshipPricing = new()
    {
        { 7, 50 },
        { 14, 100 },
        { 30, 200 }
    };

    private readonly ITourSponsorshipRepository _sponsorshipRepository;
    private readonly ITourRepository _tourRepository;
    private readonly IInternalWalletService _walletService;

    public TourSponsorshipService(
        ITourSponsorshipRepository sponsorshipRepository,
        ITourRepository tourRepository,
        IInternalWalletService walletService)
    {
        _sponsorshipRepository = sponsorshipRepository;
        _tourRepository = tourRepository;
        _walletService = walletService;
    }

    public TourSponsorshipDto PurchaseSponsorship(long tourId, long authorId, CreateTourSponsorshipDto dto)
    {
        if (!SponsorshipPricing.TryGetValue(dto.DurationDays, out var price))
            throw new ArgumentException($"Invalid duration. Allowed values: {string.Join(", ", SponsorshipPricing.Keys)}");

        var tour = _tourRepository.Get(tourId);

        if (tour.AuthorId != authorId)
            throw new UnauthorizedAccessException("You can only sponsor your own tours.");

        if (tour.Status != TourStatus.Published)
            throw new InvalidOperationException("Only published tours can be sponsored.");

        double balance = _walletService.GetWalletBalance(authorId);
        if (balance < price)
            throw new InvalidOperationException("Insufficient balance in wallet.");

        _walletService.DebitWallet(authorId, price);

        var sponsorship = new TourSponsorship(tourId, authorId, dto.DurationDays, price);
        var created = _sponsorshipRepository.Create(sponsorship);

        return new TourSponsorshipDto
        {
            Id = created.Id,
            TourId = created.TourId,
            AuthorId = created.AuthorId,
            StartDate = created.StartDate,
            EndDate = created.EndDate,
            DurationDays = created.DurationDays,
            Price = created.Price,
            IsActive = created.IsActive
        };
    }

    public TourSponsorshipDto? GetSponsorshipStatus(long tourId)
    {
        var sponsorship = _sponsorshipRepository.GetActiveByTourId(tourId);
        if (sponsorship == null) return null;

        return new TourSponsorshipDto
        {
            Id = sponsorship.Id,
            TourId = sponsorship.TourId,
            AuthorId = sponsorship.AuthorId,
            StartDate = sponsorship.StartDate,
            EndDate = sponsorship.EndDate,
            DurationDays = sponsorship.DurationDays,
            Price = sponsorship.Price,
            IsActive = sponsorship.IsActive
        };
    }

    public List<TourSponsorshipDto> GetSponsorshipHistory(long authorId)
    {
        return _sponsorshipRepository.GetByAuthorId(authorId).Select(s => new TourSponsorshipDto
        {
            Id = s.Id,
            TourId = s.TourId,
            AuthorId = s.AuthorId,
            StartDate = s.StartDate,
            EndDate = s.EndDate,
            DurationDays = s.DurationDays,
            Price = s.Price,
            IsActive = s.IsActive
        }).ToList();
    }
}
