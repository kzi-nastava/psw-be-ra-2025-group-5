namespace Explorer.Stakeholders.API.Dtos.ProfileMessages
{
    public class UpdateMessageDto
    {
        public string Content { get; set; }
        public int AttachedResourceType { get; set; }
        public long? AttachedResourceId { get; set; }
    }
}
