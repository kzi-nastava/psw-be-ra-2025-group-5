
namespace Explorer.Stakeholders.API.Dtos.TouristPlanner;

public class PlannerDayDto
{
    public long Id { get; set; }
    public DateOnly Date { get; set; }
    public List<PlannerTimeBlockDto> TimeBlocks { get; set; } = [];
    public List<PlannerWarningDto> Warnings { get; set; } = new();
}
