using Explorer.Tours.Core.Domain.Tours.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Core.Domain.RepositoryInterfaces.Tours
{
    public interface ITourManualRepository
    {
        TourManualProgress? Get(long userId, string pageKey);
        TourManualProgress Create(TourManualProgress progress);
        TourManualProgress Update(TourManualProgress progress);
    }
}
