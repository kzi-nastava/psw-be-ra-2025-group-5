using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;

namespace Explorer.Tours.Core.UseCases.Administration
{
    public class TouristEquipmentService : ITouristEquipmentService
    {
        private readonly ITouristEquipmentRepository _touristEquipmentRepository;
        private readonly IMapper _mapper;

        public TouristEquipmentService(ITouristEquipmentRepository repository, IMapper mapper)
        {
            _touristEquipmentRepository = repository;
            _mapper = mapper;
        }

        public PagedResult<TouristEquipmentDto> GetPaged(int page, int pageSize)
        {
            var result = _touristEquipmentRepository.GetPaged(page, pageSize);

            var items = result.Results.Select(_mapper.Map<TouristEquipmentDto>).ToList();
            return new PagedResult<TouristEquipmentDto>(items, result.TotalCount);
        }

        public TouristEquipmentDto Create(TouristEquipmentDto entity)
        {
            var result = _touristEquipmentRepository.Create(_mapper.Map<TouristEquipment>(entity));
            return _mapper.Map<TouristEquipmentDto>(result);
        }

        public TouristEquipmentDto Update(TouristEquipmentDto entity)
        {
            var result = _touristEquipmentRepository.Update(_mapper.Map<TouristEquipment>(entity));
            return _mapper.Map<TouristEquipmentDto>(result);
        }

        public void Delete(long id)
        {
            _touristEquipmentRepository.Delete(id);
        }
    }
}
