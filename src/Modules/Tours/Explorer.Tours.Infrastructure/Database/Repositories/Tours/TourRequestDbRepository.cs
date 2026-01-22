using Explorer.BuildingBlocks.Core.Exceptions;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Tours.Core.Domain.Tours.Entities;
using Explorer.Tours.Core.Domain.RepositoryInterfaces.Tours;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Infrastructure.Database.Repositories.Tours
{
    public class TourRequestDbRepository : ITourRequestRepository
    {
        protected readonly ToursContext DbContext;
        private readonly DbSet<TourRequest> _dbSet;

        public TourRequestDbRepository(ToursContext dbContext)
        {
            DbContext = dbContext;
            _dbSet = DbContext.Set<TourRequest>();
        }

        public TourRequest Get(long id)
        {
            var entity = _dbSet.Find(id);
            if (entity == null) throw new NotFoundException("Not found: " + id);
            return entity;
        }

        public TourRequest Create(TourRequest entity)
        {
            _dbSet.Add(entity);
            DbContext.SaveChanges();
            return entity;
        }

        public TourRequest Update(TourRequest entity)
        {
            try
            {
                DbContext.Update(entity);
                DbContext.SaveChanges();
            }
            catch (DbUpdateException e)
            {
                throw new NotFoundException(e.Message);
            }
            return entity;
        }

        public PagedResult<TourRequest> GetPagedByTourist(long touristId, int page, int pageSize)
        {
            var query = _dbSet.Where(tr => tr.TouristId == touristId);
            var task = query.GetPagedById(page, pageSize);
            task.Wait();
            return task.Result;
        }

        public PagedResult<TourRequest> GetPagedByAuthor(long authorId, int page, int pageSize)
        {
            var query = _dbSet.Where(tr => tr.AuthorId == authorId);
            var task = query.GetPagedById(page, pageSize);
            task.Wait();
            return task.Result;
        }
    }
}
