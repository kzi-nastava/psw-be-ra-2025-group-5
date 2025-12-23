using AutoMapper;
using Explorer.BuildingBlocks.Core.Exceptions;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos.Comments;
using Explorer.Stakeholders.API.Dtos.Tours.Problems;
using Explorer.Stakeholders.API.Public.Notifications;
using Explorer.Stakeholders.API.Public.Reporting;
using Explorer.Stakeholders.Core.Domain.Comments;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces.TourProblems;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces.Users;
using Explorer.Stakeholders.Core.Domain.TourProblems;
using Explorer.Tours.Core.Domain.RepositoryInterfaces.Tours;
using Explorer.Tours.Core.Domain.Tours;
using System.Data;


namespace Explorer.Stakeholders.Core.UseCases.Reporting;

public class TourProblemService : ITourProblemService
{
    private readonly ITourProblemRepository _repository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public TourProblemService(
        ITourProblemRepository repository, 
        IUserRepository userRepository,
        IMapper mapper)
    {
        _repository = repository;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public PagedResult<TourProblemDto> GetPaged(int page, int pageSize)
    {
        var result = _repository.GetPaged(page, pageSize);
        return MapToDto(result);
    }

    public PagedResult<TourProblemDto> GetPagedByReporterId(long reporterId, int page, int pageSize)
    {
        var result = _repository.GetPagedByReporterId(reporterId, page, pageSize);
        return MapToDto(result);
    }
    public PagedResult<TourProblemDto> GetPagedByTourIds(List<long> tourIds, int page, int pageSize)
    {
        var result = _repository.GetPagedByTourIds(tourIds, page, pageSize);
        return MapToDto(result);
    }

    public TourProblemDto Create(TourProblemDto dto)
    {
        var created = _repository.Create(_mapper.Map<TourProblem>(dto));
        return _mapper.Map<TourProblemDto>(created);
    }

    public TourProblemDto Update(TourProblemDto dto)
    {
        var updated = _repository.Update(_mapper.Map<TourProblem>(dto));
        return _mapper.Map<TourProblemDto>(updated);
    }

    public void Delete(long id) => _repository.Delete(id);

    public CommentDto AddComment(long problemId, long authorId, string content)
    {
        TourProblem problem = _repository.Get(problemId);
        Comment comment = new Comment(authorId, content);

        _repository.AddComment(comment);

        problem.Comments.Add(comment.CommentId);
        _repository.Update(problem);

        var dto = _mapper.Map<CommentDto>(comment);

        var user = _userRepository.GetById(authorId);
        dto.AuthorRole = user.Role.ToString();

        return dto;
    }

    public TourProblemDto GetById(long id)
    {
        var problem = _repository.Get(id);

        if (problem == null)
            throw new NotFoundException($"TourProblem {id} not found");

        var tourDto = new TourDto { Id = problem.TourId, Name = "Unknown", Status = "Unknown" };

        var reporter = _userRepository.GetById(problem.ReporterId);

        var dto = new TourProblemDto
        {
            Id = problem.Id,
            TourId = problem.TourId,
            ReporterId = problem.ReporterId,
            TourName = tourDto?.Name ?? string.Empty,
            ReporterName = reporter?.Username ?? string.Empty,
            Category = (API.Dtos.Tours.Problems.ProblemCategory)problem.Category,
            Priority = (API.Dtos.Tours.Problems.ProblemPriority)problem.Priority,
            Description = problem.Description,
            OccurredAt = problem.OccurredAt,
            CreatedAt = problem.CreatedAt,
            Comments = problem.Comments
                .Select(cid => {
                    var c = _repository.GetCommentById(cid);
                    var commentDto = _mapper.Map<CommentDto>(c);
                    var user = _userRepository.GetById(c.AuthorId);
                    commentDto.AuthorRole = user.Role.ToString();
                    return commentDto;
                })
                .ToList(),
            IsResolved = problem.IsResolved,
            Deadline = problem.Deadline,
            TourStatus = "Unknown"
        };

        return dto;
    }



    public List<CommentDto> GetComments(long id)
    {
        var problem = _repository.Get(id);

        var comments = _repository.GetCommentsByIds(problem.Comments);

        return comments.Select(c =>
        {
            var dto = _mapper.Map<CommentDto>(c);

            var user = _userRepository.GetById(c.AuthorId);
            dto.AuthorRole = user.Role.ToString();

            return dto;
        }).ToList();
    }

    private PagedResult<TourProblemDto> MapToDto(PagedResult<TourProblem> result)
    {
        var items = result.Results.Select(problem =>
        {
            var tourDto = new TourDto { Id = problem.TourId, Name = "Unknown", Status = "Unknown" };
            var reporter = _userRepository.GetById(problem.ReporterId);

            return new TourProblemDto
            {
                Id = problem.Id,
                TourId = problem.TourId,
                ReporterId = problem.ReporterId,
                TourName = tourDto?.Name ?? string.Empty,
                ReporterName = reporter?.Username ?? string.Empty,
                Category = (API.Dtos.Tours.Problems.ProblemCategory)problem.Category,
                Priority = (API.Dtos.Tours.Problems.ProblemPriority)problem.Priority,
                Description = problem.Description,
                OccurredAt = problem.OccurredAt,
                CreatedAt = problem.CreatedAt,
                Comments = problem.Comments
                    .Select(cid => {
                        var comment = _repository.GetCommentById(cid);
                        return _mapper.Map<CommentDto>(comment);
                    })
                    .ToList(),
                IsResolved = problem.IsResolved,
                Deadline = problem.Deadline,
                TourStatus = "Unknown"
            };
        }).ToList();

        return new PagedResult<TourProblemDto>(items, result.TotalCount);
    }



    public TourProblemDto MarkResolved(long problemId, bool isResolved)
    {
        _repository.MarkResolved(problemId, isResolved);

        var updatedProblem = _repository.Get(problemId)
            ?? throw new NotFoundException($"TourProblem {problemId} not found");

        return _mapper.Map<TourProblemDto>(updatedProblem);
    }


    public void SetDeadline(long problemId, DateTimeOffset? deadline)
    {
        if (!deadline.HasValue)
            throw new ArgumentException("Deadline cannot be null.");

        if (deadline.Value <= DateTimeOffset.UtcNow)
            throw new InvalidOperationException("Cannot set a deadline in the past.");

        _repository.UpdateDeadline(problemId, deadline);
    }

}