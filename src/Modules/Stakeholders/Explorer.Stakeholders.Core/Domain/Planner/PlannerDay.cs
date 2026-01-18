using Explorer.BuildingBlocks.Core.Domain;

namespace Explorer.Stakeholders.Core.Domain.Planner;

public class PlannerDay : Entity
{
    public DateOnly Date { get; private set; }

    private readonly List<PlannerTimeBlock> _timeBlocks = [];
    public IReadOnlyCollection<PlannerTimeBlock> TimeBlocks => _timeBlocks;

    private PlannerDay() { }

    public PlannerDay(DateOnly date)
    {
        Date = date;
    }

    public void AddBlock(PlannerTimeBlock block)
    {
        EnsureNoOverlap(block.TimeRange);
        _timeBlocks.Add(block);
    }

    public void RemoveBlock(long blockId)
    {
        var block = _timeBlocks.FirstOrDefault(b => b.Id == blockId) ?? throw new InvalidDataException("Planner block not found.");
        _timeBlocks.Remove(block);
    }

    public void RescheduleBlock(long blockId, TimeOnly start, TimeOnly end)
    {
        var block = _timeBlocks.First(b => b.Id == blockId) ?? throw new InvalidDataException("Planner block not found.");
        var candidate = new TimeRange(start, end);

        EnsureNoOverlap(candidate, blockId);
        block.Reschedule(candidate);
    }

    private void EnsureNoOverlap(TimeRange candidate, long? excludingBlockId = null)
    {
        foreach (var block in _timeBlocks)
        {
            if (excludingBlockId.HasValue && block.Id == excludingBlockId.Value)
                continue;

            if (block.TimeRange.OverlapsWith(candidate))
                throw new InvalidDataException("Time block overlaps with an existing block.");
        }
    }
}
