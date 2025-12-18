using Explorer.BuildingBlocks.Core.Domain;
using Explorer.Tours.Core.Domain.Shared;
using System.Text.Json.Serialization;

namespace Explorer.Tours.Core.Domain;

public class OrderItem : ValueObject
{
    public long TourId { get; }
    public string TourName { get; }
    public double ItemPrice { get; }

    [JsonConstructor]
    public OrderItem(long tourId, string tourName, double itemPrice)
    {
        TourId = tourId;
        TourName = tourName;
        ItemPrice = itemPrice;

        Validate();
    }

    public void Validate()
    {
        Guard.AgainstZero(TourId, nameof(TourId));
        Guard.AgainstNullOrWhiteSpace(TourName, nameof(TourName));
        Guard.AgainstNegative(ItemPrice, nameof(ItemPrice));
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return TourId;
    }
}
