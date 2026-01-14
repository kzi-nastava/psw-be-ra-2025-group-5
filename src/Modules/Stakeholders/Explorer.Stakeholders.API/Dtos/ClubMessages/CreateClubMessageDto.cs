namespace Explorer.Stakeholders.API.Dtos.ClubMessages
{
    public class CreateClubMessageDto
    {
        public string Content { get; set; }
        public int AttachedResourceType { get; set; }
        public long? AttachedResourceId { get; set; }
    }
}
