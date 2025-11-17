using Explorer.BuildingBlocks.Core.Domain;
using Explorer.Tours.Core.Domain.Shared;

namespace Explorer.Tours.Core.Domain;

public enum TourStatus { Draft, Published, Archived };
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

    public Tour(int authorId, string name, string? description, TourDifficulty difficulty, List<string> tags)
    {
        Guard.AgainstNull(authorId, nameof(authorId));
        Guard.AgainstNullOrWhiteSpace(name, nameof(name));
        Guard.AgainstInvalidEnum(difficulty, nameof(difficulty));
        Guard.AgainstDuplicateStrings(tags, nameof(tags));

        AuthorId = authorId;
        Name = name;
        Description = description;
        Difficulty = difficulty;
        Tags = tags;
        Price = 0.0;
        Status = TourStatus.Draft;
    }
}
