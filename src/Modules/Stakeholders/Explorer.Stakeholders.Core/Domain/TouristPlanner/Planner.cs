using Explorer.BuildingBlocks.Core.Domain;
using Explorer.Stakeholders.Core.Domain.Shared;

namespace Explorer.Stakeholders.Core.Domain.TouristPlanner;
public class Planner : AggregateRoot
{
    public long TouristId { get; private set; }
    public List<PlannerDay> Days = [];

    private Planner() { }

    public Planner(long touristId)
    {
        Guard.AgainstNull(touristId, nameof(touristId));

        TouristId = touristId;
    }

    public void AddDay(PlannerDay day)
    {
        if (Days.Any(d => d.Date == day.Date)) throw new InvalidDataException("A planner day for this date already exists.");
        Days.Add(day);
    }
}
