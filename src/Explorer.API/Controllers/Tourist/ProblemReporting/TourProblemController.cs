using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public.Reporting;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.UseCases.Reporting;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using Explorer.Tours.Infrastructure.Database.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist.ProblemReporting
{
    [Route("api/tourist/problemreporting")]
    [ApiController]
    public class TourProblemController : ControllerBase
    {
        private readonly ITourProblemService _tourProblemService;
        private readonly ITourRepository _tourRepository;

        public TourProblemController(ITourProblemService tourProblemService, ITourRepository tourRepository)
        {
            _tourProblemService = tourProblemService;
            _tourRepository = tourRepository;
        }

        [HttpGet]
        [Authorize(Policy = "touristPolicy")]
        public ActionResult<PagedResult<TourProblemDto>> GetAll([FromQuery] int page, [FromQuery] int pageSize)
        {
            return Ok(_tourProblemService.GetPaged(page, pageSize));
        }

        [HttpPost]
        [Authorize(Policy = "touristPolicy")]
        public ActionResult<TourProblemDto> Create([FromBody] TourProblemDto tourProblem)
        {
            return Ok(_tourProblemService.Create(tourProblem));
        }

        [HttpPut("{id:long}")]
        [Authorize(Policy = "touristPolicy")]
        public ActionResult<TourProblemDto> Update([FromBody] TourProblemDto tourProblem)
        {
            return Ok(_tourProblemService.Update(tourProblem));
        }

        [HttpDelete("{id:long}")]
        [Authorize(Policy = "touristPolicy")]
        public ActionResult Delete(long id)
        {
            _tourProblemService.Delete(id);
            return Ok();
        }

        [HttpPost("{id}/comments")]
        [Authorize(Policy = "authorTouristAdminPolicy")]
        public ActionResult<CommentDto> AddComment(long id, [FromBody] CreateCommentDto dto)
        {
            long userId = long.Parse(User.Claims.First(c => c.Type == "id").Value);
            string role = User.Claims.FirstOrDefault(c => c.Type == "role")?.Value;
            bool isAdmin = User.IsInRole("administrator");

            var problem = _tourProblemService.GetById(id);
            var tour = _tourRepository.Get(problem.TourId);

            if (userId != problem.ReporterId && userId != tour.AuthorId && !isAdmin)
                return Forbid();

            var comment = _tourProblemService.AddComment(id, userId, dto.Content);
            return Ok(comment);
        }

        [HttpGet("{id}/comments")]
        [Authorize(Policy = "authorTouristAdminPolicy")]
        public ActionResult<List<CommentDto>> GetComments(long id)
        {
            long userId = long.Parse(User.Claims.First(c => c.Type == "id").Value);
            string role = User.Claims.FirstOrDefault(c => c.Type == "role")?.Value;
            bool isAdmin = User.IsInRole("administrator");

            var problem = _tourProblemService.GetById(id);
            var tour = _tourRepository.Get(problem.TourId);

            if (userId != problem.ReporterId && userId != tour.AuthorId && !isAdmin)
                return Forbid();

            List<CommentDto> comments = _tourProblemService.GetComments(id);

            if (comments == null)
                return NotFound();


            return Ok(comments);
        }

    }
}
