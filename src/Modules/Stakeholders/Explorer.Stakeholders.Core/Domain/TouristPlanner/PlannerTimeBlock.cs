using Explorer.BuildingBlocks.Core.Domain;
using Explorer.Stakeholders.API.Dtos.TouristPlanner;
using Explorer.Stakeholders.Core.Domain.Shared;
using System.ComponentModel.DataAnnotations.Schema;

namespace Explorer.Stakeholders.Core.Domain.TouristPlanner;

public enum TransportType
{
    Walking,
    Bicycle,
    Car
}

public class PlannerTimeBlock : Entity
{
    public long TourId { get; private set; }
    public TimeRange TimeRange { get; internal set; }
    public long PlannerDayId { get; set; }
    public TransportType TransportType { get; private set; }

    private PlannerTimeBlock() { }

    public PlannerTimeBlock(long tourId, TimeOnly startTime, TimeOnly endTime, TransportType transportType)
    {
        Guard.AgainstNull(tourId, nameof(tourId));

        TourId = tourId; 
        TimeRange = new TimeRange(startTime, endTime);
        TransportType = transportType;
    }

    public void Reschedule(TimeRange newRange, TransportType transportType)
    {
        TimeRange = newRange;
        TransportType = transportType;
    }
}
