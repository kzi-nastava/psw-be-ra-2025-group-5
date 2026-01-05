using Explorer.Stakeholders.API.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.API.Public
{
    public interface IClubJoinRequestService
    {
        void RequestToJoin(long clubId, long touristId);
        void CancelRequest(long clubId, long touristId);
        string GetMembershipStatus(long clubId, long touristId);

        void AcceptRequest(long clubId, long touristId, long ownerId);
        void RejectRequest(long clubId, long touristId, long ownerId);
        List<ClubJoinRequestDto> GetPendingRequests(long clubId, long ownerId);
    }

}
