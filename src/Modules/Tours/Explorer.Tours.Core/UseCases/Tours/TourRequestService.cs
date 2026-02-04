using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos.Tours;
using Explorer.Tours.API.Public.Tour;
using Explorer.Tours.Core.Domain.RepositoryInterfaces.Tours;
using Explorer.Tours.Core.Domain.Tours.Entities;
using Explorer.Stakeholders.API.Internal;
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
        private readonly IUserInfoService _userInfoService;
        private readonly IMapper _mapper;

        public TourRequestService(ITourRequestRepository repository, IUserInfoService userInfoService, IMapper mapper)
        {
            _tourRequestRepository = repository;
            _userInfoService = userInfoService;
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

        public List<object> GetAllAuthors()
        {
            var authors = _userInfoService.GetAuthors();

            var result = new List<object>();

            foreach (var author in authors)
            {
                result.Add(new
                {
                    Id = author.Id,
                    Username = author.Username,
                    Name = author.Name + " " + author.Surname
                });
            }

            return result;
        }

        public TourRequestDto Update(long id, TourRequestDto dto)
        {
            var tourRequest = _tourRequestRepository.Get(id);
            _mapper.Map(dto, tourRequest);
            var result = _tourRequestRepository.Update(tourRequest);

            return _mapper.Map<TourRequestDto>(result);
        }

        public void Delete(long id)
        {
            _tourRequestRepository.Delete(id);
        }

        public TourRequestDto Get(long id)
        {
            var result = _tourRequestRepository.Get(id);
            return _mapper.Map<TourRequestDto>(result);
        }

        public TourRequestDto Accept(long requestId, long authorId)
        {
            var request = _tourRequestRepository.Get(requestId)
                ?? throw new KeyNotFoundException("Tour request not found.");

            if (request.AuthorId != authorId)
                throw new UnauthorizedAccessException("Author cannot manage this request.");

            if (request.Status != TourRequestStatus.Pending)
                throw new InvalidOperationException("Only pending requests can be processed.");

            request.Accept();
            _tourRequestRepository.Update(request);

            return _mapper.Map<TourRequestDto>(request);
        }

        public TourRequestDto Decline(long requestId, long authorId)
        {
            var request = _tourRequestRepository.Get(requestId)
                ?? throw new KeyNotFoundException("Tour request not found.");

            if (request.AuthorId != authorId)
                throw new UnauthorizedAccessException("Author cannot manage this request.");

            if (request.Status != TourRequestStatus.Pending)
                throw new InvalidOperationException("Only pending requests can be processed.");

            request.Decline();
            _tourRequestRepository.Update(request);

            return _mapper.Map<TourRequestDto>(request);
        }

        
    }
}
