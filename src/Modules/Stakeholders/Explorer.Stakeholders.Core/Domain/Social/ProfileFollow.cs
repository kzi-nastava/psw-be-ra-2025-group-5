using Explorer.Stakeholders.Core.Domain.Shared;
using Explorer.Stakeholders.Core.Domain.Users;

namespace Explorer.Stakeholders.Core.Domain.Social;

public class ProfileFollow
{
    public long FollowerId { get; set; }
    public Person Follower { get; set; }

    public long FollowingId { get; set; }
    public Person Following { get; set; }

    private ProfileFollow() { }

    public ProfileFollow(long followerId, long followedId)
    {
        Validate(followerId, followedId);

        FollowerId = followerId;
        FollowingId = followedId;
    }

    private static void Validate(long followerId, long followedId)
    {
        Guard.AgainstNull(followerId, nameof(followerId));
        Guard.AgainstNull(followedId, nameof(followedId));
        if (followerId == followedId) throw new ArgumentException("A user cannot follow themselves.");
    }
}
