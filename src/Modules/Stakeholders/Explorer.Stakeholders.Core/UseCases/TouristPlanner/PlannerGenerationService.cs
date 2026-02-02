using AutoMapper;
using Explorer.Stakeholders.API.Dtos.TouristPlanner;
using Explorer.Stakeholders.API.Public.TouristPlanner;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces.TouristPlanner;
using Explorer.Stakeholders.Core.Domain.TouristPlanner;
using Explorer.Tours.API.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Core.UseCases.TouristPlanner
{
    public class PlannerGenerationService : IPlannerGenerationService
    {
        private readonly IPlannerRepository _plannerRepository;
        private readonly ITourSharedService _tourSharedService;
        private readonly IMapper _mapper;

        public PlannerGenerationService(IPlannerRepository plannerRepository, ITourSharedService tourSharedService, IMapper mapper)
        {
            _plannerRepository = plannerRepository;
            _tourSharedService = tourSharedService;
            _mapper = mapper;
        }

        public PlannerDto GeneratePlan(long touristId)
        {
            var previousPlanner = _plannerRepository.GetByTouristId(touristId);
            var previousTransportTypes = new Dictionary<long, TransportType>();

            if (previousPlanner != null)
            {
                foreach (var day in previousPlanner.Days)
                {
                    foreach (var block in day.TimeBlocks)
                    {
                        previousTransportTypes[block.TourId] = block.TransportType;
                    }
                }
                
                _plannerRepository.DeleteByTouristId(touristId);
            }

            var planner = _plannerRepository.Create(touristId);

            var tours = _tourSharedService.GetPurchased(touristId);
            if (!tours.Any())
            {
                return _mapper.Map<PlannerDto>(planner);
            }

            var startDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1));
            int dayOffset = 0;

            foreach (var tour in tours)
            {
                var currentDate = startDate.AddDays(dayOffset);
                var day = new PlannerDay(currentDate);

                var durationByTransport = _tourSharedService.GetDurationsByTransport(new[] { tour.Id }, TransportType.Walking.ToString());
                if (!durationByTransport.TryGetValue(tour.Id, out var duration))
                    continue;

                var currentStartTime = new TimeOnly(9, 0); 
                var endTime = currentStartTime.AddMinutes(duration);

                TransportType transportType = previousTransportTypes.ContainsKey(tour.Id) ? previousTransportTypes[tour.Id]: TransportType.Walking;
                var block = new PlannerTimeBlock(tour.Id, currentStartTime, endTime, transportType);
                day.AddBlock(block, duration);

                planner.AddDay(day);

                dayOffset += 2;
            }

            planner = _plannerRepository.Update(planner);

            return _mapper.Map<PlannerDto>(planner);
        }

    }
}
