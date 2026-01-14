namespace Explorer.Payments.API.Dtos.Pricing;

public class TourPriceDto
{
    public double BasePrice { get; set; }
    public uint DiscountPercentage { get; set; }
    public double FinalPrice { get; set; }

    public static TourPriceDto NoDiscount(double basePrice)
    => new TourPriceDto
        {
            BasePrice = basePrice,
            DiscountPercentage = 0,
            FinalPrice = basePrice
        };

    public static TourPriceDto WithDiscount(double basePrice, uint discountPercentage)
    => new TourPriceDto
        {
            BasePrice = basePrice,
            DiscountPercentage = discountPercentage,
            FinalPrice = basePrice - (basePrice * discountPercentage / 100)
        };
}
