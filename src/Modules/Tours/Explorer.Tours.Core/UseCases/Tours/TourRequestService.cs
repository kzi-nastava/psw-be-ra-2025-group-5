using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos.Tours;
using Explorer.Tours.API.Public.Tour;
using Explorer.Tours.Core.Domain.RepositoryInterfaces.Tours;
using Explorer.Tours.Core.Domain.Tours.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Core.UseCases.Tours
{
    public class TourRequestService : ITourRequestService
    {
        private readonly ITourRequestRepository _tourRequestRepository;
        private readonly IMapper _mapper;

        public TourRequestService(ITourRequestRepository repository, IMapper mapper)
        {
            _tourRequestRepository = repository;
            _mapper = mapper;
        }

        public TourRequestDto Create(TourRequestDto entity)
        {
            var result = _tourRequestRepository.Create(_mapper.Map<TourRequest>(entity));
            return _mapper.Map<TourRequestDto>(result);
        }

        public PagedResult<TourRequestDto> GetByTourist(long touristId, int page, int pageSize)
        {
            var result = _tourRequestRepository.GetPagedByTourist(touristId, page, pageSize);

            var items = result.Results.Select(_mapper.Map<TourRequestDto>).ToList();
            return new PagedResult<TourRequestDto>(items, result.TotalCount);
        }

        public PagedResult<TourRequestDto> GetByAuthor(long authorId, int page, int pageSize)
        {
            var result = _tourRequestRepository.GetPagedByAuthor(authorId, page, pageSize);

            var items = result.Results.Select(_mapper.Map<TourRequestDto>).ToList();
            return new PagedResult<TourRequestDto>(items, result.TotalCount);
        }
    }
}
