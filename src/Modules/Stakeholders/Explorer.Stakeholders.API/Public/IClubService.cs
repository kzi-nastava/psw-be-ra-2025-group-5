using Explorer.Stakeholders.API.Dtos;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.API.Public
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
