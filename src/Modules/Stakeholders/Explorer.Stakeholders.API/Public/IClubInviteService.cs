using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Dtos.Users;

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
