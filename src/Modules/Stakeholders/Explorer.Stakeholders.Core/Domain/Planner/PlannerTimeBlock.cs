using Explorer.BuildingBlocks.Core.Domain;
using Explorer.Stakeholders.Core.Domain.Shared;

namespace Explorer.Stakeholders.Core.Domain.Planner;

public class PlannerTimeBlock : Entity
{
    public long TourId { get; private set; }
    public TimeRange TimeRange { get; private set; }
    public int Order { get; private set; }  // drag and drop sorting fallback

    private PlannerTimeBlock() { }

    public PlannerTimeBlock(long tourId, TimeOnly startTime, TimeOnly endTime, int order)
    {
        Guard.AgainstNull(tourId, nameof(tourId));

        TourId = tourId;
        TimeRange = new TimeRange(startTime, endTime);
        Order = order;
    }

    public void Reschedule(TimeRange newRange)
    {
        TimeRange = newRange;
    }
}
