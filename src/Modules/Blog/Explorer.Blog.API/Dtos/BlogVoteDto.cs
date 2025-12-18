using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Blog.API.Dtos
{
    public class BlogVoteDto
    {
        public long UserId { get; set; }
        public long BlogPostId { get; set; }
        public string VoteType { get; set; }
        public DateTime VoteDate { get; set; }
    }
}
