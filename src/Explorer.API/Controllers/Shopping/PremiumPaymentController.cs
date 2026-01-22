using Explorer.Payments.API.Public;
using Explorer.Stakeholders.API.Public.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Users
{
    [Route("api/premium")]
    [ApiController]
    [Authorize(Policy = "authorOrTouristPolicy")]
    public class PremiumController : ControllerBase
    {
        private readonly IPremiumPaymentService _paymentService;
        private readonly IPremiumService _premiumService;

        public PremiumController(
            IPremiumPaymentService paymentService,
            IPremiumService premiumService)
        {
            _paymentService = paymentService;
            _premiumService = premiumService;
        }

        [HttpPost("purchase")]
        public IActionResult Purchase()
        {
            var userId = long.Parse(User.FindFirst("id")!.Value);

            // brza provera da purchase ne radi kao extend
            if (_premiumService.IsPremium(userId))
                return Conflict("User already has premium. Use /extend.");

            try
            {
                _paymentService.PurchasePremium(userId);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("extend")]
        public IActionResult Extend()
        {
            var userId = long.Parse(User.FindFirst("id")!.Value);

            try
            {
                _paymentService.ExtendPremium(userId);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Premium not found. Purchase first.");
            }
            catch (InvalidOperationException ex)
            {
                // npr expired, wallet not found, not enough coins
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("cancel")]
        public IActionResult Cancel()
        {
            var userId = long.Parse(User.FindFirst("id")!.Value);
            _premiumService.RemovePremium(userId);
            return NoContent();
        }

        [HttpGet("status")]
        public ActionResult<bool> Status()
        {
            var userId = long.Parse(User.FindFirst("id")!.Value);
            var result = _premiumService.IsPremium(userId);
            return Ok(result);
        }
    }
}
