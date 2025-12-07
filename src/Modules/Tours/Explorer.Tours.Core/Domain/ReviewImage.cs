using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Core.Domain;

public class ReviewImage: Entity
{
    public string ImagePath { get; set; } = null!;
    public long ReviewId { get; set; }

    public ReviewImage() { }

    public ReviewImage(string imagePath)
    {
        ImagePath = imagePath;
    }

    public void UpdatePath(string path)
    {
        ImagePath = path;
    }
}
