using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Author;

[Authorize(Policy = "authorPolicy")]
[Route("api/tours")]
[ApiController]
public class TourController : ControllerBase
{
    private readonly ITourService _tourService;

    public TourController(ITourService tourService)
    {
        _tourService = tourService;
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

    [HttpPost]
    public ActionResult<TourDto> Create([FromBody] CreateTourDto tour)
    {
        var authorId = long.Parse(User.Claims.First(c => c.Type == "id").Value);
        return Ok(_tourService.Create(tour, authorId));
    }

    [HttpPut("{id:long}")]
    public ActionResult<TourDto> Update(long id, [FromBody] UpdateTourDto tour)
    {
        var authorId = long.Parse(User.Claims.First(c => c.Type == "id").Value);
        return Ok(_tourService.Update(id, tour, authorId));
    }

    [HttpDelete("{id:long}")]
    public ActionResult Delete(long id)
    {
        _tourService.Delete(id);
        return Ok();
    }

    // KeyPoint operacije
    [HttpPost("{tourId:long}/keypoints")]
    public ActionResult<TourDto> AddKeyPoint(long tourId, [FromBody] CreateKeyPointDto keyPoint)
    {
        var result = _tourService.AddKeyPoint(tourId, keyPoint);
        return Ok(result);
    }

    [HttpPut("{tourId:long}/keypoints/{keyPointId:long}")]
    public ActionResult<TourDto> UpdateKeyPoint(long tourId, long keyPointId, [FromBody] CreateKeyPointDto keyPoint)
    {
        var result = _tourService.UpdateKeyPoint(tourId, keyPointId, keyPoint);
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

    [HttpPost("{id}/thumbnail")]
    [Consumes("multipart/form-data")]
    public ActionResult<TourThumbnailDto> AddThumbnail(
    long id,
    [FromForm] TourThumbnailUploadDto dto)
    {
        if (dto.File == null || dto.File.Length == 0)
            return BadRequest("No file uploaded");

        var authorId = long.Parse(User.Claims.First(c => c.Type == "id").Value);
        var tour = _tourService.Get(id);
        
        if (tour.AuthorId != authorId)
            return Forbid("You can only add thumbnails to your own tours");

        using var ms = new MemoryStream();
        dto.File.CopyTo(ms);

        var result = _tourService.AddThumbnail(id, ms.ToArray(), dto.File.ContentType);
        return Ok(result);
    }

    [HttpGet("{id}/thumbnail")]
    public ActionResult<TourThumbnailDto> GetThumbnail(long id)
    {
        var result = _tourService.GetThumbnail(id);
        return Ok(result);
    }
}