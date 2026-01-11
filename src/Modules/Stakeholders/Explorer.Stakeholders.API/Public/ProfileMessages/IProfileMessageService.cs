using Explorer.Stakeholders.API.Dtos.ProfileMessages;

namespace Explorer.Stakeholders.API.Public.ProfileMessages
{
    public interface IProfileMessageService
    {
        ProfileMessageDto Create(long receiverId, long authorId, ProfileMessageDto dto);
        ProfileMessageDto Update(long messageId, long authorId, ProfileMessageDto dto);
        void Delete(long messageId, long userId);
        ProfileMessageDto GetById(long id);
        List<ProfileMessageDto> GetByReceiverId(long receiverId);
    }
}
