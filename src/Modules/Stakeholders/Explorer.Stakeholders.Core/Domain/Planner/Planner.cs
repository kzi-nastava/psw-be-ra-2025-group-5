using Explorer.BuildingBlocks.Core.Domain;
using Explorer.Stakeholders.Core.Domain.Shared;

namespace Explorer.Stakeholders.Core.Domain.Planner;

public class Planner : AggregateRoot
{
    public long TouristId { get; private set; }
    private readonly List<PlannerDay> _days = [];
    public IReadOnlyCollection<PlannerDay> Days => _days;

    private Planner() { }

    public Planner(long touristId)
    {
        Guard.AgainstNull(touristId, nameof(touristId));

        TouristId = touristId;
    }

    public void AddDay(PlannerDay day)
    {
        if (_days.Any(d => d.Date == day.Date)) throw new InvalidDataException("A planner day for this date already exists.");
        _days.Add(day);
    }
}
