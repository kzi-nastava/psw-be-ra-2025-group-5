using Explorer.BuildingBlocks.Core.Exceptions;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces.Social;
using Explorer.Stakeholders.Core.Domain.Social;
using Microsoft.EntityFrameworkCore;

namespace Explorer.Stakeholders.Infrastructure.Database.Repositories.Social;

public class ProfileFollowDbRepository : IProfileFollowRepository
{
    protected readonly StakeholdersContext DbContext;
    private readonly DbSet<ProfileFollow> _dbSet;

    public ProfileFollowDbRepository(StakeholdersContext dbContext)
    {
        DbContext = dbContext;
        _dbSet = DbContext.Set<ProfileFollow>();
    }

    public bool Exists(long followerId, long followingId) => _dbSet.Any(f => f.FollowerId == followerId && f.FollowingId == followingId);

    public async Task<IEnumerable<ProfileFollow>> GetFollowers(long profileId)
    {
        return await _dbSet
            .Where(f => f.FollowingId == profileId)
            .Include(f => f.Follower)
            .ToListAsync();
    }

    public async Task<IEnumerable<ProfileFollow>> GetFollowing(long profileId)
    {
        return await _dbSet
            .Where(f => f.FollowerId == profileId)
            .Include(f => f.Following)
            .ToListAsync();
    }

    public ProfileFollow Add(ProfileFollow follow)
    {
        _dbSet.Add(follow);
        DbContext.SaveChanges();
        return follow;
    }

    public void Remove(ProfileFollow follow)
    {
        var entity = _dbSet.SingleOrDefault(f => f.FollowerId == follow.FollowerId && f.FollowingId == follow.FollowingId) ?? throw new NotFoundException("Follow relationship does not exist.");
        _dbSet.Remove(entity);
        DbContext.SaveChanges();
    }
}
