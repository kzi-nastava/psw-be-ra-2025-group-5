using Explorer.BuildingBlocks.Core.Domain;
using System.Text.Json.Serialization;

namespace Explorer.Stakeholders.Core.Domain.TouristPlanner;

public class TimeRange : ValueObject
{
    public TimeOnly Start { get; }
    public TimeOnly End { get; }

    [JsonConstructor]
    public TimeRange(TimeOnly start, TimeOnly end)
    {
        if (end != TimeOnly.MinValue && end <= start) throw new ArgumentException("End time must be after start time.");

        Start = start;
        End = end;
    }

    public bool OverlapsWith(TimeRange other)
    {
        return Start < other.End && End > other.Start;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Start;
        yield return End;
    }
}
