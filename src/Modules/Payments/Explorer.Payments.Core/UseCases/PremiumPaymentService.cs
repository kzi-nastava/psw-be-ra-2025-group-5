using Explorer.Payments.API.Public;
using Explorer.Payments.Core.Domain.RepositoryInterfaces;
using Explorer.Stakeholders.API.Public.Users;
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
        private readonly IPremiumService _premiumService;

        private const int PremiumDays = 30;
        private const double Price = 50;

        public PremiumPaymentService(
            IWalletRepository walletRepository,
            IPremiumService premiumService)
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
            var until = DateTime.UtcNow.AddDays(PremiumDays);
            _premiumService.ExtendPremium(userId, until);
        }

        private void Charge(long userId)
        {
            var wallet = _walletRepository.GetByTouristId(userId);
            if (wallet.Balance < Price)
                throw new InvalidOperationException("Not enough coins.");

            wallet.Debit(Price);
            _walletRepository.Update(wallet);
        }
    }
}
