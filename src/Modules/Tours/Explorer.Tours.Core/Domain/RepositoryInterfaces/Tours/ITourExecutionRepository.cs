using Explorer.Tours.Core.Domain.TourExecutions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Core.Domain.RepositoryInterfaces.Tours
{
    public interface ITourExecutionRepository
    {
        TourExecution Get(long id);
        void Add(TourExecution execution);
        void Update(TourExecution execution);
        TourExecution GetActiveForUser(long userId, long tourId);
        // za auto proveru isteka ture
        IEnumerable<TourExecution> GetAllActiveOlderThan(DateTime olderThan);
        IEnumerable<TourExecution> GetByUserId(long userId);
        bool HasAnyExecution(long userId, long tourId);
        TourExecution GetActiveOrCompletedForUser(long userId, long tourId);

    }
}
