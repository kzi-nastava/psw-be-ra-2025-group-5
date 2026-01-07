using System;

namespace Explorer.Stakeholders.API.Dtos.ClubMessages
{
    public class ClubMessageDto
    {
        public long Id { get; set; }
        public long ClubId { get; set; }
        public long AuthorId { get; set; }
        public string Content { get; set; }
        public int AttachedResourceType { get; set; }
        public long? AttachedResourceId { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
    }
}
