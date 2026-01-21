using Explorer.Payments.API.Public;
using Explorer.Stakeholders.API.Public.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Users
{
    [Route("api/premium")]
    [ApiController]
    [Authorize]
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
            _paymentService.PurchasePremium(userId);
            return NoContent();
        }

        [HttpPost("extend")]
        public IActionResult Extend()
        {
            var userId = long.Parse(User.FindFirst("id")!.Value);
            _paymentService.ExtendPremium(userId);
            return NoContent();
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
