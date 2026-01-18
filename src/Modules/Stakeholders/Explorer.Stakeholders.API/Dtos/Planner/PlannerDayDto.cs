
namespace Explorer.Stakeholders.API.Dtos.Planner;

public class PlannerDayDto
{
    public long Id { get; set; }
    public DateOnly Date { get; set; }
    public List<PlannerTimeBlockDto> TimeBlocks { get; set; } = [];
}
