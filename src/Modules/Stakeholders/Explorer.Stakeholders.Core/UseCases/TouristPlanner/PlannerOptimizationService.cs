using AutoMapper;
using Explorer.Stakeholders.API.Dtos.TouristPlanner;
using Explorer.Stakeholders.API.Public.TouristPlanner;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces.TouristPlanner;
using Explorer.Stakeholders.Core.Domain.TouristPlanner;
using Explorer.Tours.API.Internal;

namespace Explorer.Stakeholders.Core.UseCases.TouristPlanner;

public class PlannerOptimizationService : IPlannerOptimizationService
{
    private readonly IPlannerRepository _plannerRepository;
    private readonly IPlannerValidationService _validationService;
    private readonly ITourSharedService _tourSharedService;
    private readonly IMapper _mapper;

    private const int MAX_TOURS_PER_DAY = 2;
    private const int MIN_BREAK_MINUTES = 30;
    private const int MAX_OPTIMIZATION_ITERATIONS = 5;
    private static readonly TimeOnly NIGHT_START = new(22, 0);
    private static readonly TimeOnly DAY_START = new(8, 0);

    public PlannerOptimizationService(
        IPlannerRepository plannerRepository,
        IPlannerValidationService validationService,
        ITourSharedService tourSharedService,
        IMapper mapper)
    {
        _plannerRepository = plannerRepository;
        _validationService = validationService;
        _tourSharedService = tourSharedService;
        _mapper = mapper;
    }

    public OptimizationResultDto OptimizePlanner(long touristId)
    {
        var planner = _plannerRepository.GetByTouristId(touristId);
        if (planner == null)
            return new OptimizationResultDto { Success = false, UnresolvedWarnings = ["Planner not found."] };

        var result = new OptimizationResultDto { Success = true };
        var daysToProcess = planner.Days.OrderBy(d => d.Date).ToList();

        foreach (var day in daysToProcess)
        {
            OptimizeSingleDay(planner, day, result);
        }

        if (result.ActionsPerformed.Count > 0)
            _plannerRepository.Update(planner);

        result.Planner = _mapper.Map<PlannerDto>(planner);
        result.Success = result.UnresolvedWarnings.Count == 0;

        return result;
    }

    private void OptimizeSingleDay(Planner planner, PlannerDay day, OptimizationResultDto result)
    {
        if (day.TimeBlocks.Count == 0) return;

        var tourIds = day.TimeBlocks.Select(b => b.TourId).Distinct();
        var defaultTransport = day.TimeBlocks.FirstOrDefault()?.TransportType ?? TransportType.Walking;
        var systemDurations = _tourSharedService.GetDurationsByTransport(tourIds, defaultTransport.ToString());

        for (int iteration = 0; iteration < MAX_OPTIMIZATION_ITERATIONS; iteration++)
        {
            var dayDto = _mapper.Map<PlannerDayDto>(day);
            var currentWarnings = _validationService.ValidateDay(dayDto, systemDurations);

            if (currentWarnings.Count == 0) break;

            int actionsBefore = result.ActionsPerformed.Count;

            // Priority 1: Fix TooManyTours
            if (currentWarnings.Any(w => w.Type == "TooManyTours"))
                FixTooManyTours(planner, day, result);

            // Priority 2: Fix LateNightActivity
            if (currentWarnings.Any(w => w.Type == "LateNightActivity"))
                FixLateNightActivity(day, result);

            // Priority 3: Fix NoBreaks
            if (currentWarnings.Any(w => w.Type == "NoBreaks"))
                FixNoBreaks(day, result);

            // Priority 4: Fix DurationTooShort
            if (currentWarnings.Any(w => w.Type == "DurationTooShort"))
                FixDurationTooShort(day, systemDurations, result);

            if (result.ActionsPerformed.Count == actionsBefore)
            {
                var remainingDayDto = _mapper.Map<PlannerDayDto>(day);
                var remainingWarnings = _validationService.ValidateDay(remainingDayDto, systemDurations);
                foreach (var warning in remainingWarnings)
                {
                    if (!result.UnresolvedWarnings.Contains(warning.Message))
                        result.UnresolvedWarnings.Add(warning.Message);
                }
                break;
            }
        }
    }

