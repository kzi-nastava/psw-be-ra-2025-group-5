using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.Infrastructure.Authentication;
using Explorer.Tours.API.Dtos.KeyPoints;
using Explorer.Tours.API.Dtos.Tours;
using Explorer.Tours.API.Public.Tour;
using Explorer.Tours.API.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tours.Author;

[Authorize(Policy = "authorPolicy")]
[Route("api/tours")]
[ApiController]
public class TourController : ControllerBase
{
    private readonly ITourService _tourService;
    private readonly ITourSearchHistoryService _searchHistoryService;

    public TourController(ITourService tourService, ITourSearchHistoryService searchHistoryService)
    {
        _tourService = tourService;
        _searchHistoryService = searchHistoryService;
    }

    [HttpGet]
    public ActionResult<PagedResult<TourDto>> GetAll([FromQuery] int page, [FromQuery] int pageSize, [FromQuery] long? authorId)
    {
        return authorId.HasValue 
            ? Ok(_tourService.GetPagedByAuthor((long)authorId, page, pageSize)) 
            : Ok(_tourService.GetPaged(page, pageSize));
    }

    [HttpGet("{id:long}")]
    public ActionResult<TourDto> Get(long id)
    {
        return Ok(_tourService.Get(id));
    }

    [HttpGet("tags")]
    [AllowAnonymous]
    public ActionResult<List<string>> GetAllTags()
    {
        return Ok(_tourService.GetAllTags());
    }

    [HttpGet("search")]
    [AllowAnonymous]
    public ActionResult<PagedResult<TourDto>> Search([FromQuery] double latitude, [FromQuery] double longitude, [FromQuery] double distance, [FromQuery] int page, [FromQuery] int pageSize, [FromQuery] string? difficulty, [FromQuery] double? minPrice, [FromQuery] double? maxPrice, [FromQuery] List<string>? tags, [FromQuery] string? sortBy, [FromQuery] string? sortOrder)
    {
        var searchDto = new TourSearchDto
        {
            Latitude = latitude,
            Longitude = longitude,
            Distance = distance,
            Difficulty = difficulty,
            MinPrice = minPrice,
            MaxPrice = maxPrice,
            Tags = tags,
            SortBy = sortBy,
            SortOrder = sortOrder
        };

        var result = _tourService.SearchByLocation(searchDto, page, pageSize);

        if (User.Identity?.IsAuthenticated == true)
        {
            var userId = long.Parse(User.FindFirst("id")?.Value ?? "0");
            if (userId > 0)
            {
                try
                {
                    _searchHistoryService.SaveSearch(userId, searchDto);
                }
                catch
                {
                }
            }
        }

        return Ok(result);
    }

    [HttpPost]
    public ActionResult<TourDto> Create([FromBody] CreateTourDto tour)
    {
        long userId = long.Parse(User.Claims.First(c => c.Type == "id").Value);
        return Ok(_tourService.Create(tour, userId));
    }

    [HttpPut("{id:long}")]
    public ActionResult<TourDto> Update(long id, [FromBody] UpdateTourDto tour)
    {
        return Ok(_tourService.Update(id, tour));
    }

    [HttpDelete("{id:long}")]
    public ActionResult Delete(long id)
    {
        _tourService.Delete(id);
        return Ok();
    }

    // KeyPoint operacije
    [HttpPost("{tourId:long}/keypoints")]
    [Consumes("multipart/form-data")]
    public ActionResult<TourDto> AddKeyPoint(long tourId, [FromForm] CreateKeyPointDto dto)
    {
        var result = _tourService.AddKeyPoint(tourId, dto);
        return Ok(result);
    }

    [HttpPut("{tourId:long}/keypoints/{keyPointId:long}")]
    [Consumes("multipart/form-data")]
    public ActionResult<TourDto> UpdateKeyPoint(long tourId, long keyPointId, [FromForm] CreateKeyPointDto dto)
    {
        var result = _tourService.UpdateKeyPoint(tourId, keyPointId, dto);
        return Ok(result);
    }

