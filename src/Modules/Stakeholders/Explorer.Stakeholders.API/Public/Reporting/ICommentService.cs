using Explorer.Stakeholders.API.Dtos.Comments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.API.Public.Reporting
{
    public interface ICommentService
    {
        CommentDto Create(long authorId, CreateCommentDto comment);
        CommentDto GetByCommentId(long id);
    }
}