    private void FixTooManyTours(Planner planner, PlannerDay day, OptimizationResultDto result)
    {
        while (day.TimeBlocks.Count > MAX_TOURS_PER_DAY)
        {
            var blockToMove = day.TimeBlocks.OrderBy(b => b.TimeRange.Start).Last();
            var targetDay = FindDayWithCapacity(planner, day.Date, blockToMove);

            if (targetDay != null)
            {
                MoveBlockToDay(day, targetDay, blockToMove, result);
            }
            else
            {
                result.UnresolvedWarnings.Add($"Cannot move excess tour (ID: {blockToMove.TourId}) - no adjacent day available.");
                break;
            }
        }
    }

    private PlannerDay? FindDayWithCapacity(Planner planner, DateOnly sourceDate, PlannerTimeBlock block)
    {
        var duration = block.TimeRange.End - block.TimeRange.Start;

        // Try next day first
        var nextDate = sourceDate.AddDays(1);
        var nextDay = planner.Days.FirstOrDefault(d => d.Date == nextDate);
        if (nextDay == null)
        {
            nextDay = new PlannerDay(nextDate);
            planner.AddDay(nextDay);
            return nextDay;
        }
        if (nextDay.TimeBlocks.Count < MAX_TOURS_PER_DAY && HasAvailableSlot(nextDay, duration))
            return nextDay;

        // Try previous day
        var prevDate = sourceDate.AddDays(-1);
        var prevDay = planner.Days.FirstOrDefault(d => d.Date == prevDate);
        if (prevDay == null)
        {
            prevDay = new PlannerDay(prevDate);
            planner.AddDay(prevDay);
            return prevDay;
        }
        if (prevDay.TimeBlocks.Count < MAX_TOURS_PER_DAY && HasAvailableSlot(prevDay, duration))
            return prevDay;

        return null;
    }

    private static bool HasAvailableSlot(PlannerDay day, TimeSpan duration)
    {
        var ordered = day.TimeBlocks.OrderBy(b => b.TimeRange.Start).ToList();
        var pointer = DAY_START;

        foreach (var block in ordered)
        {
            pointer = SnapUpToQuarterHour(pointer);
            if (block.TimeRange.Start - pointer >= duration)
                return true;
            pointer = block.TimeRange.End;
        }

        pointer = SnapUpToQuarterHour(pointer);
        var remaining = NIGHT_START - pointer;
        return remaining >= duration;
    }

    private void MoveBlockToDay(PlannerDay sourceDay, PlannerDay targetDay, PlannerTimeBlock block, OptimizationResultDto result)
    {
        var duration = block.TimeRange.End - block.TimeRange.Start;
        sourceDay.TimeBlocks.Remove(block);

        var newSlot = FindFirstAvailableSlot(targetDay, duration);
        block.TimeRange = newSlot;
        targetDay.TimeBlocks.Add(block);

        result.ActionsPerformed.Add(new OptimizationActionDto
        {
            ActionType = "MovedToDay",
            BlockId = block.Id,
            Description = $"Moved tour to {targetDay.Date:yyyy-MM-dd}"
        });
    }

