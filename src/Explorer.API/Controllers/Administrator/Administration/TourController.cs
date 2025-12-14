using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using Explorer.Tours.API.Public;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
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
        private readonly ITourRepository _tourRepository;
        private readonly ITourProblemRepository _tourProblemRepository;

        public TourController(ITourRepository tourRepository, ITourProblemRepository tourProblemRepository)
        {
            _tourRepository = tourRepository;
            _tourProblemRepository = tourProblemRepository;
        }

        [HttpPost("{id}/close")]
        public ActionResult CloseTour(long id)
        {
            var tour = _tourRepository.Get(id);
            if (tour == null) return NotFound();

            if (tour.Status == TourStatus.Closed)
                return BadRequest("Tour is already closed");

            var problems = _tourProblemRepository.GetByTourId(id);
            var unresolvedExpired = problems
                .Where(p => !p.IsResolved && p.Deadline.HasValue && p.Deadline <= DateTimeOffset.UtcNow);

            if (!unresolvedExpired.Any())
                return BadRequest("Cannot close tour: no unresolved problems with expired deadline.");

            _tourRepository.Close(id);
            return Ok();
        }

        [HttpGet("{id}/tour-status")]
        public ActionResult GetTourStatus(long id)
        {
            var tour = _tourRepository.Get(id);
            if (tour == null) return NotFound();

            return Ok(new { status = tour.Status });
        }

    }
}
