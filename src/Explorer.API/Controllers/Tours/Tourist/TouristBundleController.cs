using Explorer.Payments.API.Dtos;
using Explorer.Payments.API.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tours.Tourist
{
    [Route("api/tourist/bundles")]
    [ApiController]
    public class TouristBundleController : ControllerBase
    {
        private readonly IBundleService _bundleService;

        public TouristBundleController(IBundleService bundleService)
        {
            _bundleService = bundleService;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult<List<BundleDto>> GetAllPublished()
        {
            var result = _bundleService.GetAllPublished();
            return Ok(result);
        }

        [HttpGet("{id:long}")]
        [AllowAnonymous]
        public ActionResult<BundleDto> GetPublished(long id)
        {
            var bundle = _bundleService.Get(id);
            if (bundle.Status != "Published")
                return NotFound();
            return Ok(bundle);
        }

        [HttpPost("{bundleId:long}/purchase/{touristId:long}")]
        [Authorize(Policy = "touristPolicy")]
        public ActionResult<BundleDto> PurchaseBundle(long bundleId, long touristId)
        {
            try
            {
                var result = _bundleService.PurchaseBundle(bundleId, touristId);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("purchased/{touristId:long}")]
        [Authorize(Policy = "touristPolicy")]
        public ActionResult<List<BundleDto>> GetPurchasedBundles(long touristId)
        {
            var result = _bundleService.GetPurchasedBundles(touristId);
            return Ok(result);
        }
    }
}

