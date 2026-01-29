using Explorer.BuildingBlocks.Core.Domain;
using Explorer.Stakeholders.Core.Domain.Shared;

namespace Explorer.Stakeholders.Core.Domain.Diaries;

public class Diary : Entity
{
    public string Name { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public string Content { get; private set; }
    public string Country { get; private set; }
    public string? City { get; private set; }
    public long TouristId { get; private set; }

    public Diary(string name, DateTime createdAt, string content, string country, string? city, long touristId)
    {
        Guard.AgainstNull(touristId, nameof(touristId));

        Name = name;
        CreatedAt = createdAt;
        Content = content;
        Country = country;
        City = string.IsNullOrWhiteSpace(city) ? null : city;
        TouristId = touristId;
    }
}
