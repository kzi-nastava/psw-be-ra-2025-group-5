using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payments.Core.Domain;
public class Coupon: Entity
{
    public string Code { get; private set; }
    public int Percentage { get; private set; }
    public DateTime? ExpirationDate { get; private set; }
    public long AuthorId { get; private set; }
    public long? TourId { get; private set; }

    public Coupon(string code, int percentage, long authorId, long? tourId, DateTime? expirationDate)
    {
        if (string.IsNullOrWhiteSpace(code) || code.Length != 8)
            throw new ArgumentException("Coupon code must have exactly 8 characters.");

        if (percentage <= 0 || percentage > 100)
            throw new ArgumentException("Percentage must be between 1 and 100.");

        Code = code;
        Percentage = percentage;
        AuthorId = authorId;
        TourId = tourId;
        ExpirationDate = expirationDate.HasValue
        ? DateTime.SpecifyKind(expirationDate.Value, DateTimeKind.Utc)
        : null;
    }

    public bool IsExpired()
    {
        return ExpirationDate.HasValue && ExpirationDate.Value < DateTime.UtcNow;
    }

}

