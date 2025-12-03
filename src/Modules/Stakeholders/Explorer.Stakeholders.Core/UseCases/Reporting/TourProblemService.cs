using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public.Reporting;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using System.Data;

namespace Explorer.Stakeholders.Core.UseCases.Reporting;

public class TourProblemService : ITourProblemService
{
    private readonly ITourProblemRepository _repository;
    private readonly IMapper _mapper;

    public TourProblemService(ITourProblemRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public PagedResult<TourProblemDto> GetPaged(int page, int pageSize)
    {
        var result = _repository.GetPaged(page, pageSize);

        var items = result.Results.Select(problem => new TourProblemDto
        {
            Id = problem.Id,
            TourId = problem.TourId,
            ReporterId = problem.ReporterId,
            Category = (API.Dtos.ProblemCategory)problem.Category,
            Priority = (API.Dtos.ProblemPriority)problem.Priority,
            Description = problem.Description,
            OccurredAt = problem.OccurredAt,
            CreatedAt = problem.CreatedAt,
            Comments = problem.Comments
                .Select(cid => {
                    var comment = _repository.GetCommentById(cid);
                    return _mapper.Map<CommentDto>(comment);
                })
                .ToList()
        }).ToList();

        return new PagedResult<TourProblemDto>(items, result.TotalCount);
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

        return _mapper.Map<CommentDto>(comment);
    }



    public TourProblemDto GetById(long id)
    {
        var problem = _repository.Get(id);

        var dto = new TourProblemDto
        {
            Id = problem.Id,
            TourId = problem.TourId,
            ReporterId = problem.ReporterId,
            Category = (API.Dtos.ProblemCategory)problem.Category,
            Priority = (API.Dtos.ProblemPriority)problem.Priority,
            Description = problem.Description,
            OccurredAt = problem.OccurredAt,
            CreatedAt = problem.CreatedAt,
            Comments = problem.Comments
                .Select(cid => {
                    var c = _repository.GetCommentById(cid);
                    return _mapper.Map<CommentDto>(c);
                })
                .ToList()
        };

        return dto;
    }



    public List<CommentDto> GetComments(long id)
    {
        var problem = _repository.Get(id);

        var comments = _repository.GetCommentsByIds(problem.Comments);

        return comments.Select(c => _mapper.Map<CommentDto>(c)).ToList();
    }


}