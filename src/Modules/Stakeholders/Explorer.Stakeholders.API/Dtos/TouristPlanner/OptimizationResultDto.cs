namespace Explorer.Stakeholders.API.Dtos.TouristPlanner;

public class OptimizationResultDto
{
    public bool Success { get; set; }
    public PlannerDto Planner { get; set; } = new();
    public List<string> UnresolvedWarnings { get; set; } = [];
    public List<OptimizationActionDto> ActionsPerformed { get; set; } = [];
}

public class OptimizationActionDto
{
    public string ActionType { get; set; } = string.Empty;
    public long BlockId { get; set; }
    public string Description { get; set; } = string.Empty;
}
