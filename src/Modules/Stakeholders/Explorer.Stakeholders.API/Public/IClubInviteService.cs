using Explorer.Stakeholders.API.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.API.Public
{
    public interface IClubInviteService
    {
        void InviteTourist(long clubId, long touristId, long ownerId);
        void AcceptInvite(long inviteId, long touristId);
        void RejectInvite(long inviteId, long touristId);
        List<ClubInviteDto> GetInvitesForClub(long clubId, long ownerId);
        List<UserDto> GetAvailableTourists(long clubId, long ownerId);
        ClubInviteDto? GetInviteForUserAndClub(long clubId, long touristId);


    }
}
