using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payments.API.Internal
{
    public interface IInternalWalletService
    {
        void CreateWalletForPerson(long personId);
        void DebitWallet(long userId, double amount);
        double GetWalletBalance(long userId);
    }
}
