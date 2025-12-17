using Explorer.BuildingBlocks.Core.Domain;

namespace Explorer.Stakeholders.Core.Domain;

public enum DiaryStatus
{
    Draft = 0,
    Published = 1
}

public class Diary : Entity
{
    public string Name { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DiaryStatus Status { get; private set; }
    public string Country { get; private set; }
    public string? City { get; private set; }
    public long TouristId { get; private set; }

    public Diary(string name, DateTime createdAt, DiaryStatus status, string country, string? city, long touristId)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Diary name cannot be empty.");

        if (string.IsNullOrWhiteSpace(country))
            throw new ArgumentException("Country cannot be empty.");

        //if (touristId <= 0)
        //    throw new ArgumentException("Invalid Tourist ID.");

        Name = name;
        CreatedAt = createdAt;
        Status = status;
        Country = country;
        City = string.IsNullOrWhiteSpace(city) ? null : city;
        TouristId = touristId;
    }
}
