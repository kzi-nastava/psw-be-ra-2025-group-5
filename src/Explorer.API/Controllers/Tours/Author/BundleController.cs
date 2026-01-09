using Explorer.Payments.API.Dtos;
using Explorer.Payments.API.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tours.Author
{
    [Authorize(Policy = "authorPolicy")]
    [Route("api/author/bundles")]
    [ApiController]
    public class BundleController : ControllerBase
    {
        private readonly IBundleService _bundleService;

        public BundleController(IBundleService bundleService)
        {
            _bundleService = bundleService;
        }

        [HttpGet("{id:long}")]
        public ActionResult<BundleDto> Get(long id)
        {
            var result = _bundleService.Get(id);
            return Ok(result);
        }

        [HttpGet]
        public ActionResult<List<BundleDto>> GetByAuthor([FromQuery] long authorId)
        {
            var result = _bundleService.GetByAuthor(authorId);
            return Ok(result);
        }

        [HttpPost]
        public ActionResult<BundleDto> Create([FromQuery] long authorId, [FromBody] BundleDto dto)
        {
            dto.AuthorId = authorId;

            var result = _bundleService.Create(dto);
            return Ok(result);
        }

        [HttpPut("{id:long}")]
        public ActionResult<BundleDto> Update(long id, [FromBody] BundleDto dto)
        {
            dto.Id = id;
            var result = _bundleService.Update(dto);
            return Ok(result);
        }

        [HttpDelete("{id:long}")]
        public ActionResult Delete(long id)
        {
            _bundleService.Delete(id);
            return Ok();
        }

        [HttpPut("{id:long}/publish")]
        public ActionResult<BundleDto> Publish(long id)
        {
            var result = _bundleService.PublishBundle(id);
            return Ok(result);
        }

        [HttpPut("{id:long}/archive")]
        public ActionResult<BundleDto> Archive(long id)
        {
            var result = _bundleService.ArchiveBundle(id);
            return Ok(result);
        }

        [HttpGet("{id:long}/total-tours-price")]
        public ActionResult<double> GetTotalToursPrice(long id)
        {
            var result = _bundleService.GetTotalToursPrice(id);
            return Ok(result);
        }
    }
}
