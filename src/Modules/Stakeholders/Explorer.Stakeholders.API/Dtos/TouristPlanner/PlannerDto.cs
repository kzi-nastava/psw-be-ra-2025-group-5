
namespace Explorer.Stakeholders.API.Dtos.TouristPlanner;

public class PlannerDto
{
    public long Id { get; set; }
    public long TouristId { get; set; }
    public List<PlannerDayDto> Days { get; set; } = [];
}
