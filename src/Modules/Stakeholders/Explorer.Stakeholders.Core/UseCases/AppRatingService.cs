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
        private readonly IUserRepository _userRepository;


        public AppRatingService(IAppRatingRepository repository, IUserRepository userRepository, IMapper mapper)
        {
            _repository = repository;
            _userRepository = userRepository; 
            _mapper = mapper;
        }

        public AppRatingDto Create(AppRatingDto dto)
        {
            try
            {
                var entity = _mapper.Map<AppRating>(dto);
                entity.CreatedAt = DateTime.UtcNow;   
                entity.UpdatedAt = DateTime.UtcNow;  
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
            var dtos = _mapper.Map<IEnumerable<AppRatingDto>>(entities).ToList();

           
            foreach (var dto in dtos)
            {
                var user = _userRepository.GetById(dto.UserId);
                dto.Username = user?.Username;
            }

            return dtos;
        }
        public AppRatingDto Get(long id)
        {
            var entity = _repository.Get(id);
            return _mapper.Map<AppRatingDto>(entity);
        }
        public IEnumerable<AppRatingDto> GetByUserId(long userId)
        {
            var user = _userRepository.GetById(userId);
            return _repository.GetAll()
                              .Where(r => r.UserId == userId)
                              .Select(r => {
                                  var dto = _mapper.Map<AppRatingDto>(r);
                                  dto.Username = user?.Username;
                                  return dto;
                              });
        }

    }
}
