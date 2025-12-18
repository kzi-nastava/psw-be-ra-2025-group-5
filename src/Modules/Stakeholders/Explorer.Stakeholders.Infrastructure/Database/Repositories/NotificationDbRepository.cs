using Explorer.BuildingBlocks.Core.Exceptions;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Explorer.Stakeholders.Infrastructure.Database.Repositories
{
    public class NotificationDbRepository : INotificationRepository
    {
        private readonly StakeholdersContext _dbContext;
        private readonly DbSet<Notification> _dbSet;

        public NotificationDbRepository(StakeholdersContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<Notification>();
        }

        public Notification Create(Notification entity)
        {
            _dbSet.Add(entity);
            _dbContext.SaveChanges();
            return entity;
        }

        public Notification Get(long id)
        {
            var entity = _dbSet.Find(id);
            if (entity == null)
                throw new NotFoundException($"Notification {id} not found");
            return entity;
        }

        public PagedResult<Notification> GetPagedByUserId(long userId, int page, int pageSize)
        {
            var task = _dbSet
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .GetPagedById(page, pageSize);
            task.Wait();
            return task.Result;
        }

        public List<Notification> GetUnreadByUserId(long userId)
        {
            return _dbSet
                .Where(n => n.UserId == userId && !n.IsRead)
                .OrderByDescending(n => n.CreatedAt)
                .ToList();
        }

        public int GetUnreadCountByUserId(long userId)
        {
            return _dbSet.Count(n => n.UserId == userId && !n.IsRead);
        }

        public Notification Update(Notification entity)
        {
            try
            {
                _dbContext.Update(entity);
                _dbContext.SaveChanges();
            }
            catch (DbUpdateException e)
            {
                throw new NotFoundException(e.Message);
            }
            return entity;
        }

        public void Delete(long id)
        {
            var entity = Get(id);
            _dbSet.Remove(entity);
            _dbContext.SaveChanges();
        }
    }
}
