using Explorer.BuildingBlocks.Core.Domain;
using Explorer.Tours.Core.Domain.Shared;

namespace Explorer.Tours.Core.Domain;

public enum TourStatus { Draft, Published, Archived, Closed };
public enum TourDifficulty { Easy, Medium, Hard };

public class Tour : Entity
{
    public int AuthorId { get; init; }
    public string Name { get; init; }
    public string? Description { get; init; }
    public TourDifficulty Difficulty { get; init; }
    public List<string> Tags { get; init; }
    public double Price { get; init; }
    public TourStatus Status { get; init; }

    public Tour(int authorId, string name, string? description, TourDifficulty difficulty, List<string> tags, double price = 0.0, TourStatus status = TourStatus.Draft)
    {
        Guard.AgainstNull(authorId, nameof(authorId));
        Guard.AgainstNegative(price, nameof(price));
        Guard.AgainstNullOrWhiteSpace(name, nameof(name));
        Guard.AgainstInvalidEnum(difficulty, nameof(difficulty));
        Guard.AgainstInvalidEnum(status, nameof(status));
        Guard.AgainstDuplicateStrings(tags, nameof(tags));

        AuthorId = authorId;
        Name = name;
        Description = description;
        Difficulty = difficulty;
        Tags = tags;
        Price = price;
        Status = status;
    }
}
