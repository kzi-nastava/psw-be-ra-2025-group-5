using Explorer.BuildingBlocks.Core.Exceptions;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Explorer.Stakeholders.Infrastructure.Database.Repositories;

public class TourProblemDbRepository : ITourProblemRepository
{
    protected readonly StakeholdersContext DbContext;
    private readonly DbSet<TourProblem> _dbSet;

    public TourProblemDbRepository(StakeholdersContext dbContext)
    {
        DbContext = dbContext;
        _dbSet = DbContext.Set<TourProblem>();
    }

    public PagedResult<TourProblem> GetPaged(int page, int pageSize)
    {
        var task = _dbSet.GetPagedById(page, pageSize);
        task.Wait();
        return task.Result;
    }

    public PagedResult<TourProblem> GetPagedByReporterId(long reporterId, int page, int pageSize)
    {
        var filteredQuery = _dbSet.Where(p => p.ReporterId == reporterId);

        var task = filteredQuery.GetPagedById(page, pageSize);
        task.Wait();
        return task.Result;
    }

    public PagedResult<TourProblem> GetPagedByTourIds(List<long> tourIds, int page, int pageSize)
    {
        var filteredQuery = _dbSet.Where(p => tourIds.Contains(p.TourId));

        var task = filteredQuery.GetPagedById(page, pageSize);
        task.Wait();
        return task.Result;
    }

    public TourProblem Get(long id)
    {
        var entity = _dbSet.Find(id);
        if (entity == null) throw new NotFoundException("Not found: " + id);
        return entity;
    }

    public TourProblem Create(TourProblem entity)
    {
        _dbSet.Add(entity);
        DbContext.SaveChanges();
        return entity;
    }

    public TourProblem Update(TourProblem entity)
    {
        try
        {
            DbContext.Update(entity);
            DbContext.SaveChanges();
        }
        catch (DbUpdateException e)
        {
            throw new NotFoundException(e.Message);
        }
        return entity;
    }

    public void Delete(long id)
    {
        var entity = Get(id);
        _dbSet.Remove(entity);
        DbContext.SaveChanges();
    }

    public TourProblem GetWithComments(long id)
    {
        var entity = DbContext.TourProblems.Find(id);
        if (entity == null)
            throw new NotFoundException($"TourProblem {id} not found");

        var comments = DbContext.Comments
            .Where(c => entity.Comments.Contains(c.CommentId))
            .ToList();

        return entity;
    }


    public void AddComment(Comment comment)
    {
        DbContext.Comments.Add(comment);
        DbContext.SaveChanges();
    }
    public Comment GetCommentById(long commentId)
    {
        var comment = DbContext.Comments.Find(commentId);
        if (comment == null) throw new NotFoundException($"Comment not found: {commentId}");
        return comment;
    }

    public List<Comment> GetCommentsByIds(List<long> ids)
    {
        return DbContext.Comments
            .Where(c => ids.Contains(c.CommentId))
            .ToList();
    }

    public void MarkResolved(long problemId, bool isResolved)
    {
        var problem = Get(problemId);

        problem.IsResolved = isResolved;

        DbContext.Update(problem);
        DbContext.SaveChanges();
    }

    public void UpdateDeadline(long problemId, DateTimeOffset? deadline)
    {
        var problem = _dbSet.Find(problemId);
        if (problem == null)
            throw new NotFoundException($"TourProblem {problemId} not found");

        DbContext.Entry(problem).Property(p => p.Deadline).CurrentValue = deadline;
        DbContext.SaveChanges();
    }
}