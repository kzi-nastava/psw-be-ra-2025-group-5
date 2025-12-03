using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Core.UseCases
{
    public class TourReviewService: ITourReviewService
    {
        private readonly ITourReviewRepository _repository;
        private readonly IMapper _mapper;

        public TourReviewService(ITourReviewRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public PagedResult<TourReviewDto> GetPaged(int page, int pageSize)
        {
            var result = _repository.GetPaged(page, pageSize);
            var items = result.Results.Select(_mapper.Map<TourReviewDto>).ToList();
            return new PagedResult<TourReviewDto>(items, result.TotalCount);
        }
        public PagedResult<TourReviewDto> GetByTour(long tourId, int page, int pageSize)
        {
            var result = _repository.GetByTour(tourId, page, pageSize);
            var items = result.Results.Select(_mapper.Map<TourReviewDto>).ToList();
            return new PagedResult<TourReviewDto>(items, result.TotalCount);
        }
        public PagedResult<TourReviewDto> GetByTourist(long touristId, int page, int pageSize)
        {
            var result = _repository.GetByTour(touristId, page, pageSize);
            var items = result.Results.Select(_mapper.Map<TourReviewDto>).ToList();
            return new PagedResult<TourReviewDto>(items, result.TotalCount);
        }
        public TourReviewDto Get(long id)
        {
            var review = _repository.Get(id);
            return _mapper.Map<TourReviewDto>(review);
        }
        public TourReviewDto Create(TourReviewDto dto)
        {
            var review = _mapper.Map<TourReview>(dto);
            var result = _repository.Create(review);
            return _mapper.Map<TourReviewDto>(result);
        }
        public TourReviewDto Update(long id, TourReviewDto dto)
        {
            var review = _mapper.Map<TourReview>(dto);
            var result = _repository.Update(review);
            return _mapper.Map<TourReviewDto>(result);
        }
        public void Delete(long id)
        {
            _repository.Delete(id);
        }
    }
}
