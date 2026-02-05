using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payments.Core.Domain.RepositoryInterfaces
{
    public interface IWalletRepository
    {
        List<Wallet> GetAll();
        Wallet GetByUserId(long userId);
        Wallet Create(Wallet wallet);
        Wallet Update(Wallet wallet);
        Wallet GetByTouristId(long touristId);
    }
}
