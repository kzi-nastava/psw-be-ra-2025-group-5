using Explorer.Stakeholders.Core.Domain.ProfileMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Core.Domain.RepositoryInterfaces.ProfileMessages
{
    public interface IProfileMessageRepository
    {
        ProfileMessage Create(ProfileMessage message);
        ProfileMessage Update(ProfileMessage message);
        void Delete(long id);
        ProfileMessage? GetById(long id);
        List<ProfileMessage> GetByReceiverId(long receiverId);
    }
}
