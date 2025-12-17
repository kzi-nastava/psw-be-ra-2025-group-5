using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.API.Public.Reporting;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.UseCases.Reporting;
using Explorer.Tours.API.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist.ProblemReporting
{
    [Route("api/tourist/problemreporting")]
    [ApiController]
    public class TourProblemController : ControllerBase
    {
        private readonly ITourProblemService _tourProblemService;
        private readonly ITourService _tourService;
        private readonly INotificationService _notificationService;

        public TourProblemController(
            ITourProblemService tourProblemService, 
            ITourService tourService,
            INotificationService notificationService)
        {
            _tourProblemService = tourProblemService;
            _tourService = tourService;
            _notificationService = notificationService;
        }

        [HttpGet]
        [Authorize(Policy = "authorTouristAdminPolicy")]
        public ActionResult<PagedResult<TourProblemDto>> GetAll([FromQuery] int page, [FromQuery] int pageSize)
        {
            long userId = long.Parse(User.Claims.First(c => c.Type == "id").Value);
            bool isAdmin = User.IsInRole("administrator");

            PagedResult<TourProblemDto> result;

            if (isAdmin)
            {
                result = _tourProblemService.GetPaged(page, pageSize);
            }
            else if (User.IsInRole("tourist"))
            {
                result = _tourProblemService.GetPagedByReporterId(userId, page, pageSize);
            }
            else if (User.IsInRole("author"))
            {
                var tours = _tourService.GetPagedByAuthor(userId, page, int.MaxValue).Results;
                var tourIds = tours.Select(t => (long)t.Id).ToList();

                result = _tourProblemService.GetPagedByTourIds(tourIds, page, pageSize);
            }
            else
            {
                return Forbid();
            }

            return Ok(result);
        }


        [HttpPost]
        [Authorize(Policy = "touristPolicy")]
        public ActionResult<TourProblemDto> Create([FromBody] TourProblemDto tourProblem)
        {
            var result = _tourProblemService.Create(tourProblem);

            try
            {
                var tour = _tourService.GetById(result.TourId);
                _notificationService.CreateProblemReportedNotification(
                    authorId: tour.AuthorId,
                    tourId: result.TourId,
                    problemId: result.Id,
                    tourName: tour.Name
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send notification: {ex.Message}");
            }

            return Ok(result);
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
            var tour = _tourService.GetById(problem.TourId);

            if (userId != problem.ReporterId && userId != tour.AuthorId && !isAdmin)
                return Forbid();

            var comment = _tourProblemService.AddComment(id, userId, dto.Content);

            try
            {
                var commenterName = User.Claims.First(c => c.Type == "username").Value;

                if (problem.ReporterId != userId)
                {
                    _notificationService.CreateCommentAddedNotification(
                        recipientId: problem.ReporterId,
                        problemId: id,
                        commenterName: commenterName,
                        tourId: problem.TourId
                    );
                }

                if (tour.AuthorId != userId && tour.AuthorId != problem.ReporterId)
                {
                    _notificationService.CreateCommentAddedNotification(
                        recipientId: tour.AuthorId,
                        problemId: id,
                        commenterName: commenterName,
                        tourId: problem.TourId
                    );
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send notification: {ex.Message}");
            }

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
            var tour = _tourService.GetById(problem.TourId);

            if (userId != problem.ReporterId && userId != tour.AuthorId && !isAdmin)
                return Forbid();

            List<CommentDto> comments = _tourProblemService.GetComments(id);

            if (comments == null)
                return NotFound();


            return Ok(comments);
        }

        [HttpPut("{id}/problem-resolved")]
        [Authorize(Policy = "touristOrAdminPolicy")]
        public ActionResult MarkResolved([FromRoute] long id, [FromQuery] bool isResolved)
        {
            long userId = long.Parse(User.Claims.First(c => c.Type == "id").Value);
            bool isAdmin = User.IsInRole("administrator");
            string username = User.Claims.First(c => c.Type == "username").Value;

            var result = _tourProblemService.MarkResolved(id, isResolved);

            try
            {
                var problem = _tourProblemService.GetById(id);
                var tour = _tourService.GetById(problem.TourId);

                if (isAdmin)
                {
                    if (problem.ReporterId != userId)
                    {
                        _notificationService.CreateProblemStatusChangedNotification(
                            recipientId: problem.ReporterId,
                            problemId: id,
                            isResolved: isResolved,
                            changedByUsername: username,
                            tourId: problem.TourId
                        );
                    }

                    if (tour.AuthorId != userId && tour.AuthorId != problem.ReporterId)
                    {
                        _notificationService.CreateProblemStatusChangedNotification(
                            recipientId: tour.AuthorId,
                            problemId: id,
                            isResolved: isResolved,
                            changedByUsername: username,
                            tourId: problem.TourId
                        );
                    }
                }
                else
                {
                    if (tour.AuthorId != userId)
                    {
                        _notificationService.CreateProblemStatusChangedNotification(
                            recipientId: tour.AuthorId,
                            problemId: id,
                            isResolved: isResolved,
                            changedByUsername: username,
                            tourId: problem.TourId
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send notification: {ex.Message}");
            }

            return Ok(result);
        }

        [HttpGet("{id:long}")]
        [Authorize(Policy = "authorTouristAdminPolicy")]
        public ActionResult<TourProblemDto> GetById(long id)
        {
            long userId = long.Parse(User.Claims.First(c => c.Type == "id").Value);
            bool isAdmin = User.IsInRole("administrator");

            var problem = _tourProblemService.GetById(id);
            var tour = _tourService.GetById(problem.TourId);

            if (userId != problem.ReporterId && userId != tour.AuthorId && !isAdmin)
                return Forbid();

            return Ok(problem);
        }

        [HttpPut("{id}/deadline")]
        [Authorize(Policy = "administratorPolicy")]
        public ActionResult<TourProblemDto> SetDeadline(long id, [FromBody] SetDeadlineDto dto)
        {
            _tourProblemService.SetDeadline(id, dto.Deadline);

            try
            {
                var problem = _tourProblemService.GetById(id);
                var tour = _tourService.GetById(problem.TourId);

                _notificationService.CreateDeadlineSetNotification(
                    authorId: tour.AuthorId,
                    problemId: id,
                    deadline: dto.Deadline,
                    tourName: tour.Name
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send notification: {ex.Message}");
            }

            return Ok(_tourProblemService.GetById(id));
        }
    }
}
