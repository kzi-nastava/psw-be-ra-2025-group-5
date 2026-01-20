using Explorer.Payments.API.Dtos.Wallet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payments.API.Public
{
    public interface IWalletService
    {
        WalletDto GetWalletForUser(long touristId);
        void CreditToTourist(long touristId, double amount);
        List<WalletUserDto> GetAllTourist();


    }
}
