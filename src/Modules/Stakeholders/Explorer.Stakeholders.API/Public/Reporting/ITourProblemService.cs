using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;

namespace Explorer.Stakeholders.API.Public.Reporting
{
    public interface ITourProblemService
    {
        PagedResult<TourProblemDto> GetPaged(int page, int pageSize);
        TourProblemDto Create(TourProblemDto problem);
        TourProblemDto Update(TourProblemDto problem);
        void Delete(long id);
        CommentDto AddComment(long problemId, long authorId, string content);
        TourProblemDto GetById(long id);
        List<CommentDto> GetComments(long id);
        PagedResult<TourProblemDto> GetPagedByReporterId(long reporterId, int page, int pageSize);
        PagedResult<TourProblemDto> GetPagedByTourIds(List<long> tourIds, int page, int pageSize);
    }
}