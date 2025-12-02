using Explorer.BuildingBlocks.Core.UseCases;

namespace Explorer.Stakeholders.Core.Domain.RepositoryInterfaces
{
    public interface ITourProblemRepository
    {
        PagedResult<TourProblem> GetPaged(int page, int pageSize);
        TourProblem Create(TourProblem entity);
        TourProblem Update(TourProblem entity);
        void Delete(long id);
        TourProblem Get(long id);
        TourProblem GetWithComments(long id);
        void AddComment(Comment comment);
        Comment GetCommentById(long commentId);
        public List<Comment> GetCommentsByIds(List<long> ids);
    }
}