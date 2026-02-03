using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.Stakeholders.API.Dtos.Social;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Explorer.Stakeholders.API.Internal
{
    public interface IInternalProfileFollowService
    {
        Task<IEnumerable<FollowerDto>> GetFollowers(long profileId);
        Task<IEnumerable<FollowingDto>> GetFollowing(long profileId);
    }
}
