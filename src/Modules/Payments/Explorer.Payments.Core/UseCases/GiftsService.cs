using System;
using System.Linq;
using System.Collections.Generic;
using Explorer.Payments.API.Dtos.Gifts;
using Explorer.Payments.API.Public;
using Explorer.Payments.Core.Domain;
using Explorer.Payments.Core.Domain.RepositoryInterfaces;
using Explorer.Payments.API.Dtos.PurchaseToken;
using Explorer.Stakeholders.API.Dtos.Notifications;
using Explorer.Stakeholders.API.Internal;
using Explorer.Tours.API.Internal;
using Microsoft.Extensions.Logging;

namespace Explorer.Payments.Core.UseCases
{
    public class GiftsService : IGiftsService
    {
        private readonly IGiftRepository _giftRepo;
        private readonly IShoppingCartService _shoppingCartService;
        private readonly ITourSharedService _tourService;
        private readonly IPaymentNotificationService _notificationService;
        private readonly IUserInfoService _userInfoService;
        private readonly ILogger<GiftsService> _logger;
        private readonly ITourSaleService _tourSaleService;
        private readonly IInternalProfileFollowService _followService; 
        private readonly IUserRoleService _userRoleService;

        public GiftsService(
            IGiftRepository giftRepo,
            IShoppingCartService shoppingCartService,
            ITourSharedService tourService,
            ITourSaleService tourSaleService,
            IPaymentNotificationService notificationService,
            IUserInfoService userInfoService,
            ILogger<GiftsService> logger,
            IInternalProfileFollowService followService, 
            IUserRoleService userRoleService)
        {
            _giftRepo = giftRepo;
            _shoppingCartService = shoppingCartService;
            _tourService = tourService;
            _tourSaleService = tourSaleService;
            _notificationService = notificationService;
            _userInfoService = userInfoService;
            _logger = logger;
            _followService = followService;
            _userRoleService = userRoleService;
        }

        public GiftDto CreateGift(long donorId, CreateGiftDto request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var tour = _tourService.Get(request.TourId);
            if (tour == null || !string.Equals(tour.Status, "Published", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("Tour not purchasable.");

            try
            {
                _shoppingCartService.CheckoutAsGift(donorId, request.RecipientId, request.TourId);

                var priceDto = _tourSaleService.GetFinalPrice(request.TourId);
                var gift = new Gift(donorId, request.RecipientId, request.TourId, priceDto.FinalPrice, request.Message);
                _giftRepo.Create(gift);

                try
                {
                    var donorProfile = _userInfoService.GetProfileByUserId(donorId);
                    string donorName = $"{donorProfile.Name} {donorProfile.Surname}";

                    _notificationService.CreateGiftReceivedNotification(
                        recipientId: request.RecipientId,
                        tourId: request.TourId,
                        tourName: tour.Name,
                        donorName: donorName,
                        message: request.Message
                    );
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to create gift notification");
                }

                return new GiftDto
                {
                    Id = gift.Id,
                    DonorId = gift.DonorId,
                    RecipientId = gift.RecipientId,
                    TourId = gift.TourId,
                    Price = gift.Price,
                    Message = gift.Message,
                    Status = gift.Status,
                    CreatedAt = gift.CreatedAt
                };
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Failed to purchase gift");
                throw;
            }
        }

        public GiftDto? GetById(long id)
        {
            var gift = _giftRepo.GetById(id);
            if (gift == null) return null;

            return new GiftDto
            {
                Id = gift.Id,
                DonorId = gift.DonorId,
                RecipientId = gift.RecipientId,
                TourId = gift.TourId,
                Price = gift.Price,
                Message = gift.Message,
                Status = gift.Status,
                CreatedAt = gift.CreatedAt
            };
        }

        public List<TouristFriendDto> GetTouristFriends(long userId)
        {
            try
            {
                var followingList = _followService.GetFollowing(userId).GetAwaiter().GetResult();
                var touristFriends = new List<TouristFriendDto>();

                foreach (var following in followingList)
                {
                    try
                    {
                        if (!_userRoleService.IsTourist(following.FollowingId))
                            continue;

                        var profile = _userInfoService.GetProfileByUserId(following.FollowingId);

                        if (profile != null)
                        {
                            touristFriends.Add(new TouristFriendDto
                            {
                                UserId = following.FollowingId,
                                Name = profile.Name,
                                Surname = profile.Surname,
                                Username = profile.Username
                            });
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Failed to get profile for user {UserId}", following.FollowingId);
                    }
                }

                return touristFriends.OrderBy(f => f.Name).ThenBy(f => f.Surname).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get tourist friends for user {UserId}", userId);
                return new List<TouristFriendDto>();
            }
        }
    }
}