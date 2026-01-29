using Explorer.Stakeholders.Core.Domain.TouristPlanner;

namespace Explorer.Stakeholders.Core.Domain.RepositoryInterfaces.TouristPlanner;

public interface IPlannerRepository
{
    Planner Get(long id);
    Planner GetByTouristId(long touristId);
    Planner Create(long touristId);
    Planner Update(Planner planner);
}
