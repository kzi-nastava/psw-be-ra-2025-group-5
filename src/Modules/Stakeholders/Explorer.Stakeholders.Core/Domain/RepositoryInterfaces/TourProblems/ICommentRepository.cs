using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.Core.Domain.Comments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Core.Domain.RepositoryInterfaces.TourProblems
{
    public interface ICommentRepository
    {
        Comment CreateComment(Comment comment);
        Comment GetByCommentId(long id);
    }
}
