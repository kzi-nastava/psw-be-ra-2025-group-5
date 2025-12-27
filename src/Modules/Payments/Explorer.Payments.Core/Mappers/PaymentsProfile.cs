using AutoMapper;
using Explorer.Payments.API.Dtos;
using Explorer.Payments.Core.Domain;

namespace Explorer.Payments.Core.Mappers;

public class PaymentsProfile: Profile
{
    public PaymentsProfile()
    {
        CreateMap<OrderItem, OrderItemDto>().ReverseMap();

        CreateMap<ShoppingCartDto, ShoppingCart>().ReverseMap();

        CreateMap<CreateShoppingCartDto, ShoppingCart>()
            .ConstructUsing(src => new ShoppingCart(src.TouristId));

        CreateMap<TourPurchaseTokenDto, TourPurchaseToken>().ReverseMap();
        CreateMap<CreateTourPurchaseTokenDto, TourPurchaseToken>();

        CreateMap<Wallet, WalletDto>();
    }
}
