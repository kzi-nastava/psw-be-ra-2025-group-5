using Explorer.BuildingBlocks.Core.Domain;
using Explorer.BuildingBlocks.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Core.Domain
{
    public class TourProgress: ValueObject
    {
        public double Percentage { get; private set; }

        private TourProgress() { }

        public TourProgress(double percentage)
        {
            if (percentage < 0 || percentage > 100)
                throw new ArgumentOutOfRangeException(nameof(percentage), "Progress must be between 0 and 100.");

            Percentage = Math.Round(percentage, 2);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Percentage;
        }

        public bool IsAbove(double threshold) => Percentage >= threshold;
    }
}
