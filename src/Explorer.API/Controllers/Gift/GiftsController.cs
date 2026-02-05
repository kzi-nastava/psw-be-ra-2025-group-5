using System;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Explorer.Payments.API.Dtos.Gifts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Explorer.Payments.API.Public
{
    [ApiController]
    [Route("api/gifts")]
    public class GiftsController : ControllerBase
    {
        private readonly IGiftsService _giftsService;
        private readonly ILogger<GiftsController> _logger;
        private readonly IMapper _mapper;

        public GiftsController(IGiftsService giftsService, ILogger<GiftsController> logger, IMapper mapper)
        {
            _giftsService = giftsService;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpPost("")]
        [Authorize]
        public IActionResult CreateGift([FromBody] CreateGiftDto dto)
        {
            var donorClaim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("sub") ?? User.FindFirst("id");
            if (donorClaim == null) return Unauthorized();
            if (!long.TryParse(donorClaim.Value, out var donorId)) return Unauthorized();

            object createdGift = null;
            GiftDto response = null;

            try
            {
                createdGift = _giftsService.CreateGift(donorId, dto);
                response = _mapper.Map<GiftDto>(createdGift);

                var successBody = new
                {
                    gift = response,
                    message = "Gift sent successfully. The recipient will have access to the tour."
                };
                return CreatedAtAction(nameof(GetById), new { id = response.Id }, successBody);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogInformation(ex, "Business validation failed while creating gift for donor {DonorId}", donorId);
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Post-processing error during gift creation (gift created: {HasGift}) for donor {DonorId}", createdGift != null, donorId);

                if (response != null)
                {
                    var fallbackBody = new
                    {
                        gift = response,
                        message = "Gift created, but a follow-up step failed (e.g. notification). The recipient will still receive the tour. We logged the issue."
                    };
                    return CreatedAtAction(nameof(GetById), new { id = response.Id }, fallbackBody);
                }

                return StatusCode(500, new { error = "Unexpected server error. Please try again later." });
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        public IActionResult GetById(long id)
        {
            var gift = _giftsService.GetById(id);
            if (gift == null) return NotFound();

            var userClaim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("sub") ?? User.FindFirst("id");
            if (userClaim != null && long.TryParse(userClaim.Value, out var userId))
            {
                if (userId != gift.RecipientId && userId != gift.DonorId && !User.IsInRole("Admin"))
                    return Forbid();
            }

            return Ok(gift);
        }

        [HttpGet("tourist-friends")]
        [Authorize]
        public IActionResult GetTouristFriends()
        {
            var userClaim = User.FindFirst(ClaimTypes.NameIdentifier)
                            ?? User.FindFirst("sub")
                            ?? User.FindFirst("id");

            if (userClaim == null) return Unauthorized();
            if (!long.TryParse(userClaim.Value, out var userId)) return Unauthorized();

            try
            {
                var touristFriends = _giftsService.GetTouristFriends(userId);
                return Ok(touristFriends);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving tourist friends for user {UserId}", userId);
                return StatusCode(500, new { error = "Failed to retrieve friends list" });
            }
        }
    }
}