using Explorer.Tours.API.Dtos.Tours;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.API.Public.Tour
{
    public interface ITourManualService
    {
        TourManualStatusDto GetStatus(long userId, string pageKey);
        void MarkAsSeen(long userId, string pageKey);
    }
}
