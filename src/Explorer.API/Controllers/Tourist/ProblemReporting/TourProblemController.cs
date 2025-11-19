using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Reporting;
using Explorer.Tours.Core.UseCases.Administration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist.ProblemReporting
{
    [Authorize(Policy = "touristPolicy")]
    [Route("api/tourist/problemreporting")]
    [ApiController]
    public class TourProblemController : ControllerBase
    {
        private readonly ITourProblemService _tourProblemService;

        public TourProblemController(ITourProblemService tourProblemService)
        {
            _tourProblemService = tourProblemService;
        }

        [HttpGet]
        public ActionResult<PagedResult<TourProblemDto>> GetAll([FromQuery] int page, [FromQuery] int pageSize)
        {
            return Ok(_tourProblemService.GetPaged(page, pageSize));
        }

        [HttpPost]
        public ActionResult<TourProblemDto> Create([FromBody] TourProblemDto tourProblem)
        {
            return Ok(_tourProblemService.Create(tourProblem));
        }

        [HttpPut("{id:long}")]
        public ActionResult<TourProblemDto> Update([FromBody] TourProblemDto tourProblem)
        {
            return Ok(_tourProblemService.Update(tourProblem));
        }

        [HttpDelete("{id:long}")]
        public ActionResult Delete(long id)
        {
            _tourProblemService.Delete(id);
            return Ok();
        }
    }
}
