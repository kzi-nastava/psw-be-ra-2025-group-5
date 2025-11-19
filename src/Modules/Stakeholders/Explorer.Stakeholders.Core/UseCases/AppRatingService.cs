using AutoMapper;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using System.Collections.Generic;

namespace Explorer.Stakeholders.Core.UseCases
{
    public class AppRatingService : IAppRatingService
    {
        private readonly IAppRatingRepository _repository;
        private readonly IMapper _mapper;

        public AppRatingService(IAppRatingRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public AppRatingDto Create(AppRatingDto dto)
        {
            try
            {
                var entity = _mapper.Map<AppRating>(dto);
                var result = _repository.Create(entity);
                return _mapper.Map<AppRatingDto>(result);
            }
            catch (Exception ex)
            {
                throw new Exception($"CREATE ERROR: {ex.Message}", ex);
            }
        }


        public AppRatingDto Update(AppRatingDto dto)
        {
            var existing = _repository.Get(dto.Id);
            if (existing == null)
                throw new KeyNotFoundException($"AppRating sa Id={dto.Id} ne postoji.");

            existing.Update(dto.Rating, dto.Comment); 
            _repository.Update(existing); 

            return _mapper.Map<AppRatingDto>(existing);
        }


        public void Delete(long id)
        {
            var existing = _repository.Get(id);
            if (existing == null)
                throw new KeyNotFoundException($"AppRating sa Id={id} ne postoji.");

            _repository.Delete(id);
        }


        public IEnumerable<AppRatingDto> GetAll()
        {
            var entities = _repository.GetAll();
            return _mapper.Map<IEnumerable<AppRatingDto>>(entities);
        }
        public AppRatingDto Get(long id)
        {
            var entity = _repository.Get(id);
            return _mapper.Map<AppRatingDto>(entity);
        }
        public IEnumerable<AppRatingDto> GetByUserId(long userId)
        {
            return _repository.GetAll()
                              .Where(r => r.UserId == userId)
                              .Select(r => _mapper.Map<AppRatingDto>(r));
        }

    }
}
