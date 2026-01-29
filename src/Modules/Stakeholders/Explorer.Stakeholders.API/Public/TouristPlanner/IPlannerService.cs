using Explorer.Stakeholders.API.Dtos.TouristPlanner;

namespace Explorer.Stakeholders.API.Public.TouristPlanner
{
    public interface IPlannerService
    {
        public PlannerDto GetOrCreatePlanner(long touristId);
        public PlannerDayDto GetDay(long touristId, DateOnly date);
        public PlannerDayDto AddBlock(long touristId, DateOnly date, CreatePlannerTimeBlockDto dto);
        public PlannerDayDto RemoveBlock(long touristId, DateOnly date, long blockId);
        public PlannerDayDto RescheduleBlock(long touristId, DateOnly date, long blockId, CreatePlannerTimeBlockDto dto);
    }
}
