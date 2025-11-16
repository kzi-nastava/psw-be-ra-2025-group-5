using Explorer.BuildingBlocks.Core.Domain;

namespace Explorer.Tours.Core.Domain;

public enum TourStatus { Draft, Published, Archived };
public enum TourDifficulty { Easy, Medium, Hard };

public class Tour : Entity
{
    public string Name { get; init; }
    public string? Description { get; init; }
    public TourDifficulty Difficulty { get; init; }
    public List<string> Tags { get; init; }
    public double Price { get; init; }
    public TourStatus Status { get; init; }

    public Tour(string name, string? description, TourDifficulty difficulty, List<string> tags)
    {
        Name = name;
        Description = description;
        Difficulty = difficulty;
        Tags = tags;
        Price = 0.0;
        Status = TourStatus.Draft;
    }
}
