using Explorer.Stakeholders.Core.Domain.ClubMessages;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces.ClubMessages;
using System.Collections.Generic;
using System.Linq;

namespace Explorer.Stakeholders.Infrastructure.Database.Repositories.ClubMessages
{
    public class ClubMessageDbRepository : IClubMessageRepository
    {
        private readonly StakeholdersContext _dbContext;

        public ClubMessageDbRepository(StakeholdersContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ClubMessage Create(ClubMessage message)
        {
            _dbContext.ClubMessages.Add(message);
            _dbContext.SaveChanges();
            return message;
        }

        public ClubMessage Update(ClubMessage message)
        {
            _dbContext.ClubMessages.Update(message);
            _dbContext.SaveChanges();
            return message;
        }

        public void Delete(long id)
        {
            var message = _dbContext.ClubMessages.Find(id);
            if (message != null)
            {
                _dbContext.ClubMessages.Remove(message);
                _dbContext.SaveChanges();
            }
        }

        public ClubMessage GetById(long id)
        {
            return _dbContext.ClubMessages.Find(id);
        }

        public List<ClubMessage> GetByClubId(long clubId)
        {
            return _dbContext.ClubMessages
                .Where(m => m.ClubId == clubId)
                .OrderByDescending(m => m.CreatedAt)
                .ToList();
        }
    }
}
