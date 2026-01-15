using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.API.Dtos.ProfileMessages
{
    public class ProfileMessageDto
    {
        public long Id { get; set; }
        public long AuthorId { get; set; }
        public long ReceiverId { get; set; }
        public string AuthorName { get; set; }
        public string Content { get; set; }
        public int AttachedResourceType { get; set; }
        public long? AttachedResourceId { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
    }
}