    private void FixLateNightActivity(PlannerDay day, OptimizationResultDto result)
    {
        var lateBlocks = day.TimeBlocks
            .Where(b => b.TimeRange.Start >= NIGHT_START)
            .OrderBy(b => b.TimeRange.Start)
            .ToList();

        foreach (var block in lateBlocks)
        {
            var duration = block.TimeRange.End - block.TimeRange.Start;
            var earlierSlot = FindSlotBefore(day, duration, NIGHT_START, block.Id);

            if (earlierSlot != null)
            {
                var oldStart = block.TimeRange.Start;
                block.Reschedule(earlierSlot, block.TransportType);
                result.ActionsPerformed.Add(new OptimizationActionDto
                {
                    ActionType = "Shifted",
                    BlockId = block.Id,
                    Description = $"Moved from {oldStart:HH:mm} to {earlierSlot.Start:HH:mm}"
                });
            }
            else
            {
                result.UnresolvedWarnings.Add($"Cannot move late tour (starting at {block.TimeRange.Start:HH:mm}) to earlier time.");
            }
        }
    }

    private TimeRange? FindSlotBefore(PlannerDay day, TimeSpan duration, TimeOnly beforeTime, long excludeBlockId)
    {
        var orderedBlocks = day.TimeBlocks
            .Where(b => b.Id != excludeBlockId)
            .OrderBy(b => b.TimeRange.Start)
            .ToList();

        var pointer = DAY_START;

        foreach (var block in orderedBlocks)
        {
            if (block.TimeRange.Start >= beforeTime) break;

            pointer = SnapUpToQuarterHour(pointer);
            var availableEnd = pointer.Add(duration);

            if (availableEnd <= block.TimeRange.Start && availableEnd <= beforeTime)
                return new TimeRange(pointer, availableEnd);

            pointer = block.TimeRange.End;
        }

        pointer = SnapUpToQuarterHour(pointer);
        var finalEnd = pointer.Add(duration);
        if (finalEnd <= beforeTime)
            return new TimeRange(pointer, finalEnd);

        return null;
    }

    private void FixNoBreaks(PlannerDay day, OptimizationResultDto result)
    {
        var ordered = day.TimeBlocks.OrderBy(b => b.TimeRange.Start).ToList();

        for (int i = 1; i < ordered.Count; i++)
        {
            var prevBlock = ordered[i - 1];
            var currBlock = ordered[i];
            var gap = currBlock.TimeRange.Start - prevBlock.TimeRange.End;

            if (gap.TotalMinutes < MIN_BREAK_MINUTES)
            {
                var requiredShift = TimeSpan.FromMinutes(MIN_BREAK_MINUTES) - gap;
                var newStart = SnapUpToQuarterHour(currBlock.TimeRange.Start.Add(requiredShift));
                var blockDuration = currBlock.TimeRange.End - currBlock.TimeRange.Start;
                var newEnd = newStart.Add(blockDuration);

                // Check if we can shift this block and all subsequent blocks
                if (CanShiftBlock(day, currBlock, newStart, newEnd, ordered, i))
                {
                    ShiftBlockAndSubsequent(ordered, i, requiredShift, result);
                }
                else
                {
                    result.UnresolvedWarnings.Add($"Cannot create sufficient break before tour at {currBlock.TimeRange.Start:HH:mm}.");
                }
            }
        }
    }

    private bool CanShiftBlock(PlannerDay day, PlannerTimeBlock block, TimeOnly newStart, TimeOnly newEnd, List<PlannerTimeBlock> ordered, int startIndex)
    {
        // Check if the last block would exceed night time after shift
        var lastBlock = ordered.Last();
        var totalShift = newStart - block.TimeRange.Start;
        var lastNewEnd = lastBlock.TimeRange.End.Add(totalShift);

        return lastNewEnd <= NIGHT_START;
    }

    private void ShiftBlockAndSubsequent(List<PlannerTimeBlock> ordered, int startIndex, TimeSpan shift, OptimizationResultDto result)
    {
        for (int i = startIndex; i < ordered.Count; i++)
        {
            var block = ordered[i];
            var newStart = SnapUpToQuarterHour(block.TimeRange.Start.Add(shift));
            var duration = block.TimeRange.End - block.TimeRange.Start;
            var newEnd = newStart.Add(duration);

            var oldStart = block.TimeRange.Start;
            block.TimeRange = new TimeRange(newStart, newEnd);

            result.ActionsPerformed.Add(new OptimizationActionDto
            {
                ActionType = "Shifted",
                BlockId = block.Id,
                Description = $"Shifted from {oldStart:HH:mm} to {newStart:HH:mm}"
            });
        }
    }

