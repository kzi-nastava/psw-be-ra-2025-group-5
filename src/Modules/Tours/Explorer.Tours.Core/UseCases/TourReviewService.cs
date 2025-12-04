using AutoMapper;
using Explorer.BuildingBlocks.Core.Exceptions;
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
    public class TourReviewService : ITourReviewService
    {
        private readonly ITourReviewRepository _reviewRepo;
        private readonly ITourRepository _tourRepo;
        private readonly IMapper _mapper;

        public TourReviewService(ITourReviewRepository reviewRepo, ITourRepository tourRepo, IMapper mapper)
        {
            _reviewRepo = reviewRepo;
            _tourRepo = tourRepo;
            _mapper = mapper;
        }

        // ===== Paged queries =====
        public PagedResult<TourReviewDto> GetPaged(int page, int pageSize)
        {
            var result = _reviewRepo.GetPaged(page, pageSize);
            var items = result.Results.Select(_mapper.Map<TourReviewDto>).ToList();
            return new PagedResult<TourReviewDto>(items, result.TotalCount);
        }

        public PagedResult<TourReviewDto> GetByTour(long tourId, int page, int pageSize)
        {
            // Provera da li tura postoji
            var tour = _tourRepo.Get(tourId);
            if (tour == null)
                throw new NotFoundException($"Tour {tourId} not found");

            var result = _reviewRepo.GetByTour(tourId, page, pageSize);
            var items = result.Results.Select(_mapper.Map<TourReviewDto>).ToList();
            return new PagedResult<TourReviewDto>(items, result.TotalCount);
        }

        public PagedResult<TourReviewDto> GetByTourist(long touristId, int page, int pageSize)
        {
            var result = _reviewRepo.GetByTourist(touristId, page, pageSize);
            var items = result.Results.Select(_mapper.Map<TourReviewDto>).ToList();
            return new PagedResult<TourReviewDto>(items, result.TotalCount);
        }

        // ===== Single review =====
        public TourReviewDto Get(long id)
        {
            var review = _reviewRepo.Get(id);
            if (review == null)
                throw new NotFoundException($"Review {id} not found");

            return _mapper.Map<TourReviewDto>(review);
        }

        // ===== Create =====
        public TourReviewDto Create(TourReviewDto dto)
        {
            // Provera da li tura postoji
            var tour = _tourRepo.Get(dto.TourID);
            if (tour == null)
                throw new NotFoundException($"Tour {dto.TourID} not found");

            // Mapiranje i kreiranje
            var review = _mapper.Map<TourReview>(dto);

            var created = _reviewRepo.Create(review);
            return _mapper.Map<TourReviewDto>(created);
        }

        // ===== Update =====
        public TourReviewDto Update(long reviewId, TourReviewDto dto)
        {
            // Provera da li review postoji
            var existing = _reviewRepo.Get(reviewId);
            if (existing == null)
                throw new NotFoundException($"Review {reviewId} not found");

            // Opcionalno: proveri da li tour postoji
            var tour = _tourRepo.Get(dto.TourID);
            if (tour == null)
                throw new NotFoundException($"Tour {dto.TourID} not found");

            // Mapiranje i update
            var review = _mapper.Map<TourReview>(dto);

            var updated = _reviewRepo.Update(review);
            return _mapper.Map<TourReviewDto>(updated);
        }

        // ===== Delete =====
        public void Delete(long reviewId)
        {
            var existing = _reviewRepo.Get(reviewId);
            if (existing == null)
                throw new NotFoundException($"Review {reviewId} not found");

            _reviewRepo.Delete(reviewId);
        }
    }
}
