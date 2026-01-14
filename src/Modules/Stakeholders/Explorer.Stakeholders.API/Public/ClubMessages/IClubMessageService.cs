using Explorer.Stakeholders.API.Dtos.ClubMessages;
using System.Collections.Generic;

namespace Explorer.Stakeholders.API.Public.ClubMessages
{
    public interface IClubMessageService
    {
        ClubMessageDto Create(long clubId, long authorId, CreateClubMessageDto dto);
        ClubMessageDto Update(long messageId, long authorId, UpdateClubMessageDto dto);
        void Delete(long messageId, long userId, bool isOwner);
        ClubMessageDto GetById(long id);
        List<ClubMessageDto> GetByClubId(long clubId);
    }
}