    [HttpDelete("{tourId:long}/keypoints/{keyPointId:long}")]
    public ActionResult<TourDto> RemoveKeyPoint(long tourId, long keyPointId, [FromQuery] double tourLength)
    {
        var result = _tourService.RemoveKeyPoint(tourId, keyPointId, tourLength);
        return Ok(result);
    }

    [HttpPut("{tourId:long}/keypoints/reorder")]
    public ActionResult<TourDto> ReorderKeyPoints(long tourId, [FromBody] ReorderKeyPointsDto reorderDto)
    {
        var result = _tourService.ReorderKeyPoints(tourId, reorderDto);
        return Ok(result);
    }

    // Status operacije
    [HttpPost("{tourId:long}/publish")]
    public ActionResult<TourDto> Publish(long tourId)
    {
        var result = _tourService.Publish(tourId);
        return Ok(result);
    }

    [HttpPost("{tourId:long}/archive")]
    public ActionResult<TourDto> Archive(long tourId)
    {
        var result = _tourService.Archive(tourId);
        return Ok(result);
    }

    [HttpPost("{tourId:long}/reactivate")]
    public ActionResult<TourDto> Reactivate(long tourId)
    {
        var result = _tourService.Reactivate(tourId);
        return Ok(result);
    }

    [HttpPost("{tourId:long}/equipment/{equipmentId:long}")]
    public ActionResult<TourDto> AddEquipment(long tourId, long equipmentId)
    {
        var result = _tourService.AddRequiredEquipment(tourId, equipmentId);
        return Ok(result);
    }

    [HttpDelete("{tourId:long}/equipment/{equipmentId:long}")]
    public ActionResult<TourDto> RemoveEquipment(long tourId, long equipmentId)
    {
        var result = _tourService.RemoveRequiredEquipment(tourId, equipmentId);
        return Ok(result);
    }

    [Authorize(Roles = "author")]
    [HttpPost("{tourId:long}/thumbnail")]
    [Consumes("multipart/form-data")]
    public ActionResult<TourDto> UploadThumbnail(
     long tourId,
     [FromForm] UploadTourThumbnailDto dto)
    {
        var userId = User.PersonId();

        if (!_tourService.CanEditTour(tourId, userId))
            return Forbid();

        var result = _tourService.UploadThumbnail(tourId, dto.Thumbnail);
        return Ok(result);
    }

    [AllowAnonymous]
    [HttpGet("{tourId:long}/thumbnail/{*fileName}")]
    public IActionResult GetThumbnail(long tourId, string fileName)
    {
        string filePath;

        Console.WriteLine($"[THUMBNAIL DEBUG] Original fileName: '{fileName}'");

        // Normalize path - remove leading slash and normalize separators
        var normalizedFileName = fileName.TrimStart('/').Replace('/', Path.DirectorySeparatorChar);
        Console.WriteLine($"[THUMBNAIL DEBUG] Normalized fileName: '{normalizedFileName}'");

        // Check if this is a static image path (starts with "images/")
        if (normalizedFileName.StartsWith($"images{Path.DirectorySeparatorChar}", StringComparison.OrdinalIgnoreCase))
        {
            filePath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                normalizedFileName);
            Console.WriteLine($"[THUMBNAIL DEBUG] Using STATIC path (full): '{filePath}'");
        }
        else
        {
            // Try UserUploads first (for manually uploaded images)
            filePath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "UserUploads",
                "tours",
                tourId.ToString(),
                normalizedFileName);
            Console.WriteLine($"[THUMBNAIL DEBUG] Trying UPLOADED path: '{filePath}'");

            // If not found in UserUploads, try wwwroot/images/tours (for seed images)
            if (!System.IO.File.Exists(filePath))
            {
                var staticPath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "images",
                    "tours",
                    normalizedFileName);
                Console.WriteLine($"[THUMBNAIL DEBUG] Not found in uploads, trying STATIC path: '{staticPath}'");

