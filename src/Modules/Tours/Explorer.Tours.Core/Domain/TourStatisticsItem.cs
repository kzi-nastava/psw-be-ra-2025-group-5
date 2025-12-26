using Explorer.Tours.Core.Domain.Tours;

namespace Explorer.Tours.Core.Domain;

public class TourStatisticsItem
{
    public TourDifficulty Difficulty { get; }
    public IReadOnlyCollection<string> Tags { get; }

    private TourStatisticsItem() {}

    public TourStatisticsItem(TourDifficulty difficulty, IReadOnlyCollection<string> tags)
    {
        Difficulty = difficulty;
        Tags = tags;
    }
}
