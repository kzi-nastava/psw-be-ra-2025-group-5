using Explorer.Stakeholders.Core.Domain.ProfileMessages;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces.ProfileMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Infrastructure.Database.Repositories
{
    public class ProfileMessageDbRepository : IProfileMessageRepository
    {
        private readonly StakeholdersContext _dbContext;

        public ProfileMessageDbRepository(StakeholdersContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ProfileMessage Create(ProfileMessage message)
        {
            _dbContext.ProfileMessages.Add(message);
            _dbContext.SaveChanges();
            return message;
        }

        public ProfileMessage Update(ProfileMessage message)
        {
            _dbContext.ProfileMessages.Update(message);
            _dbContext.SaveChanges();
            return message;
        }

        public void Delete(long id)
        {
            var message = _dbContext.ProfileMessages.Find(id);
            if (message != null)
            {
                _dbContext.ProfileMessages.Remove(message);
                _dbContext.SaveChanges();
            }
        }

        public ProfileMessage? GetById(long id)
        {
            return _dbContext.ProfileMessages.Find(id);
        }

        public List<ProfileMessage> GetByReceiverId(long authorId, long receiverId)
        {
            return _dbContext.ProfileMessages
                .Where(pm =>
                        (pm.ReceiverId == receiverId && pm.AuthorId == authorId) ||
                        (pm.ReceiverId == authorId && pm.AuthorId == receiverId)
                )
                .ToList();
        }
    }
}
