using Explorer.BuildingBlocks.Core.Domain;
using System;

namespace Explorer.Stakeholders.Core.Domain.ClubMessages
{
    public class ClubMessage : Entity
    {
        public enum ResourceType
        {
            None = 0,
            Tour = 1,
            BlogPost = 2
        }

        public long ClubId { get; private set; }
        public long AuthorId { get; private set; }
        public string Content { get; private set; }
        public ResourceType AttachedResourceType { get; private set; }
        public long? AttachedResourceId { get; private set; }
        public DateTimeOffset CreatedAt { get; private set; }
        public DateTimeOffset? UpdatedAt { get; private set; }

        private ClubMessage() { }

        public ClubMessage(long clubId, long authorId, string content, ResourceType resourceType = ResourceType.None, long? resourceId = null)
        {
            ClubId = clubId;
            AuthorId = authorId;
            Content = content;
            AttachedResourceType = resourceType;
            AttachedResourceId = resourceId;
            CreatedAt = DateTimeOffset.UtcNow;
            UpdatedAt = null;
            Validate();
        }

        private void Validate()
        {
            if (ClubId <= 0)
                throw new ArgumentException("Club ID must be valid.");

            if (AuthorId <= 0)
                throw new ArgumentException("Author ID must be valid.");

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
