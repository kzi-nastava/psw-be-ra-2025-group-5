using AutoMapper;
using Explorer.Payments.API.Dtos.PurchaseToken;
using Explorer.Payments.API.Dtos.ShoppingCart;
using Explorer.Payments.API.Public;
using Explorer.Payments.Core.Domain;
using Explorer.Payments.Core.Domain.RepositoryInterfaces;
using Explorer.Stakeholders.API.Dtos.Notifications;
using Explorer.Stakeholders.API.Internal;
using Explorer.Tours.API.Internal;

namespace Explorer.Payments.Core.UseCases;

public class ShoppingCartService : IShoppingCartService
{
    private readonly IShoppingCartRepository _ShoppingCartRepository;
    private readonly ITourSharedService _TourService;
    private readonly ITourPurchaseTokenService _TokenService;
    private readonly IMapper _mapper;
    private readonly IWalletRepository _walletRepository;
    private readonly IPaymentRepository _paymentRepository;
    private readonly IPaymentNotificationService _notificationService;
    private readonly ITourSaleService _TourSaleService;
    private readonly ICouponRepository _couponRepository;

    public ShoppingCartService(IShoppingCartRepository repository, ITourSharedService tourService, IMapper mapper, ITourPurchaseTokenService tokenService, 
        IWalletRepository walletRepository, IPaymentRepository paymentRepository, IPaymentNotificationService notificationService, ITourSaleService tourSaleService, 
        ICouponRepository couponRepository)
    {
        _ShoppingCartRepository = repository;
        _TourService = tourService;
        _mapper = mapper;
        _TokenService = tokenService;
        _walletRepository = walletRepository;
        _paymentRepository = paymentRepository;
        _notificationService = notificationService;
        _TourSaleService = tourSaleService;
        _couponRepository = couponRepository;
    }

    private ShoppingCartDto ValidateCart(ShoppingCartDto cart)
    {
        cart.Items.ForEach(item => item.ItemPrice = _TourSaleService.GetFinalPrice(item.TourId));
        return cart;
    }

    public List<ShoppingCartDto> GetAll()
    {
        var entities = _mapper.Map<List<ShoppingCartDto>>(_ShoppingCartRepository.GetAll());
        return [..entities.Select(entity => ValidateCart(entity))];
    }

    public ShoppingCartDto GetByTourist(long touristId)
    {
        var result = _mapper.Map<ShoppingCartDto>(_ShoppingCartRepository.GetByTourist(touristId));
        return ValidateCart(result);
    }

    public ShoppingCartDto Create(CreateShoppingCartDto entity)
    {
        if(GetAll().Any(cart => cart.TouristId == entity.TouristId))
            throw new InvalidOperationException($"Shopping cart already exists for tourist {entity.TouristId}");

        var result = _ShoppingCartRepository.Create(_mapper.Map<ShoppingCart>(entity));
        return _mapper.Map<ShoppingCartDto>(result);
    }

    public ShoppingCartDto AddOrderItem(long touristId, long tourId)
    {
        var cart = _ShoppingCartRepository.GetByTourist(touristId) ?? _ShoppingCartRepository.Create(new ShoppingCart(touristId));
        var tour = _TourService.Get(tourId);
        cart.AddItem(tour.Id, tour.Name, tour.Price);

        var result = _mapper.Map<ShoppingCartDto>(_ShoppingCartRepository.Update(cart));
        return ValidateCart(result);
    }

    public ShoppingCartDto RemoveOrderItem(long touristId, long tourId)
    {
        var cart = _ShoppingCartRepository.GetByTourist(touristId);
        cart.RemoveItem(tourId);

        var result = _mapper.Map<ShoppingCartDto>(_ShoppingCartRepository.Update(cart));
        return ValidateCart(result);
    }

