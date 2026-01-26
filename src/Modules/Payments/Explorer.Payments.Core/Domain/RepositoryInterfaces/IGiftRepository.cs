using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payments.Core.Domain.RepositoryInterfaces
{
    public interface IGiftRepository
    {
        Gift Create(Gift gift);
        Gift GetById(long id);
        List<Gift> GetByDonor(long donorId);
        List<Gift> GetByRecipient(long recipientId);
    }
}
