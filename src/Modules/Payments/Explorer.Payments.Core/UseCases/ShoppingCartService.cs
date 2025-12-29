    using AutoMapper;
    using Explorer.Payments.API.Dtos;
    using Explorer.Payments.API.Public;
    using Explorer.Payments.Core.Domain;
    using Explorer.Payments.Core.Domain.RepositoryInterfaces;
    using Explorer.Stakeholders.API.Dtos.Notifications;
    using Explorer.Stakeholders.API.Internal;
    using Explorer.Tours.API.Internal;
    using Explorer.Payments.Core.Domain; 
    using Explorer.Payments.Core.Domain.RepositoryInterfaces; 

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

        public ShoppingCartService(
         IShoppingCartRepository repository,
         ITourSharedService tourService,
         IMapper mapper,
         ITourPurchaseTokenService tokenService,
         IWalletRepository walletRepository,
         IPaymentRepository paymentRepository,
         IPaymentNotificationService notificationService)
        {
            _ShoppingCartRepository = repository;
            _TourService = tourService;
            _mapper = mapper;
            _TokenService = tokenService;
            _walletRepository = walletRepository;
            _paymentRepository = paymentRepository;
            _notificationService = notificationService;
        }


        public List<ShoppingCartDto> GetAll()
        {
            var entities = _ShoppingCartRepository.GetAll();
            return _mapper.Map<List<ShoppingCartDto>>(entities);
        }

        public ShoppingCartDto GetByTourist(long touristId)
        {
            var result = _ShoppingCartRepository.GetByTourist(touristId);
            return _mapper.Map<ShoppingCartDto>(result);
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

            var result = _ShoppingCartRepository.Update(cart);
            return _mapper.Map<ShoppingCartDto>(result);
        }

        public ShoppingCartDto RemoveOrderItem(long touristId, long tourId)
        {
            var cart = _ShoppingCartRepository.GetByTourist(touristId);
            cart.RemoveItem(tourId);

            var result = _ShoppingCartRepository.Update(cart);
            return _mapper.Map<ShoppingCartDto>(result);
        }

    public ShoppingCartDto Checkout(long touristId)
    {
        var cart = _ShoppingCartRepository.GetByTourist(touristId);

        if (cart == null || !cart.Items.Any())
            throw new InvalidOperationException("Shopping cart is empty.");

        var tourIds = cart.Items.Select(i => i.TourId).ToList();
        var tours = tourIds.ToDictionary(
            id => id,
            id => _TourService.Get(id)
        );

        var purchasableItems = cart.Items
            .Where(item => tours[item.TourId].Status == "Published")
            .ToList();

        if (!purchasableItems.Any())
        {
            cart.ClearShoppingCart();
            var resultEmpty = _ShoppingCartRepository.Update(cart);
            return _mapper.Map<ShoppingCartDto>(resultEmpty);
        }

        double totalPrice = purchasableItems.Sum(i => i.ItemPrice);
        var wallet = _walletRepository.GetByTouristId(touristId);

        if (wallet == null)
            throw new InvalidOperationException("Wallet not found.");

        if (wallet.Balance < totalPrice)
            throw new InvalidOperationException("Not enough Adventure Coins.");

      
        ChargeWallet(touristId, totalPrice);

        foreach (var item in purchasableItems)
        {
            _paymentRepository.Create(new Payment(touristId, item.TourId, item.ItemPrice));
            _TokenService.Create(new CreateTourPurchaseTokenDto
            {
                TourId = item.TourId,
                TouristId = touristId
            });
        }

        cart.ClearShoppingCart();
        var result = _ShoppingCartRepository.Update(cart);

       
        SendPurchaseNotification(touristId, purchasableItems.Count);

        return _mapper.Map<ShoppingCartDto>(result);
    }


    private void ChargeWallet(long touristId, double totalPrice) 
        {
            var wallet = _walletRepository.GetByTouristId(touristId);
            wallet.Debit(totalPrice);
            _walletRepository.Update(wallet);
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




}
