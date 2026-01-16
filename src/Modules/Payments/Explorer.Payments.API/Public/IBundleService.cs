using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Payments.API.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payments.API.Public
{
    public interface IBundleService
    {
        BundleDto Get(long id);
        List<BundleDto> GetByAuthor(long authorId);
        BundleDto Create(BundleDto bundle);
        BundleDto Update(BundleDto bundle);
        void Delete(long id);
        BundleDto PublishBundle(long bundleId);
        BundleDto ArchiveBundle(long bundleId);
        double GetTotalToursPrice(long bundleId);
    }
}
