using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist;

[Authorize(Policy = "touristPolicy")]
[Route("api/tourist/diaries")]
[ApiController]
public class DiaryController : ControllerBase
{
    private readonly IDiaryService _diaryService;

    public DiaryController(IDiaryService diaryService)
    {
        _diaryService = diaryService;
    }

    [HttpGet]
    public ActionResult<PagedResult<DiaryDto>> GetAll([FromQuery] int page, [FromQuery] int pageSize)
    {
        var result = _diaryService.GetByTourist(User.PersonId(), page, pageSize);
        return Ok(result);
    }

    [HttpPost]
    public ActionResult<DiaryDto> Create([FromBody] DiaryDto diary)
    {
        diary.TouristId = User.PersonId();
        diary.CreatedAt = DateTime.UtcNow;
        diary.Status = 0;

        var created = _diaryService.Create(diary);
        return Ok(created);
    }

    [HttpPut("{id:long}")]
    public ActionResult<DiaryDto> Update(long id, [FromBody] DiaryDto diary)
    {
        diary.Id = id;
        diary.TouristId = User.PersonId();
        var updated = _diaryService.Update(diary);
        return Ok(updated);
    }

    [HttpDelete("{id:long}")]
    public ActionResult Delete(long id)
    {
        _diaryService.Delete(id);
        return Ok();
    }
}
