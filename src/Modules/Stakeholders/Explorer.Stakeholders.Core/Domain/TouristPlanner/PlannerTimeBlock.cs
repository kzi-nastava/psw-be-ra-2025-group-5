using Explorer.BuildingBlocks.Core.Domain;
using Explorer.Stakeholders.Core.Domain.Shared;

namespace Explorer.Stakeholders.Core.Domain.TouristPlanner;

public class PlannerTimeBlock : Entity
{
    public long TourId { get; private set; }
    public TimeRange TimeRange { get; internal set; }

    public long PlannerDayId { get; set; }

    private PlannerTimeBlock() { }

    public PlannerTimeBlock(long tourId, TimeOnly startTime, TimeOnly endTime)
    {
        Guard.AgainstNull(tourId, nameof(tourId));

        TourId = tourId;
        TimeRange = new TimeRange(startTime, endTime);
    }

    public void Reschedule(TimeRange newRange)
    {
        TimeRange = newRange;
    }
}
