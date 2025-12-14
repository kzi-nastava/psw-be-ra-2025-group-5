using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Core.Domain;

public class ReviewImage: Entity
{
    public long ReviewId { get; set; }
    public byte[] Data { get; set; } = null!;
    public string ContentType { get; set; } = null!;
    public int Order { get; set; }

    protected ReviewImage() { } // EF Core

    public ReviewImage(long reviewId, byte[] data, string contentType, int order)
    {
        ReviewId = reviewId;
        Data = data;
        ContentType = contentType;
        Order = order;

        Validate();
    }

    private void Validate()
    {
        if (ReviewId <= 0)
            throw new ArgumentException("Invalid ReviewId");

        if (Data == null || Data.Length == 0)
            throw new ArgumentException("Image data is required");

        if (string.IsNullOrWhiteSpace(ContentType))
            throw new ArgumentException("ContentType is required");

        if (Order < 0)
            throw new ArgumentException("Order must be >= 0");
    }
}
