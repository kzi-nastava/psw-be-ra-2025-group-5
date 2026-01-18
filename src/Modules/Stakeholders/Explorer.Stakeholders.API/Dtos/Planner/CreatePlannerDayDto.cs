
namespace Explorer.Stakeholders.API.Dtos.Planner;

public class CreatePlannerDayDto
{
    public long TourId { get; init; }
    public TimeOnly StartTime { get; init; }
    public TimeOnly EndTime { get; init; }
}
