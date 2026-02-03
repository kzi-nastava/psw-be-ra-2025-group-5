using Explorer.Payments.API.Dtos.Wallet;
using Explorer.Payments.API.Public;
using Explorer.Payments.Core.Domain.RepositoryInterfaces;
using Explorer.Stakeholders.API.Dtos.Notifications;
using Explorer.Stakeholders.API.Dtos.Users;
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
        private readonly IUserInfoService _userInfoService;
        private readonly IPaymentRepository _paymentRepository;

        public WalletService(IWalletRepository walletRepository,
                         IPaymentRepository paymentRepository,
                         IPaymentNotificationService notificationService,
                         IUserInfoService userInfoService)
        {
            _walletRepository = walletRepository;
            _paymentRepository = paymentRepository;
            _notificationService = notificationService;
            _userInfoService = userInfoService;
        }

        public WalletDto GetWalletForUser(long userId)
        {
            var wallet = _walletRepository.GetByUserId(userId);
            if (wallet == null)
                throw new KeyNotFoundException("Wallet not found for this tourist.");

            return new WalletDto
            {
                UserId = wallet.UserId,
                Balance = wallet.Balance
            };
        }
        public void CreditToTourist(long userId, double amount)
        {
            var wallet = _walletRepository.GetByUserId(userId);
            if (wallet == null)
                throw new Exception("User wallet not found");

            wallet.Credit(amount);

            _walletRepository.Update(wallet);

            var notification = _notificationService.Create(new NotificationDto
            {
                UserId = userId,
                Title = "Credit Added",
                Message = $"You received {amount} AC from Admin.",
                Type = "CreditAdded",
                CreatedAt = DateTime.UtcNow,
            });
        }

        public List<WalletUserDto> GetAllTourist()
        {
            var tourists = _userInfoService.GetTourists();
            var result = new List<WalletUserDto>();

            foreach (var tourist in tourists)
            {
                var wallet = _walletRepository.GetByUserId(tourist.Id);

                result.Add(new WalletUserDto
                {
                    Id = tourist.Id,
                    Username = tourist.Username,
                    FullName = $"{tourist.Name} {tourist.Surname}",
                    ProfileImagePath = tourist.ProfileImagePath,
                    Balance = wallet?.Balance ?? 0
                });
            }

            return result;
        }

        public IEnumerable<PaymentDto> GetPaymentsForTourist(long touristId)
        {
            var payments = _paymentRepository.GetByTourist(touristId);

            return payments.Select(p => new PaymentDto
            {
                TouristId = p.TouristId,
                TourId = (long)p.TourId,
                Price = Convert.ToDecimal(p.Price), 
                PaidAt = p.CreatedAt,             
                Status = null                       
            }).ToList();
        }


    }
}
