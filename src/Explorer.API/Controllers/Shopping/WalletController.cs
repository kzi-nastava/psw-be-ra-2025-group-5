using Explorer.Payments.API.Dtos.Wallet;
using Explorer.Payments.API.Public;
using Explorer.Stakeholders.API.Dtos.Users;
using Explorer.Stakeholders.API.Internal;
using Explorer.Stakeholders.Core.Domain.Users;
using Explorer.Stakeholders.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Shopping
{
    [Route("api/tourist/wallet")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly IWalletService _walletService;

        public WalletController(IWalletService walletService)
        {
            _walletService = walletService;
        }

        [Authorize(Policy = "touristPolicy")]

        [HttpGet("{touristId:long}")]
        public ActionResult<WalletDto> GetWallet(long touristId)
        {
            var loggedInUserId = User.PersonId();

            if (loggedInUserId != touristId)
                return Forbid();

            var result = _walletService.GetWalletForTourist(touristId);
            return result is not null ? Ok(result) : NotFound();
        }

        [Authorize(Policy = "administratorPolicy")]
        [HttpPost("credit")]
        public IActionResult Credit([FromBody] WalletDto request)
        {
            try
            {
                _walletService.CreditToTourist(request.TouristId, request.Balance);
                return Ok(new { message = "Deposit successful" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    error = ex.Message,
                    stackTrace = ex.StackTrace
                });
            }
        }
        [Authorize(Policy = "administratorPolicy")]
        [HttpGet("all-tourists")]
        public ActionResult<List<WalletUserDto>> GetAllTourists()
        {
            try
            {
                var tourists = _walletService.GetAllTourist();
                return Ok(tourists);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    error = ex.Message,
                    stackTrace = ex.StackTrace
                });
            }
        }

        [Authorize(Policy = "touristPolicy")]
        [HttpGet("{touristId:long}/payments")]
        public ActionResult<List<PaymentDto>> GetPayments(long touristId)
        {
            var loggedInUserId = User.PersonId();
            if (loggedInUserId != touristId) return Forbid();

            var payments = _walletService.GetPaymentsForTourist(touristId);
            return Ok(payments);
        }

    }
}
