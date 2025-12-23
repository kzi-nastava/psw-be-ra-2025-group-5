using Explorer.Tours.API.Dtos;

namespace Explorer.Tours.Tests.Builders;

public class TourDtoBuilder
{
    public static CreateTourDto CreateValid()
    {
        return new CreateTourDto
        {
            Name = "Zlatibor Mountain Adventure",
            Description = "A scenic trail across Zlatibor with panoramic viewpoints and traditional food stops.",
            Difficulty = "Medium",
            Tags = ["Nature", "Hiking", "Viewpoints"],
            Price = 0,
            AuthorId = 2,
            ThumbnailImage = null,
            ThumbnailContentType = null
        };
    }
}
