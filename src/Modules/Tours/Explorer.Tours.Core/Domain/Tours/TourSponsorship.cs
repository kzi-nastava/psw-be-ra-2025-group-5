using Explorer.BuildingBlocks.Core.Domain;

namespace Explorer.Tours.Core.Domain.Tours;

public class TourSponsorship : Entity
{
    public long TourId { get; private set; }
    public long AuthorId { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public int DurationDays { get; private set; }
    public double Price { get; private set; }

    private TourSponsorship() { }

    public TourSponsorship(long tourId, long authorId, int durationDays, double price)
    {
        if (tourId <= 0) throw new ArgumentException("TourId must be positive");
        if (authorId <= 0) throw new ArgumentException("AuthorId must be positive");
        if (durationDays <= 0) throw new ArgumentException("DurationDays must be positive");
        if (price <= 0) throw new ArgumentException("Price must be positive");

        TourId = tourId;
        AuthorId = authorId;
        DurationDays = durationDays;
        Price = price;
        StartDate = DateTime.UtcNow;
        EndDate = StartDate.AddDays(durationDays);
    }

    public bool IsActive => EndDate > DateTime.UtcNow;
}
