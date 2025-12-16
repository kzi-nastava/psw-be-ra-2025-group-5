using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Core.Domain.RepositoryInterfaces
{
    public interface ITourExecutionRepository
    {
        TourExecution Get(long id);
        void Add(TourExecution execution);
        void Update(TourExecution execution);
        TourExecution GetActiveForUser(long userId, long tourId);
        // za auto proveru isteka ture
        IEnumerable<TourExecution> GetAllActiveOlderThan(DateTime olderThan);
        TourExecution GetActiveOrCompletedForUser(long userId, long tourId);

    }
}
