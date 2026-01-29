using AutoMapper;
using Explorer.BuildingBlocks.Core.Exceptions;
using Explorer.Stakeholders.API.Dtos.TouristPlanner;
using Explorer.Stakeholders.API.Public.TouristPlanner;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces.TouristPlanner;
using Explorer.Stakeholders.Core.Domain.TouristPlanner;
using Explorer.Tours.API.Internal;

namespace Explorer.Stakeholders.Core.UseCases.TouristPlanner;

public class PlannerService : IPlannerService
{
    private readonly IPlannerRepository _plannerRepository;
    private readonly IMapper _mapper;

    private readonly IPlannerValidationService _validationService;
    private readonly ITourSharedService _tourSharedService;

    public PlannerService(IPlannerRepository plannerRepository, IMapper mapper, ITourSharedService tourSharedService, IPlannerValidationService validationService)
    {
        _plannerRepository = plannerRepository;
        _mapper = mapper;
        _tourSharedService = tourSharedService;
        _validationService = validationService;
        
    }

    public PlannerDto GetOrCreatePlanner(long touristId)
    {
        return _mapper.Map<PlannerDto>(GetOrCreatePlannerEntity(touristId));
    }

    public PlannerDayDto GetDay(long touristId, DateOnly date)
    {
        var planner = GetOrCreatePlannerEntity(touristId);
        return _mapper.Map<PlannerDayDto>(planner.Days.FirstOrDefault(d => d.Date == date)) ?? throw new NotFoundException("Day not found");
    }

    public PlannerDayDto AddBlock(long touristId, DateOnly date, CreatePlannerTimeBlockDto dto)
    {
        var planner = GetOrCreatePlannerEntity(touristId);
        var day = planner.Days.FirstOrDefault(d => d.Date == date);
        if (day == null) {
            day = new PlannerDay(date);
            planner.AddDay(day);
        }

        var transportType = ParseTransportType(dto.TransportType);
        var block = new PlannerTimeBlock(dto.TourId, dto.StartTime, dto.EndTime, transportType);

        day.AddBlock(block, dto.Duration);

        var tourIds = day.TimeBlocks.Select(b => b.TourId).Distinct();

        var systemDurations = _tourSharedService.GetDurationsByTransport(tourIds, transportType.ToString());

        var dayDto = _mapper.Map<PlannerDayDto>(day);
        dayDto.Warnings = _validationService.ValidateDay(dayDto, systemDurations);

        _plannerRepository.Update(planner);

        return dayDto;
    }

    public PlannerDayDto RemoveBlock(long touristId, DateOnly date, long blockId)
    {
        var planner = GetOrCreatePlannerEntity(touristId);
        var day = planner.Days.FirstOrDefault(d => d.Date == date) ?? throw new NotFoundException("Day not found");

        day.RemoveBlock(blockId);
        _plannerRepository.Update(planner);

        return _mapper.Map<PlannerDayDto>(day);
    }

    public PlannerDayDto RescheduleBlock(long touristId, DateOnly date, long blockId, CreatePlannerTimeBlockDto dto)
    {
        var planner = GetOrCreatePlannerEntity(touristId);
        var day = planner.Days.FirstOrDefault(d => d.Date == date);

        if(day == null) return AddBlock(touristId, date, dto);

        var transportType = ParseTransportType(dto.TransportType);
        day.RescheduleBlock(blockId, dto.StartTime, dto.EndTime, transportType);

        var tourIds = day.TimeBlocks.Select(b => b.TourId).Distinct();

        var systemDurations = _tourSharedService.GetDurationsByTransport(tourIds, transportType.ToString());

        var dayDto = _mapper.Map<PlannerDayDto>(day);
        dayDto.Warnings = _validationService.ValidateDay(dayDto, systemDurations);

        _plannerRepository.Update(planner);

        return dayDto;
    }

    private Planner GetOrCreatePlannerEntity(long touristId)
    {
        return _plannerRepository.GetByTouristId(touristId) ?? _plannerRepository.Create(touristId);
    }

    private static TransportType ParseTransportType(string transportType)
    {
        if (string.IsNullOrEmpty(transportType))
            return TransportType.Walking;

        if (!Enum.TryParse<TransportType>(transportType, true, out var result))
            throw new Exception($"Invalid transport type: {transportType}");

        return result;
    }

}
