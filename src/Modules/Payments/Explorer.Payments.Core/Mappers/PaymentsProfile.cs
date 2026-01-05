using AutoMapper;
using Explorer.Payments.API.Dtos.Pricing;
using Explorer.Payments.API.Dtos.PurchaseToken;
using Explorer.Payments.API.Dtos.ShoppingCart;
using Explorer.Payments.API.Dtos.Wallet;
using Explorer.Payments.Core.Domain;

namespace Explorer.Payments.Core.Mappers;

public class PaymentsProfile: Profile
{
    public PaymentsProfile()
    {
        CreateMap<OrderItem, OrderItemDto>()
            .ForMember(dest => dest.ItemPrice,
               opt => opt.MapFrom(src => new TourPriceDto
               {
                   BasePrice = src.ItemPrice,
                   FinalPrice = src.ItemPrice,
                   DiscountPercentage = 0
               }));

        CreateMap<OrderItemDto, OrderItem>()
            .ForMember(dest => dest.ItemPrice, opt => opt.MapFrom(src => src.ItemPrice.BasePrice));

        CreateMap<ShoppingCartDto, ShoppingCart>().ReverseMap();

        CreateMap<CreateShoppingCartDto, ShoppingCart>()
            .ConstructUsing(src => new ShoppingCart(src.TouristId));

        CreateMap<TourPurchaseTokenDto, TourPurchaseToken>().ReverseMap();
        CreateMap<CreateTourPurchaseTokenDto, TourPurchaseToken>();

        CreateMap<Wallet, WalletDto>();

        CreateMap<TourSaleDto, TourSale>().ReverseMap();
        CreateMap<CreateTourSaleDto, TourSale>();
    }
}
