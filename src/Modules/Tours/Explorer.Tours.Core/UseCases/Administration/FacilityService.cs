using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Public.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Explorer.Tours.Core.Domain.Facilities;
using Explorer.Tours.Core.Domain.RepositoryInterfaces.Facilities;
using Explorer.Tours.API.Dtos.Facilities;


namespace Explorer.Tours.Core.UseCases.Administration
{
    public class FacilityService : IFacilityService
    {
        private readonly IFacilityRepository _facilityRepository;
        private readonly IMapper _mapper;
        public FacilityService(IFacilityRepository repository, IMapper mapper)
        {
            _facilityRepository = repository;
            _mapper = mapper;
        }

        public FacilityDto Create(FacilityDto entity)
        {
            var result = _facilityRepository.Create(_mapper.Map<Facility>(entity));
            return _mapper.Map<FacilityDto>(result);
        }

        public void Delete(long id)
        {
            _facilityRepository.Delete(id);
        }

        public PagedResult<FacilityDto> GetPaged(int page, int pageSize)
        {
            var result = _facilityRepository.GetPaged(page, pageSize);

            var items = result.Results.Select(_mapper.Map<FacilityDto>).ToList();
            return new PagedResult<FacilityDto>(items, result.TotalCount);
        }

        public FacilityDto Update(FacilityDto entity)
        {
            var result = _facilityRepository.Update(_mapper.Map<Facility>(entity));
            return _mapper.Map<FacilityDto>(result);
        }

        public IEnumerable<FacilityDto> GetAllForTourists()
        {
            var facilities = _facilityRepository.GetAllForTourists();
            return facilities.Select(_mapper.Map<FacilityDto>).ToList();
        }

    }
}
