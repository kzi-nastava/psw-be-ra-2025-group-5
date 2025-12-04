using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.API.Public;

namespace Explorer.Stakeholders.Infrastructure.Database.Repositories
{
    public class CommentDbRepository : ICommentRepository
    {
        private readonly StakeholdersContext _dbContext;

        public CommentDbRepository(StakeholdersContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Comment CreateComment(Comment comment)
        {
            _dbContext.Comments.Add(comment);
            _dbContext.SaveChanges();
            return comment;
        }

        public Comment GetByCommentId(long id)
        {
            return _dbContext.Comments.Find(id);
        }
    }
}
