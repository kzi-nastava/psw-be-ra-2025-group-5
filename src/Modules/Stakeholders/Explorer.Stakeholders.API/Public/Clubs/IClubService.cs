using Explorer.Stakeholders.API.Dtos.Clubs;
using Explorer.Stakeholders.API.Dtos.Users;
using Microsoft.AspNetCore.Http;

namespace Explorer.Stakeholders.API.Public.Clubs
{
    public interface IClubService
    {
        ClubDto Create(ClubDto clubDto, List<IFormFile> images);
        void Delete(long userId, long id);
        ClubDto GetById(long id);
        List<ClubDto> GetAll();
        ClubDto Update(ClubDto clubDto, List<IFormFile>? images);
        ClubDto RemoveImage(long userId, long clubId, string imagePath);
        void CloseClub(long clubId, long ownerId);
        void RemoveMember(long clubId, long ownerId, long memberId);
        List<UserDto> GetClubMembers(long clubId, long ownerId);

    }
}
