using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Explorer.Tours.API.Public.Administration;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos.Monuments;

namespace Explorer.API.Controllers.Administrator
{
    [Authorize(Policy = "administratorPolicy")]
    [Route("/api/administration/monuments")]
    [ApiController]
    public class MonumentController : ControllerBase
    {
        private readonly IMonumentService _monumentService;

        public MonumentController(IMonumentService monumentService)
        {
            _monumentService = monumentService;
        }

        [HttpGet]
        public ActionResult<PagedResult<MonumentDto>> GetAll([FromQuery] int page, [FromQuery] int pageSize ) {
            return Ok(_monumentService.GetPaged(page, pageSize));
        }

        [HttpPost]
        public ActionResult<MonumentDto> Create([FromBody] MonumentDto monumentDto) {
            return Ok(_monumentService.Create(monumentDto));
        }

        [HttpPut("{id:long}")]
        public ActionResult<MonumentDto> Update([FromBody] MonumentDto monumentDto)
        {
            return Ok(_monumentService.Update(monumentDto));
        }

        [HttpDelete("{id:long}")]
        public ActionResult Delete(long id) { 
            _monumentService.Delete(id);
            return Ok();
        }

        [AllowAnonymous]
        [HttpGet("public")]
        public ActionResult<IEnumerable<MonumentDto>> GetPublic()
        {
            var monuments = _monumentService.GetAllForTourists();
            return Ok(monuments);
        }
    }
}
