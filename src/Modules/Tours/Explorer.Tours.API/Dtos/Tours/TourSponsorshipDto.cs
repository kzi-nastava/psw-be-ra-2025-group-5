namespace Explorer.Tours.API.Dtos.Tours;

public class TourSponsorshipDto
{
    public long Id { get; set; }
    public long TourId { get; set; }
    public long AuthorId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int DurationDays { get; set; }
    public double Price { get; set; }
    public bool IsActive { get; set; }
}
