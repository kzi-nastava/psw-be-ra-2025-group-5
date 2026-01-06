using Explorer.Tours.Core.Domain.RepositoryInterfaces.Tours;
using Explorer.Tours.Core.Domain.Tours.Entities;
using Microsoft.EntityFrameworkCore;

namespace Explorer.Tours.Infrastructure.Database.Repositories.Tours
{
    public class TourManualDbRepository : ITourManualRepository
    {
        private readonly ToursContext _context;

        public TourManualDbRepository(ToursContext context)
        {
            _context = context;
        }

        public TourManualProgress? Get(long userId, string pageKey)
        {
            return _context.TourManualProgress
                .FirstOrDefault(x => x.UserId == userId && x.PageKey == pageKey);
        }

        public TourManualProgress Create(TourManualProgress progress)
        {
            _context.TourManualProgress.Add(progress);
            _context.SaveChanges();
            return progress;
        }

        public TourManualProgress Update(TourManualProgress progress)
        {
            _context.TourManualProgress.Update(progress);
            _context.SaveChanges();
            return progress;
        }
    }
}