                if (System.IO.File.Exists(staticPath))
                {
                    filePath = staticPath;
                    Console.WriteLine($"[THUMBNAIL DEBUG] Found in STATIC!");
                }
            }
        }

        Console.WriteLine($"[THUMBNAIL DEBUG] Final path: '{filePath}'");
        Console.WriteLine($"[THUMBNAIL DEBUG] File exists: {System.IO.File.Exists(filePath)}");

        if (!System.IO.File.Exists(filePath))
        {
            Console.WriteLine($"[THUMBNAIL DEBUG] FILE NOT FOUND, returning 404");
            return NotFound();
        }

        var ext = Path.GetExtension(fileName).ToLower();
        var mime = ext switch
        {
            ".png" => "image/png",
            ".jpg" => "image/jpeg",
            ".jpeg" => "image/jpeg",
            _ => "application/octet-stream"
        };

        Console.WriteLine($"[THUMBNAIL DEBUG] Serving file with MIME: {mime}");
        return PhysicalFile(filePath, mime);
    }
    
    [AllowAnonymous]
    [HttpGet("{keyPointId:long}/keypoints/images/{*fileName}")]
    public IActionResult GetImage(long keyPointId, string fileName)
    {
        string filePath;

        Console.WriteLine($"[KEYPOINT DEBUG] Original fileName: '{fileName}'");

        // Normalize path - remove leading slash and normalize separators
        var normalizedFileName = fileName.TrimStart('/').Replace('/', Path.DirectorySeparatorChar);
        Console.WriteLine($"[KEYPOINT DEBUG] Normalized fileName: '{normalizedFileName}'");

        // Check if this is a static image path (starts with "images/")
        if (normalizedFileName.StartsWith($"images{Path.DirectorySeparatorChar}", StringComparison.OrdinalIgnoreCase))
        {
            filePath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                normalizedFileName);
            Console.WriteLine($"[KEYPOINT DEBUG] Using STATIC path (full): '{filePath}'");
        }
        else
        {
            // Try UserUploads first (for manually uploaded images)
            filePath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "UserUploads",
                "keypoints",
                keyPointId.ToString(),
                normalizedFileName);
            Console.WriteLine($"[KEYPOINT DEBUG] Trying UPLOADED path: '{filePath}'");

            // If not found in UserUploads, try wwwroot/images/keypoints (for seed images)
            if (!System.IO.File.Exists(filePath))
            {
                var staticPath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "images",
                    "keypoints",
                    normalizedFileName);
                Console.WriteLine($"[KEYPOINT DEBUG] Not found in uploads, trying STATIC path: '{staticPath}'");

                if (System.IO.File.Exists(staticPath))
                {
                    filePath = staticPath;
                    Console.WriteLine($"[KEYPOINT DEBUG] Found in STATIC!");
                }
            }
        }

        Console.WriteLine($"[KEYPOINT DEBUG] Final path: '{filePath}'");
        Console.WriteLine($"[KEYPOINT DEBUG] File exists: {System.IO.File.Exists(filePath)}");

        if (!System.IO.File.Exists(filePath))
        {
            Console.WriteLine($"[KEYPOINT DEBUG] FILE NOT FOUND, returning 404");
            return NotFound();
        }

        var ext = Path.GetExtension(fileName).ToLower();
        var mime = ext switch
        {
            ".png" => "image/png",
            ".jpg" => "image/jpeg",
            ".jpeg" => "image/jpeg",
            _ => "application/octet-stream"
        };

        Console.WriteLine($"[KEYPOINT DEBUG] Serving file with MIME: {mime}");
        return PhysicalFile(filePath, mime);
    }

    [HttpGet("creation-quota")]
    public ActionResult<TourCreationQuotaDto> GetCreationQuota()
    {
        long userId = long.Parse(User.Claims.First(c => c.Type == "id").Value);
        return Ok(_tourService.GetCreationQuota(userId));
    }
}
