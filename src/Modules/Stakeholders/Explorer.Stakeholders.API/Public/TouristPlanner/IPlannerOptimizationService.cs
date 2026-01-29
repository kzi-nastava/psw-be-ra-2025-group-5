using Explorer.Stakeholders.API.Dtos.TouristPlanner;

namespace Explorer.Stakeholders.API.Public.TouristPlanner
{
    public interface IPlannerOptimizationService
    {
        OptimizationResultDto OptimizePlanner(long touristId);
    }
}
