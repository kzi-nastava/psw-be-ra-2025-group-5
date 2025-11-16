using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.API.Dtos;
using Explorer.BuildingBlocks.Core.UseCases;

namespace Explorer.API.Controllers.Administrator.Administration
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
        public ActionResult Delete([FromBody] int id) { 
            _monumentService.Delete(id);
            return Ok();
        }
    }
}
