using Explorer.Stakeholders.API.Dtos.AppRatings;
using Explorer.Stakeholders.API.Public.AppRatings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Administrator
{
    [Authorize(Roles = "administrator")]
    [Route("api/admin/app-ratings")]
    public class AdminAppRatingsController : ControllerBase
    {
        private readonly IAppRatingService _service;

        public AdminAppRatingsController(IAppRatingService service)
        {
            _service = service;
        }

        
        [HttpGet]
        public ActionResult<List<AppRatingDto>> GetAll()
        {
            return Ok(_service.GetAll());
        }
    }
}
