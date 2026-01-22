using Explorer.Payments.API.Public;
using Explorer.Payments.Core.Domain.RepositoryInterfaces;
using Explorer.Stakeholders.API.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payments.Core.UseCases
{
    public class PremiumPaymentService: IPremiumPaymentService
    {
        private readonly IWalletRepository _walletRepository;
        private readonly IPremiumSharedService _premiumService;

        private const int PremiumDays = 30;
        private const double Price = 50;

        public PremiumPaymentService(
            IWalletRepository walletRepository,
            IPremiumSharedService premiumService)
        {
            _walletRepository = walletRepository;
            _premiumService = premiumService;
        }

        public void PurchasePremium(long userId)
        {
            Charge(userId);
            var until = DateTime.UtcNow.AddDays(PremiumDays);
            _premiumService.GrantPremium(userId, until);
        }

        public void ExtendPremium(long userId)
        {
            Charge(userId);
            _premiumService.ExtendPremium(userId, DateTime.UtcNow);
        }

        private void Charge(long userId)
        {
            var wallet = _walletRepository.GetByUserId(userId);
            if (wallet == null)
                throw new InvalidOperationException("Wallet not found.");

            if (wallet.Balance < Price)
                throw new InvalidOperationException("Not enough coins.");

            wallet.Debit(Price);
            _walletRepository.Update(wallet);
        }
    }
}
