using Explorer.BuildingBlocks.Core.Exceptions;
using Explorer.Encounters.Core.Domain;
using Explorer.Encounters.Core.Domain.RepositoryInterfaces;
using Google.GenAI.Types;
using Microsoft.EntityFrameworkCore;

namespace Explorer.Encounters.Infrastructure.Database.Repositories;

public class ChallengeExecutionDbRepository : IChallengeExecutionRepository
{
    private readonly EncountersContext _dbContext;
    private readonly DbSet<ChallengeExecution> _dbSet;

    public ChallengeExecutionDbRepository(EncountersContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<ChallengeExecution>();
    }

    public ChallengeExecution Create(ChallengeExecution execution)
    {
        _dbSet.Add(execution);
        _dbContext.SaveChanges();
        return execution;
    }

    public ChallengeExecution Update(ChallengeExecution execution)
    {
        try
        {
            _dbContext.Update(execution);
            _dbContext.SaveChanges();
            return execution;
        }
        catch (DbUpdateException e)
        {
            throw new NotFoundException(e.Message);
        }
    }

    public ChallengeExecution Get(long id)
    {
        var entity = _dbSet.Find(id);
        if (entity == null) throw new NotFoundException("Execution not found: " + id);
        return entity;
    }

    public List<ChallengeExecution> GetByTourist(long touristId)
    {
        return _dbSet.Where(e => e.TouristId == touristId).ToList();
    }

    public ChallengeExecution? GetActiveByChallengeAndTourist(long challengeId, long touristId)
    {
        return _dbSet.FirstOrDefault(e => 
            e.ChallengeId == challengeId && 
            e.TouristId == touristId && 
            e.Status == ChallengeExecutionStatus.InProgress);
    }

    public ChallengeExecution? GetCompletedByChallengeAndTourist(long challengeId, long touristId)
    {
        return _dbSet.FirstOrDefault(e => 
            e.ChallengeId == challengeId && 
            e.TouristId == touristId && 
            e.Status == ChallengeExecutionStatus.Completed);
    }

    public List<ChallengeExecution>? GetTodayParticipants(long challengeId, DateTime now)
    {
        var today = now.Date;
        var tomorrow = today.AddDays(1);

        return _dbSet.Where(e => e.ChallengeId == challengeId && e.Status == ChallengeExecutionStatus.Completed && e.CompletedAt >= today && e.CompletedAt < tomorrow).ToList();

    }

    public List<ChallengeExecution> GetByChallenge(long challengeId)
    {
        return _dbContext.ChallengeExecutions
                       .Where(e => e.ChallengeId == challengeId)
                       .ToList();
    }

}
