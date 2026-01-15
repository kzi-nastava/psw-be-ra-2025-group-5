using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Payments.API.Dtos;
using Explorer.Payments.API.Public;
using Explorer.Payments.Core.Domain;
using Explorer.Payments.Core.Domain.RepositoryInterfaces;
using Explorer.Tours.API.Internal;
using Explorer.Tours.API.Public.Tour;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payments.Core.UseCases
{
    public class BundleService : IBundleService
    {
        private readonly IBundleRepository _repository;
        private readonly IMapper _mapper;
        private readonly ITourSharedService _tourSharedService;

        public BundleService(IBundleRepository repository, IMapper mapper, ITourSharedService tourSharedService)
        {
            _repository = repository;
            _mapper = mapper;
            _tourSharedService = tourSharedService;
        }

        public BundleDto Get(long id)
        {
            var result = _repository.Get(id);
            return _mapper.Map<BundleDto>(result);
        }

        public List<BundleDto> GetByAuthor(long authorId)
        {
            var result = _repository.GetByAuthor(authorId);
            return _mapper.Map<List<BundleDto>>(result);
        }

        public BundleDto Create(BundleDto dto)
        {
            var bundle = new Bundle(dto.Name, dto.Price, dto.AuthorId, BundleStatus.Draft);

            if (dto.TourIds != null && dto.TourIds.Any())
            {
                foreach (var tourId in dto.TourIds)
                {
                    bundle.AddBundleItem(tourId);
                }
            }

            var result = _repository.Create(bundle);
            return _mapper.Map<BundleDto>(result);
        }

        public BundleDto Update(BundleDto dto)
        {
            var bundle = _repository.Get(dto.Id);

            bundle.Name = dto.Name;
            bundle.Price = dto.Price;

            bundle.ClearBundleItems();
            if (dto.TourIds != null && dto.TourIds.Any())
            {
                foreach (var tourId in dto.TourIds)
                {
                    bundle.AddBundleItem(tourId);
                }
            }

            var result = _repository.Update(bundle);
            return _mapper.Map<BundleDto>(result);
        }

        public void Delete(long id)
        {
            var bundle = _repository.Get(id);
            bundle.Delete();
            _repository.Delete(id);
        }

        public BundleDto PublishBundle(long bundleId)
        {
            var bundle = _repository.Get(bundleId);
            var tourIds = bundle.BundleItems.Select(bi => bi.TourId).ToList();

            int publishedCount = 0;
            foreach (var tourId in tourIds)
            {
                try
                {
                    var tour = _tourSharedService.Get(tourId);
                    
                    if (tour.Status == "Published")
                    {
                        publishedCount++;
                    }
                }
                catch
                {
                    continue;
                }
            }

            if (publishedCount < 2)
            {
                throw new InvalidOperationException("Bundle must contain at least 2 published tours.");
            }

            bundle.Publish();
            var result = _repository.Update(bundle);

            return _mapper.Map<BundleDto>(result);
        }

        public BundleDto ArchiveBundle(long bundleId)
        {
            var bundle = _repository.Get(bundleId);
            bundle.Archive();
            var result = _repository.Update(bundle);
            return _mapper.Map<BundleDto>(result);
        }

        public double GetTotalToursPrice(long bundleId)
        {
            var bundle = _repository.Get(bundleId);
            var tourIds = bundle.BundleItems.Select(bi => bi.TourId).ToList();

            double totalPrice = 0;
            foreach (var tourId in tourIds)
            {
                var tour = _tourSharedService.Get(tourId);
                totalPrice += tour.Price;
               
            }

            return totalPrice;
        }
    }
}
