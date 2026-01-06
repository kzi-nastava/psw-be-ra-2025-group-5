using Explorer.Payments.API.Dtos.Pricing;

namespace Explorer.Payments.Tests.Builders;

public class TourSaleDtoBuilder
{
    public static CreateTourSaleDto CreateValidCreateDto()
    {
        return new CreateTourSaleDto
        {
            AuthorId = -12,
            TourIds = new List<long> { -5 },
            ExpirationDate = DateTime.UtcNow.AddDays(10),
            DiscountPercentage = 80
        };
    }

    public static TourSaleDto CreateValidRegularDto()
    {
        return new TourSaleDto
        {
            Id = -1,
            AuthorId = -11,
            TourIds = new List<long> { -1 },
            CreationDate = DateTime.UtcNow,
            ExpirationDate = DateTime.UtcNow.AddDays(10),
            DiscountPercentage = 80
        };
    }
}
