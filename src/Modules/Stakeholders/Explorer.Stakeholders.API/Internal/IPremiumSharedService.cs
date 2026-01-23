using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.API.Internal
{
    public interface IPremiumSharedService
    {
        void GrantPremium(long userId, DateTime validUntil);
        void ExtendPremium(long userId, DateTime validUntil);
        bool IsPremium(long userId);
    }
}

