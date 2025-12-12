using Explorer.Tours.API.Public;
using Explorer.Tours.Core.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Administrator.Administration
{
    [Authorize(Policy = "administratorPolicy")]
    [Route("api/admin/tours")]
    [ApiController]
    public class TourController : ControllerBase
    {
        private readonly ITourService _tourService;

        public TourController(ITourService TourService)
        {
            _tourService = TourService;
        }

        [HttpPost("{id}/close")]
        public IActionResult CloseTour(long id)
        {
            _tourService.CloseTour(id);
            return Ok();
        }

    }
}
