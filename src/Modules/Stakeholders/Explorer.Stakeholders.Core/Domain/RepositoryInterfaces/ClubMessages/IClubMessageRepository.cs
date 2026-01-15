using Explorer.Stakeholders.Core.Domain.ClubMessages;
using System.Collections.Generic;

namespace Explorer.Stakeholders.Core.Domain.RepositoryInterfaces.ClubMessages
{
    public interface IClubMessageRepository
    {
        ClubMessage Create(ClubMessage message);
        ClubMessage Update(ClubMessage message);
        void Delete(long id);
        ClubMessage GetById(long id);
        List<ClubMessage> GetByClubId(long clubId);
    }
}

