using Explorer.BuildingBlocks.Core.Domain;

namespace Explorer.Stakeholders.Core.Domain.Planner;

public class TimeRange : ValueObject
{
    public TimeOnly Start { get; private set; }
    public TimeOnly End { get; private set; }

    private TimeRange() { }

    public TimeRange(TimeOnly start, TimeOnly end)
    {
        if (end <= start) throw new ArgumentException("End time must be after start time.");

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
