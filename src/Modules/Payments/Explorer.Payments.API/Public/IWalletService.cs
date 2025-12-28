using Explorer.Payments.API.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payments.API.Public
{
    public interface IWalletService
    {
        WalletDto GetWalletForTourist(long touristId);
        void CreditToTourist(long touristId, double amount);

    }
}
