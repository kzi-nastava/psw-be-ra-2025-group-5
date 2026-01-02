using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.BuildingBlocks.Core.Domain;

namespace Explorer.Payments.Core.Domain
{
    public enum BundleStatus
    {
        Draft,
        Published,
        Archived
    }
    public class Bundle : Entity
    {
        public string Name { get; private set; }
        public double Price { get; private set; }
        public long AuthorId { get; private set; }
        public BundleStatus Status { get; private set; }
        public List<BundleItem> BundleItems { get; private set; }

        public Bundle(string name, double price, long authorId, BundleStatus status)
        {
            Name = name;
            Price = price;
            AuthorId = authorId;
            Status = status;
            BundleItems = new List<BundleItem>();

            Validate();
        }

        private void Validate()
        {
            if (string.IsNullOrWhiteSpace(Name)) throw new ArgumentException("Bundle name can`t be empty.");
            if (Price < 0) throw new ArgumentException("Price can`t be negative.");
        }

        public void AddBundleItem(long tourId)
        {
            foreach (var item in BundleItems)
            {
                if (item.TourId == tourId)
                {
                    return;
                }
            }

            var newBundleItem = new BundleItem(Id, tourId);
            BundleItems.Add(newBundleItem);
        }

        public void ClearBundleItems()
        {
            BundleItems.Clear();
        }

        public void Publish(int publishedToursCount)
        {
            if (publishedToursCount < 2)
                throw new InvalidOperationException("Bundle must contain at least two published tours to be published.");

            Status = BundleStatus.Published;
        }

        public void Archive()
        {
            Status = BundleStatus.Archived;
        }

        public void Delete()
        {
            if (Status == BundleStatus.Published)
                throw new InvalidOperationException("Cannot delete a published bundle.");
        }
    }
}
