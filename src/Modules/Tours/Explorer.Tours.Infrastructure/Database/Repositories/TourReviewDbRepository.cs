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

        public TourReview Get(long id)
        {
            var entity = _dbSet.Include(t => t.Images).FirstOrDefault(t => t.Id  == id);
            if (entity == null) throw new NotFoundException("Not found: " + id);
            return entity;
        }
        public TourReview Create(TourReview newReview)
        {
            _dbSet.Add(newReview);
            DbContext.SaveChanges();
            return newReview;
        }
        public TourReview Update(TourReview newReview)
        {
            var existing = DbContext.TourReviews
                .Include(r => r.Images)
                .FirstOrDefault(r => r.Id == newReview.Id);

            if (existing == null)
                throw new NotFoundException($"Review {newReview.Id} not found.");

            existing.UpdateComment(newReview.Comment);
            existing.UpdateGrade(newReview.Grade);

            UpdateReviewImages(existing, newReview.Images);

            DbContext.SaveChanges();
            return existing;
        }

        private void UpdateReviewImages(TourReview existing, List<ReviewImage> newImages)
        {
            var imageSet = DbContext.Set<ReviewImage>();

            
            var newIds = newImages.Where(i => i.Id != 0).Select(i => i.Id).ToHashSet();

            var imagesToDelete = existing.Images
                .Where(img => !newIds.Contains(img.Id))
                .ToList();

            foreach (var img in imagesToDelete)
                imageSet.Remove(img);

            
            var imagesToAdd = newImages
                .Where(img => img.Id == 0)
                .ToList();

            foreach (var img in imagesToAdd)
                existing.Images.Add(new ReviewImage(img.ImagePath));

            
            foreach (var existingImg in existing.Images)
            {
                var updated = newImages.FirstOrDefault(i => i.Id == existingImg.Id);
                if (updated != null)
                    existingImg.UpdatePath(updated.ImagePath);
            }
        }

        public void Delete(long id)
        {
            var entity = Get(id);
            _dbSet.Remove(entity);
            DbContext.SaveChanges();
        }
    }
}
