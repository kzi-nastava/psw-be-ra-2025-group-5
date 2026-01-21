
namespace Explorer.Stakeholders.API.Dtos.TouristPlanner;

public class PlannerTimeBlockDto
{
    public long Id { get; set; }
    public long TourId { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public int Order { get; set; }
}
