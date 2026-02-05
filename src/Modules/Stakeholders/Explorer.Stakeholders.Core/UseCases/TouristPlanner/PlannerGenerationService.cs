using AutoMapper;
using Explorer.BuildingBlocks.Core.Exceptions;
using Explorer.Stakeholders.API.Dtos.TouristPlanner;
using Explorer.Stakeholders.API.Public.TouristPlanner;
using Explorer.Stakeholders.API.Public.Users;
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
        private readonly IPremiumService _premiumService;
        private readonly IMapper _mapper;

        public PlannerGenerationService(IPlannerRepository plannerRepository, ITourSharedService tourSharedService, IPremiumService premiumService, IMapper mapper)
        {
            _plannerRepository = plannerRepository;
            _tourSharedService = tourSharedService;
            _premiumService = premiumService;
            _mapper = mapper;
        }

        public PlannerDto GeneratePlan(long touristId, PlannerGenerationOptionsDto options, long userId)
        {
            if (!_premiumService.IsPremium(userId))
            {
                throw new ForbiddenException("Plan generation is only available for premium users.");
            }

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

                var slot = FindFirstAvailableSlot(day, duration);
                if (slot == null)
                {
                    dayIndex++;
                    toursInCurrentDay = 0;
                    if (dayIndex >= options.NumberOfDays)
                        break;

                    date = startDate.AddDays(dayIndex);
                    day = GetOrCreateDay(date);
                    slot = FindFirstAvailableSlot(day, duration);
                    if (slot == null)
                        continue;
                }

                var block = new PlannerTimeBlock(tour.Id, slot.Start, slot.End, transportType);
                day.AddBlock(block, duration);

                toursInCurrentDay++;
            }


            planner = _plannerRepository.Update(planner);

            return _mapper.Map<PlannerDto>(planner);
        }

        private TimeRange FindFirstAvailableSlot(PlannerDay day, int durationMinutes)
        {
            var workStart = new TimeOnly(9, 0);
            var workEnd = new TimeOnly(21, 0);
            var breakMinutes = 60;

            var orderedBlocks = day.TimeBlocks.OrderBy(b => b.TimeRange.Start).ToList();

            var pointer = workStart;

            foreach (var block in orderedBlocks)
            {
                pointer = PlannerDay.SnapUpToQuarterHour(pointer);

                if ((block.TimeRange.Start - pointer).TotalMinutes >= durationMinutes + breakMinutes)
                {
                    var endTime = pointer.AddMinutes(durationMinutes);
                    return new TimeRange(pointer, endTime);
                }

                pointer = block.TimeRange.End.AddMinutes(breakMinutes);
            }

            pointer = PlannerDay.SnapUpToQuarterHour(pointer);
            if ((workEnd - pointer).TotalMinutes >= durationMinutes)
                return new TimeRange(pointer, pointer.AddMinutes(durationMinutes));

            return null;
        }

    }

}
