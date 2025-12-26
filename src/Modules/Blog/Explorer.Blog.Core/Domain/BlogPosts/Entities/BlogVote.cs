using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Blog.Core.Domain.BlogPosts.Entities
{
    public enum VoteType
    {
        Downvote = -1, Upvote = 1
    }
    public class BlogVote : Entity
    {
        public long UserId { get; private set; }
        public long BlogPostId { get; private set; }
        public DateTime VoteDate { get; private set; }
        public VoteType VoteType { get; private set; }

        public BlogVote(long userId, long blogPostId, VoteType voteType)
        {
            UserId = userId;
            BlogPostId = blogPostId;
            VoteDate = DateTime.UtcNow;
            VoteType = voteType;
            Validate();
        }

        private void Validate()
        {
            if (UserId == 0) throw new ArgumentException("Invalid UserId");
            if (BlogPostId == 0) throw new ArgumentException("Invalid BlogPostId");
        }
    }

    
}
