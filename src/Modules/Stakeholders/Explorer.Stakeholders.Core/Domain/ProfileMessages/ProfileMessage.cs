using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.BuildingBlocks.Core.Domain;

namespace Explorer.Stakeholders.Core.Domain.ProfileMessages
{
    public class ProfileMessage : Entity
    {
        public enum ResourceType
        {
            None = 0,
            Tour = 1,
            BlogPost = 2
        }

        public long AuthorId { get; private set; }
        public long ReceiverId { get; private set; }
        public string Content { get; private set; }
        public ResourceType AttachedResourceType { get; private set; }
        public long? AttachedResourceId { get; private set; }
        public DateTimeOffset CreatedAt { get; private set; }
        public DateTimeOffset? UpdatedAt { get; private set; }

        private ProfileMessage() { }

        public ProfileMessage(long authorId, long receiverId, string content, ResourceType resourceType = ResourceType.None, long? resourceId = null)
        {
            AuthorId = authorId;
            ReceiverId = receiverId;
            Content = content;
            AttachedResourceType = resourceType;
            AttachedResourceId = resourceId;
            CreatedAt = DateTimeOffset.UtcNow;
            UpdatedAt = null;
            Validate();
        }

        private void Validate()
        {
            if (string.IsNullOrWhiteSpace(Content))
                throw new ArgumentException("Message content cannot be empty.");

            if (Content.Length > 280)
                throw new ArgumentException("Message content cannot exceed 280 characters.");

            if (AttachedResourceType != ResourceType.None && (!AttachedResourceId.HasValue || AttachedResourceId <= 0))
                throw new ArgumentException("Resource ID must be provided when resource type is specified.");

            if (AttachedResourceType == ResourceType.None && AttachedResourceId.HasValue)
                throw new ArgumentException("Resource ID should not be provided when resource type is None.");
        }

        public void UpdateContent(string newContent, ResourceType resourceType = ResourceType.None, long? resourceId = null)
        {
            if (string.IsNullOrWhiteSpace(newContent))
                throw new ArgumentException("Message content cannot be empty.");

            if (newContent.Length > 280)
                throw new ArgumentException("Message content cannot exceed 280 characters.");

            Content = newContent;
            AttachedResourceType = resourceType;
            AttachedResourceId = resourceId;
            UpdatedAt = DateTimeOffset.UtcNow;

            Validate();
        }
    }
}
