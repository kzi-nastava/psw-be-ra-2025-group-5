using Microsoft.AspNetCore.Http;

namespace Explorer.Tours.API.Dtos;

public class TourThumbnailUploadDto
{
    public IFormFile File { get; set; }
}