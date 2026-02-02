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

        public PlannerDto GeneratePlan(long touristId, PlannerGenerationOptionsDto options)
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

            var plannerDays = new Dictionary<DateOnly, PlannerDay>();
            PlannerDay GetOrCreateDay(DateOnly date)
            {
                if (!plannerDays.ContainsKey(date))
                {
                    var day = new PlannerDay(date);
                    plannerDays[date] = day;
                    planner.AddDay(day);
                }

                return plannerDays[date];
            }

            var startDate = options.StartDate;
            int dayIndex = 0;
            int toursInCurrentDay = 0;

            foreach (var tour in tours)
            {
                if (dayIndex >= options.NumberOfDays)
                    break;

                var date = startDate.AddDays(dayIndex);
                var day = GetOrCreateDay(date);

                if (day.TimeBlocks.Count >= 2)  
                {
                    dayIndex++;
                    toursInCurrentDay = 0;
                    if (dayIndex >= options.NumberOfDays)
                        break;

                    date = startDate.AddDays(dayIndex);
                    day = GetOrCreateDay(date);
                }

                var durationByTransport = _tourSharedService.GetDurationsByTransport(new[] { tour.Id }, TransportType.Walking.ToString());

                if (!durationByTransport.TryGetValue(tour.Id, out var duration))
                    continue;

                var transportType = previousTransportTypes.TryGetValue(tour.Id, out var t) ? t : TransportType.Walking;

                TimeOnly startTime = FindFirstAvailableSlot(day, duration);
                if (startTime == default)
                {
                    dayIndex++;
                    toursInCurrentDay = 0;
                    if (dayIndex >= options.NumberOfDays)
                        break;

                    date = startDate.AddDays(dayIndex);
                    day = GetOrCreateDay(date);
                    startTime = FindFirstAvailableSlot(day, duration);
                    if (startTime == default)
                        continue; 
                }

                var endTime = startTime.AddMinutes(duration);

                var block = new PlannerTimeBlock(tour.Id, startTime, endTime, transportType);
                day.AddBlock(block, duration);

                toursInCurrentDay++;
            }

            planner = _plannerRepository.Update(planner);

            return _mapper.Map<PlannerDto>(planner);
        }

        private TimeOnly FindFirstAvailableSlot(PlannerDay day, int duration)
        {
            var workStart = new TimeOnly(9, 0);
            var workEnd = new TimeOnly(21, 0);
            var breakMinutes = 60; 

            var sortedBlocks = day.TimeBlocks.OrderBy(b => b.TimeRange.Start).ToList();

            TimeOnly current = workStart;

            foreach (var block in sortedBlocks)
            {
                if ((block.TimeRange.Start - current).TotalMinutes >= duration + breakMinutes)
                    return current;

                current = block.TimeRange.End.AddMinutes(breakMinutes);
            }

            if ((workEnd - current).TotalMinutes >= duration)
                return current;

            return default;
        }

    }

}
