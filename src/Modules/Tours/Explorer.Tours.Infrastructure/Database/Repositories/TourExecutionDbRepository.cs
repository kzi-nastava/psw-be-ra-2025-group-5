using Explorer.Tours.Core.Domain;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Infrastructure.Database.Repositories
{
    public class TourExecutionDbRepository : ITourExecutionRepository
    {
        private readonly ToursContext _context;

        public TourExecutionDbRepository(ToursContext context)
        {
            _context = context;
        }

        public TourExecution Get(long id)
        {
            return _context.TourExecutions
                .Include(e => e.CompletedKeyPoints)
                .FirstOrDefault(e => e.Id == id);
        }

        public void Add(TourExecution execution)
        {
            _context.TourExecutions.Add(execution);
            _context.SaveChanges();
        }

        public void Update(TourExecution execution)
        {
            _context.TourExecutions.Update(execution);
            _context.SaveChanges();
        }
        public TourExecution GetActiveForUser(long userId, long tourId)
        {
            return _context.TourExecutions
                .Include(x => x.CompletedKeyPoints)
                .FirstOrDefault(x => x.UserId == userId
                                     && x.TourId == tourId
                                     && x.Status == TourExecutionStatus.Active);
        }

    }
}
