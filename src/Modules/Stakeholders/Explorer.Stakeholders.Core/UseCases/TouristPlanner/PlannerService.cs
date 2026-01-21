using AutoMapper;
using Explorer.BuildingBlocks.Core.Exceptions;
using Explorer.Stakeholders.API.Dtos.TouristPlanner;
using Explorer.Stakeholders.API.Public.TouristPlanner;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces.TouristPlanner;
using Explorer.Stakeholders.Core.Domain.TouristPlanner;

namespace Explorer.Stakeholders.Core.UseCases.TouristPlanner;

public class PlannerService : IPlannerService
{
    private readonly IPlannerRepository _plannerRepository;
    private readonly IMapper _mapper;

    public PlannerService(IPlannerRepository plannerRepository, IMapper mapper)
    {
        _plannerRepository = plannerRepository;
        _mapper = mapper;
    }

    public PlannerDto GetOrCreatePlanner(long touristId)
    {
        return _mapper.Map<PlannerDto>(GetOrCreatePlannerEntity(touristId));
    }

    public PlannerDayDto GetDay(long touristId, DateOnly date)
    {
        var planner = GetOrCreatePlannerEntity(touristId);
        return _mapper.Map<PlannerDayDto>(planner.Days.FirstOrDefault(d => d.Date == date)) ?? throw new NotFoundException("Day not found.");
    }

    public PlannerDayDto AddBlock(long touristId, DateOnly date, CreatePlannerTimeBlockDto dto)
    {
        var planner = GetOrCreatePlannerEntity(touristId);
        var day = planner.Days.FirstOrDefault(d => d.Date == date);
        if (day == null) {
            day = new PlannerDay(date);
            planner.AddDay(day);
        }

        var block = new PlannerTimeBlock(dto.TourId, dto.StartTime, dto.EndTime);

        day.AddBlock(block, dto.Duration);
        _plannerRepository.Update(planner);

        return _mapper.Map<PlannerDayDto>(day);
    }

    public PlannerDayDto RemoveBlock(long touristId, DateOnly date, long blockId)
    {
        var planner = GetOrCreatePlannerEntity(touristId);
        var day = planner.Days.FirstOrDefault(d => d.Date == date) ?? throw new NotFoundException("Day not found.");

        day.RemoveBlock(blockId);
        _plannerRepository.Update(planner);

        return _mapper.Map<PlannerDayDto>(day);
    }

    public PlannerDayDto RescheduleBlock(long touristId, DateOnly date, long blockId, CreatePlannerTimeBlockDto dto)
    {
        var planner = GetOrCreatePlannerEntity(touristId);
        var day = planner.Days.FirstOrDefault(d => d.Date == date);

        if(day == null) return AddBlock(touristId, date, dto);

        day.RescheduleBlock(blockId, dto.StartTime, dto.EndTime);
        _plannerRepository.Update(planner);

        return _mapper.Map<PlannerDayDto>(day);
    }

    private Planner GetOrCreatePlannerEntity(long touristId)
    {
        return _plannerRepository.GetByTouristId(touristId) ?? _plannerRepository.Create(touristId);
    }
}
