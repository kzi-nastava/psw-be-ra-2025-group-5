using Explorer.BuildingBlocks.Core.Domain;

namespace Explorer.Stakeholders.Core.Domain.TouristPlanner;

public class PlannerDay : Entity
{
    public DateOnly Date { get; private set; }

    public List<PlannerTimeBlock> TimeBlocks = [];

    private PlannerDay() { }

    public PlannerDay(DateOnly date)
    {
        Date = date;
    }

    public void AddBlock(PlannerTimeBlock block, int duration = 0)
    {
        var nullTime = new TimeOnly(0, 0);
        if (block.TimeRange.End == nullTime)
        {
            var dayEnd = nullTime.AddMinutes(duration);
            block.TimeRange = FindFirstAvailableSlot(dayEnd - nullTime);
        }
        else
            EnsureNoOverlap(block.TimeRange);
        TimeBlocks.Add(block);
    }

    public void RemoveBlock(long blockId)
    {
        var block = TimeBlocks.FirstOrDefault(b => b.Id == blockId) ?? throw new InvalidDataException("Planner block not found");
        TimeBlocks.Remove(block);
    }

    public void RescheduleBlock(long blockId, TimeOnly start, TimeOnly end, TransportType transportType)
    {
        var block = TimeBlocks.First(b => b.Id == blockId) ?? throw new InvalidDataException("Planner block not found");
        TimeRange candidate;
        var nullTime = new TimeOnly(0, 0);

        if (end == nullTime)
            candidate = FindFirstAvailableSlot(block.TimeRange.End - block.TimeRange.Start);
        else
        {
            candidate = new TimeRange(start, end);
            EnsureNoOverlap(candidate, blockId);
        }
        block.Reschedule(candidate, transportType);
    }

    private TimeRange FindFirstAvailableSlot(TimeSpan duration)
    {
        var dayStart = new TimeOnly(0, 0);
        var dayEnd = TimeOnly.MaxValue;

        var ordered = TimeBlocks.OrderBy(b => b.TimeRange.Start).Select(b => b.TimeRange).ToList();
        var pointer = dayStart;

        foreach (var range in ordered)
        {
            pointer = SnapUpToQuarterHour(pointer);
            if (range.Start - pointer >= duration)
                return new TimeRange(pointer, pointer.Add(duration));
            pointer = range.End;
        }

        pointer = SnapUpToQuarterHour(pointer);
        if (dayEnd - pointer >= duration)
            return new TimeRange(pointer, pointer.Add(duration));

        throw new InvalidOperationException("No available time slot found");
    }

    private static TimeOnly SnapUpToQuarterHour(TimeOnly time)
    {
        var totalMinutes = time.Hour * 60 + time.Minute;
        var snappedMinutes = ((totalMinutes + 14) / 15) * 15;

        if (snappedMinutes >= 24 * 60)
            return new TimeOnly(23, 59);

        return TimeOnly.FromTimeSpan(TimeSpan.FromMinutes(snappedMinutes));
    }


    private void EnsureNoOverlap(TimeRange candidate, long? excludingBlockId = null)
    {
        foreach (var block in TimeBlocks)
        {
            if (excludingBlockId.HasValue && block.Id == excludingBlockId.Value)
                continue;

            if (block.TimeRange.OverlapsWith(candidate))
                throw new InvalidDataException("Time block overlaps with an existing block");
        }
    }
}
