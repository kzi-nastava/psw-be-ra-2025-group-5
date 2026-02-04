using Explorer.Payments.API.Internal;
using Explorer.Payments.Core.Domain;
using Explorer.Payments.Core.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payments.Core.UseCases
{
    public class WalletAdapter : IInternalWalletService
    {
        private readonly IWalletRepository _walletRepository;

        public WalletAdapter(IWalletRepository walletRepository)
        {
            _walletRepository = walletRepository;
        }

        public void CreateWalletForPerson(long personId)
        {
            var existingWallet = _walletRepository.GetByUserId(personId);
            if (existingWallet != null)
                return;

            var wallet = new Wallet(personId);
            _walletRepository.Create(wallet);
        }
    }
}
