using Explorer.Stakeholders.API.Public.Notifications;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces.TourProblems;
using Explorer.Tours.API.Public.Tour;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Administrator
{
    [Authorize(Policy = "administratorPolicy")]
    [Route("api/admin/tours")]
    [ApiController]
    public class TourController : ControllerBase
    {
        private readonly ITourService _tourService;
        private readonly ITourProblemRepository _tourProblemRepository;
        private readonly INotificationService _notificationService;

        public TourController(
            ITourService tourService,
            ITourProblemRepository tourProblemRepository,
            INotificationService notificationService)
        {
            _tourService = tourService;
            _tourProblemRepository = tourProblemRepository;
            _notificationService = notificationService;
        }

        [HttpPost("{id}/close")]
        public ActionResult CloseTour(long id)
        {
            var tour = _tourService.GetById(id);
            if (tour == null) return NotFound();

            if (tour.Status == "Closed")
                return BadRequest("Tour is already closed");

            var problems = _tourProblemRepository.GetByTourId(id);
            var unresolvedExpired = problems
                .Where(p => !p.IsResolved && p.Deadline.HasValue && p.Deadline <= DateTimeOffset.UtcNow);

            if (!unresolvedExpired.Any())
                return BadRequest("Cannot close tour: no unresolved problems with expired deadline.");

            _tourService.CloseTour(id);

            try
            {
                _notificationService.CreateTourClosedNotification(
                    authorId: tour.AuthorId,
                    tourId: id,
                    tourName: tour.Name,
                    reason: "Unresolved problems with expired deadline"
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send notification: {ex.Message}");
            }

            return Ok();
        }

        [HttpGet("{id}/tour-status")]
        public ActionResult GetTourStatus(long id)
        {
            var tour = _tourService.GetById(id);
            if (tour == null) return NotFound();

            return Ok(new { status = tour.Status });
        }

    }
}
