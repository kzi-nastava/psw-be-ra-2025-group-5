using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.API.Public.Users
{
    public interface IPremiumService
    {
        void GrantPremium(long userId, DateTime validUntil);
        void ExtendPremium(long userId, DateTime newUntil);
        void RemovePremium(long userId);
        bool IsPremium(long userId);
    }
}
