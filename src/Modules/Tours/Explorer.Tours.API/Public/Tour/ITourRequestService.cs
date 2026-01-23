using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos.Tours;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.API.Public.Tour
{
    public interface ITourRequestService
    {
        TourRequestDto Create(TourRequestDto entity);
        PagedResult<TourRequestDto> GetByTourist(long touristId, int page, int pageSize);
        PagedResult<TourRequestDto> GetByAuthor(long authorId, int page, int pageSize);
    }
}
