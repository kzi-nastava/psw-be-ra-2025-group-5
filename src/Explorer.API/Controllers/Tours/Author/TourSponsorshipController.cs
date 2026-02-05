using Explorer.Stakeholders.Infrastructure.Authentication;
using Explorer.Tours.API.Dtos.Tours;
using Explorer.Tours.API.Public.Tour;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tours.Author;

[Authorize(Policy = "authorPolicy")]
[Route("api/tours")]
[ApiController]
public class TourSponsorshipController : ControllerBase
{
    private readonly ITourSponsorshipService _sponsorshipService;

    public TourSponsorshipController(ITourSponsorshipService sponsorshipService)
    {
        _sponsorshipService = sponsorshipService;
    }

    [HttpPost("{tourId:long}/sponsor")]
    public ActionResult<TourSponsorshipDto> PurchaseSponsorship(long tourId, [FromBody] CreateTourSponsorshipDto dto)
    {
        try
        {
            long authorId = User.PersonId();
            var result = _sponsorshipService.PurchaseSponsorship(tourId, authorId, dto);
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("{tourId:long}/sponsor")]
    public ActionResult<TourSponsorshipDto?> GetSponsorshipStatus(long tourId)
    {
        var result = _sponsorshipService.GetSponsorshipStatus(tourId);
        return Ok(result);
    }

    [HttpGet("sponsorship-history")]
    public ActionResult<List<TourSponsorshipDto>> GetHistory()
    {
        long authorId = User.PersonId();
        return Ok(_sponsorshipService.GetSponsorshipHistory(authorId));
    }

    [HttpGet("sponsorship-pricing")]
    public ActionResult<Dictionary<int, double>> GetPricing()
    {
        return Ok(new Dictionary<int, double>
        {
            { 7, 50 },
            { 14, 100 },
            { 30, 200 }
        });
    }
}
