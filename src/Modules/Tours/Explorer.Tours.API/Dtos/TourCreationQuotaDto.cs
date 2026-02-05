namespace Explorer.Tours.API.Dtos
{
    public class TourCreationQuotaDto
    {
        public bool CanCreate { get; set; }
        public int Remaining { get; set; }
        public int? Limit { get; set; }
        public DateTime? ResetAt { get; set; }
    }
}
