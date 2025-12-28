using Explorer.Payments.API.Dtos;
using Explorer.Payments.API.Public;
using Explorer.Payments.Core.Domain.RepositoryInterfaces;
using Explorer.Stakeholders.API.Dtos.Notifications;
using Explorer.Stakeholders.API.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payments.Core.UseCases
{
    public class WalletService : IWalletService
    {
        private readonly IWalletRepository _walletRepository;
        private readonly IPaymentNotificationService _notificationService;

        public WalletService(IWalletRepository walletRepository, IPaymentNotificationService notificationService)
        {
            _walletRepository = walletRepository;
            _notificationService = notificationService;
        }

        public WalletDto GetWalletForTourist(long touristId)
        {
            var wallet = _walletRepository.GetByTouristId(touristId);
            if (wallet == null)
                throw new KeyNotFoundException("Wallet not found for this tourist.");

            return new WalletDto
            {
                TouristId = wallet.TouristId,
                Balance = wallet.Balance
            };
        }
        public void CreditToTourist(long touristId, double amount)
        {
            var wallet = _walletRepository.GetByTouristId(touristId);
            if (wallet == null)
                throw new Exception("Tourist wallet not found");

            wallet.Credit(amount);

            _walletRepository.Update(wallet);

            var notification = _notificationService.Create(new NotificationDto
            {
                UserId = touristId,
                Title = "Credit Added",
                Message = $"You received {amount} AC from Admin.",
                Type = "CreditAdded",
                CreatedAt = DateTime.UtcNow,
            });
        }
    }
}
