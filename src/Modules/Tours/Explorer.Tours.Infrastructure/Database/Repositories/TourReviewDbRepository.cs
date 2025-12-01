using Explorer.BuildingBlocks.Core.Domain;
using Explorer.BuildingBlocks.Core.Exceptions;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.BuildingBlocks.Infrastructure.Database;
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
    public class TourReviewDbRepository: ITourReviewRepository
    {
        protected readonly ToursContext DbContext;
        private readonly DbSet<TourReview> _dbSet;

        public TourReviewDbRepository(ToursContext dbContext)
        {
            DbContext = dbContext;
            _dbSet = dbContext.Set<TourReview>();
        }

        public PagedResult<TourReview> GetPaged(int page, int pageSize)
        {
            var query = _dbSet.Include(t => t.Images);
            var task = query.GetPagedById(page, pageSize);
            task.Wait();
            return task.Result;
        }

        public PagedResult<TourReview> GetByTour(long tourId, int page, int pageSize) {
            var query = _dbSet.Include(t => t.Images).Where(t => t.TourID == tourId);
            var task = query.GetPagedById(page, pageSize);
            task.Wait();
            return task.Result;
        }

        public PagedResult<TourReview> GetByTourist(long touristId, int page, int pageSize)
        {
            var query = _dbSet.Include(t => t.Images).Where(t => t.TouristID == touristId);
            var task = query.GetPagedById(page, pageSize);
            task.Wait();
            return task.Result;
        }

        TourReview Get(int id)
        {
            var entity = _dbSet.Include(t => t.Images).FirstOrDefault(t => t.Id  == id);
            if (entity == null) throw new NotFoundException("Not found: " + id);
            return entity;
        }
        TourReview Create(TourReview newReview)
        {
            _dbSet.Add(newReview);
            DbContext.SaveChanges();
            return newReview;
        }
        TourReview Update(TourReview newReview)
        {
            var existing = DbContext.Set<TourReview>()
                    .Include(r => r.Images)
                    .FirstOrDefault(r => r.Id == newReview.Id)
                    ?? throw new NotFoundException($"Review with id {newReview.Id} not found.");

            existing.UpdateComment(newReview.Comment);
            existing.UpdateGrade(newReview.Grade);
            existing.ReplaceImages(newReview.Images);

            DbContext.SaveChanges();

            return existing;
        }

        void Delete(int id)
        {
            var entity = Get(id);
            _dbSet.Remove(entity);
            DbContext.SaveChanges();
        }
    }
}
