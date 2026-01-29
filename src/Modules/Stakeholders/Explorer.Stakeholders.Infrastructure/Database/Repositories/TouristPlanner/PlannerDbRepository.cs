using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces.TouristPlanner;
using Explorer.Stakeholders.Core.Domain.TouristPlanner;
using Microsoft.EntityFrameworkCore;

namespace Explorer.Stakeholders.Infrastructure.Database.Repositories.TouristPlanner;

public class PlannerDbRepository : IPlannerRepository
{
    protected readonly StakeholdersContext DbContext;
    private readonly DbSet<Planner> _dbSet;

    public PlannerDbRepository(StakeholdersContext dbContext)
    {
        DbContext = dbContext;
        _dbSet = DbContext.Set<Planner>();
    }

    public Planner Get(long id)
    {
        var planner = _dbSet.Include(p => p.Days).ThenInclude(d => d.TimeBlocks).SingleOrDefault(p => p.Id == id);
        OrderPlannerBlocks(planner);
        return planner;
    }

    public Planner GetByTouristId(long touristId)
    {
        var planner = _dbSet.Include(p => p.Days).ThenInclude(d => d.TimeBlocks).SingleOrDefault(p => p.TouristId == touristId);
        OrderPlannerBlocks(planner);
        return planner;
    }

    public Planner Create(long touristId)
    {
        var entity = new Planner(touristId);
        _dbSet.Add(entity);
        DbContext.SaveChanges();
        return entity;
    }

    public Planner Update(Planner planner)
    {
        _dbSet.Update(planner);
        DbContext.SaveChanges();
        return planner;
    }

    private static void OrderPlannerBlocks(Planner planner)
    {
        if (planner == null) return;

        foreach (var day in planner.Days)
            day.TimeBlocks = [.. day.TimeBlocks.OrderBy(b => b.TimeRange.Start)];
    }
}
