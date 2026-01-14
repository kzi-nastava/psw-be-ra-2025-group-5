using Explorer.Stakeholders.API.Dtos.Social;

namespace Explorer.Stakeholders.API.Public.Social;

public interface IProfileFollowService
{
    bool Exists(ProfileFollowDto follow);
    ProfileFollowDto Follow(ProfileFollowDto follow);
    void Unfollow(ProfileFollowDto follow);
    Task<IEnumerable<FollowerDto>> GetFollowers(long profileId);
    Task<IEnumerable<FollowingDto>> GetFollowing(long profileId);
}
