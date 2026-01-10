using Explorer.Stakeholders.Core.Domain.Social;

namespace Explorer.Stakeholders.Core.Domain.RepositoryInterfaces.Social;

public interface IProfileFollowRepository
{
    bool Exists(long followerId, long followingId);
    Task<IEnumerable<ProfileFollow>> GetFollowers(long profileId);
    Task<IEnumerable<ProfileFollow>> GetFollowing(long profileId);
    ProfileFollow Add(ProfileFollow follow);
    void Remove(ProfileFollow follow);
}
