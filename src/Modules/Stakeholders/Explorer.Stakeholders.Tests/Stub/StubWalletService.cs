using Explorer.Payments.API.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Tests.Stub
{
    public class StubWalletService : IInternalWalletService
    {
        public void CreateWalletForPerson(long personId)
        {
        }

        public void DebitWallet(long userId, double amount)
        {
        }

        public double GetWalletBalance(long userId)
        {
            return 1000;
        }
    }
}