    private void FixDurationTooShort(PlannerDay day, Dictionary<long, int> systemDurations, OptimizationResultDto result)
    {
        var ordered = day.TimeBlocks.OrderBy(b => b.TimeRange.Start).ToList();

        for (int i = 0; i < ordered.Count; i++)
        {
            var block = ordered[i];
            if (!systemDurations.TryGetValue(block.TourId, out var recommendedMinutes))
                continue;

            var actualMinutes = (block.TimeRange.End - block.TimeRange.Start).TotalMinutes;
            if (actualMinutes >= recommendedMinutes * 0.6)
                continue;

            var targetDuration = TimeSpan.FromMinutes(recommendedMinutes);
            var newEnd = block.TimeRange.Start.Add(targetDuration);

            // Check if we can extend without overlapping next block
            var nextBlock = i < ordered.Count - 1 ? ordered[i + 1] : null;
            var maxEnd = nextBlock != null
                ? nextBlock.TimeRange.Start.Add(TimeSpan.FromMinutes(-MIN_BREAK_MINUTES))
                : NIGHT_START;

            if (newEnd <= maxEnd)
            {
                var oldEnd = block.TimeRange.End;
                block.TimeRange = new TimeRange(block.TimeRange.Start, newEnd);
                result.ActionsPerformed.Add(new OptimizationActionDto
                {
                    ActionType = "Extended",
                    BlockId = block.Id,
                    Description = $"Extended duration from {actualMinutes:F0}min to {recommendedMinutes}min"
                });
            }
            else
            {
                // Try starting earlier instead
                var newStart = block.TimeRange.End.Add(-targetDuration);
                var prevBlock = i > 0 ? ordered[i - 1] : null;
                var minStart = prevBlock != null
                    ? prevBlock.TimeRange.End.Add(TimeSpan.FromMinutes(MIN_BREAK_MINUTES))
                    : DAY_START;

                newStart = SnapUpToQuarterHour(newStart);
                if (newStart >= minStart)
                {
                    var oldStart = block.TimeRange.Start;
                    block.TimeRange = new TimeRange(newStart, block.TimeRange.End);
                    result.ActionsPerformed.Add(new OptimizationActionDto
                    {
                        ActionType = "Extended",
                        BlockId = block.Id,
                        Description = $"Extended by starting earlier: {oldStart:HH:mm} -> {newStart:HH:mm}"
                    });
                }
                else
                {
                    result.UnresolvedWarnings.Add($"Cannot extend tour duration to recommended {recommendedMinutes}min.");
                }
            }
        }
    }

    private static TimeRange FindFirstAvailableSlot(PlannerDay day, TimeSpan duration)
    {
        var ordered = day.TimeBlocks.OrderBy(b => b.TimeRange.Start).ToList();
        var pointer = DAY_START;

        foreach (var block in ordered)
        {
            pointer = SnapUpToQuarterHour(pointer);
            if (block.TimeRange.Start - pointer >= duration)
                return new TimeRange(pointer, pointer.Add(duration));
            pointer = block.TimeRange.End;
        }

        pointer = SnapUpToQuarterHour(pointer);
        return new TimeRange(pointer, pointer.Add(duration));
    }

    private static TimeOnly SnapUpToQuarterHour(TimeOnly time)
    {
        var totalMinutes = time.Hour * 60 + time.Minute;
        var snappedMinutes = ((totalMinutes + 14) / 15) * 15;

        if (snappedMinutes >= 24 * 60)
            return new TimeOnly(23, 45);

        return TimeOnly.FromTimeSpan(TimeSpan.FromMinutes(snappedMinutes));
    }
}
