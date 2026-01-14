
namespace Explorer.Stakeholders.API.Dtos.Social;

public class FollowerDto
{
    public long FollowerId { get; set; }
    public string FollowerName { get; set; }
}

public class FollowingDto
{
    public long FollowingId { get; set; }
    public string FollowingName { get; set; }
}
