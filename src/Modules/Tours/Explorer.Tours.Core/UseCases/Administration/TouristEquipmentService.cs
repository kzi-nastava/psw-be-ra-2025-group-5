using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos.Equipments;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.Core.Domain.Equipments.Entities;
using Explorer.Tours.Core.Domain.RepositoryInterfaces.Equipments;

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

        public PagedResult<TouristEquipmentDto> GetPaged(long touristId, int page, int pageSize)
        {
            var result = _touristEquipmentRepository.GetPagedByTouristId(touristId, page, pageSize);
            var items = result.Results.Select(_mapper.Map<TouristEquipmentDto>).ToList();
            return new PagedResult<TouristEquipmentDto>(items, result.TotalCount);
        }

        public TouristEquipmentDto Create(TouristEquipmentDto entity)
        {
            var result = _touristEquipmentRepository.Create(_mapper.Map<TouristEquipment>(entity));
            return _mapper.Map<TouristEquipmentDto>(result);
        }

        /*public TouristEquipmentDto Update(TouristEquipmentDto entity, long touristId)
        {

            var existing = _touristEquipmentRepository.Get(entity.Id);

            if (existing.TouristId != touristId)
            {
                throw new UnauthorizedAccessException("Nije dozvoljeno menjati tuđu opremu.");
            }

            var result = _touristEquipmentRepository.Update(_mapper.Map<TouristEquipment>(entity));
            return _mapper.Map<TouristEquipmentDto>(result);
        }*/

        public TouristEquipmentDto Update(TouristEquipmentDto entity, long touristId)
        {
            var existing = _touristEquipmentRepository.Get(entity.Id);

            if (existing.TouristId != touristId)
            {
                throw new UnauthorizedAccessException("Nije dozvoljeno menjati tuđu opremu.");
            }

            _mapper.Map(entity, existing);

            var result = _touristEquipmentRepository.Update(existing);

            return _mapper.Map<TouristEquipmentDto>(result);
        }

        public void Delete(long id, long touristId)
        {
            var existing = _touristEquipmentRepository.Get(id);

            if (existing.TouristId != touristId)
            {
                throw new UnauthorizedAccessException("Nije dozvoljeno brisati tuđu opremu.");
            }

            _touristEquipmentRepository.Delete(id);
        }
    }
}