    public ShoppingCartDto Checkout(long touristId)
    {
        var cart = _ShoppingCartRepository.GetByTourist(touristId);

        if (cart == null || !cart.Items.Any())
            throw new InvalidOperationException("Shopping cart is empty.");

        var tourIds = cart.Items.Select(i => i.TourId).ToList();
        var tours = tourIds.ToDictionary(id => id, id => _TourService.Get(id));
        var purchasableItems = cart.Items.Where(item => tours[item.TourId].Status == "Published").Select(item => _mapper.Map<OrderItemDto>(item)).ToList();
        purchasableItems.ForEach(item => item.ItemPrice = _TourSaleService.GetFinalPrice(item.TourId));

        if (!purchasableItems.Any())
        {
            cart.ClearShoppingCart();
            var resultEmpty = _ShoppingCartRepository.Update(cart);
            return _mapper.Map<ShoppingCartDto>(resultEmpty);
        }

        double totalPrice = purchasableItems.Sum(i => i.ItemPrice.FinalPrice);

        if (cart.AppliedCouponId.HasValue)
        {
            var coupon = _couponRepository.Get(cart.AppliedCouponId.Value);
            if (coupon != null)
            {
                OrderItemDto targetItem;

                if (coupon.TourId.HasValue)
                {
                    targetItem = purchasableItems.FirstOrDefault(i => i.TourId == coupon.TourId.Value);
                }
                else
                {
                    var authorId = coupon.AuthorId; 
                    targetItem = purchasableItems
                        .Where(i => tours[i.TourId].AuthorId == authorId)
                        .OrderByDescending(i => i.ItemPrice)
                        .FirstOrDefault();
                }

                if (targetItem != null)
                {
                    totalPrice -= targetItem.ItemPrice.FinalPrice * coupon.Percentage / 100.0;
                }
            }
        }

        var wallet = _walletRepository.GetByUserId(touristId);

        if (wallet == null)
            throw new InvalidOperationException("Wallet not found.");

        if (wallet.Balance < totalPrice)
            throw new InvalidOperationException("Not enough Adventure Coins.");
      
        ChargeWallet(touristId, totalPrice);

        foreach (var item in purchasableItems)
        {
            var tour = tours[item.TourId];
            var authorId = tour.AuthorId;

            CreditAuthorWallet(authorId, item.ItemPrice.FinalPrice);

            _paymentRepository.Create(new Payment(touristId, item.TourId, item.ItemPrice.FinalPrice));
            _TokenService.Create(new CreateTourPurchaseTokenDto { TourId = item.TourId, TouristId = touristId });
        }

        cart.ClearShoppingCart();
        var result = _ShoppingCartRepository.Update(cart);
        SendPurchaseNotification(touristId, purchasableItems.Count);

        return _mapper.Map<ShoppingCartDto>(result);
    }

    private void ChargeWallet(long userId, double totalPrice) 
    {
        var wallet = _walletRepository.GetByUserId(userId);
        wallet.Debit(totalPrice);
        _walletRepository.Update(wallet);
    }

    private void CreditAuthorWallet(long authorId, double amount)
    {
        var authorWallet = _walletRepository.GetByUserId(authorId);

        if (authorWallet == null)
        {
            authorWallet = new Wallet(authorId);
            _walletRepository.Create(authorWallet);
        }

        authorWallet.Credit(amount);
        _walletRepository.Update(authorWallet);
    }

    private void SendPurchaseNotification(long touristId, int toursCount)
    {
        var message = toursCount == 1
            ? "New tour has been added to your collection."
            : $"{toursCount} new tours have been added to your collection.";

        _notificationService.Create(new NotificationDto
        {
            UserId = touristId,
            Title = "Tour purchased",
            Message = message,
            Type = "TourPurchased",
            CreatedAt = DateTime.UtcNow
        });
    }

    public ShoppingCartDto ApplyCouponToCart(long touristId, string couponCode)
    {
        var cart = _ShoppingCartRepository.GetByTourist(touristId)
            ?? _ShoppingCartRepository.Create(new ShoppingCart(touristId));

        var coupon = _couponRepository.GetByCode(couponCode);
        if (coupon == null)
            throw new InvalidOperationException("Coupon not found");

        OrderItem targetItem = null;
        if (coupon.TourId.HasValue)
            targetItem = cart.Items.FirstOrDefault(i => i.TourId == coupon.TourId.Value);
        else
            targetItem = cart.Items.OrderByDescending(i => i.ItemPrice).FirstOrDefault();

        if (targetItem != null)
        {
            targetItem.ItemPrice -= targetItem.ItemPrice * coupon.Percentage / 100.0;
            _couponRepository.Delete(coupon.Id);
        }

        var updatedCart = _ShoppingCartRepository.Update(cart);

        return _mapper.Map<ShoppingCartDto>(updatedCart);
    }
}
