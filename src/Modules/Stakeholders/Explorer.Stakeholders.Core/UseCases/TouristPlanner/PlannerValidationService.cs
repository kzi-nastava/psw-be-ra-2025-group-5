using Explorer.Stakeholders.API.Dtos.TouristPlanner;
using Explorer.Stakeholders.API.Public.TouristPlanner;

namespace Explorer.Stakeholders.Core.UseCases.TouristPlanner
{
    public class PlannerValidationService : IPlannerValidationService
    {
        private const int MAX_TOURS_PER_DAY = 2;
        private static readonly TimeOnly NIGHT_START = new(22, 0);
        private const int MIN_BREAK_MINUTES = 30;
        public List<PlannerWarningDto> ValidateDay(PlannerDayDto dayDto, Dictionary<long, int> systemDurations)
        {
            var warnings = new List<PlannerWarningDto>();

            warnings.AddRange(CheckTooManyTours(dayDto));
            warnings.AddRange(CheckNoBreaks(dayDto));
            warnings.AddRange(CheckLateNight(dayDto));
            warnings.AddRange(CheckDurationMismatch(dayDto, systemDurations));

            return warnings;
        }

        private IEnumerable<PlannerWarningDto> CheckTooManyTours(PlannerDayDto dayDto)
        {
            if (dayDto.TimeBlocks.Count > MAX_TOURS_PER_DAY)
            {
                yield return new PlannerWarningDto
                {
                    Type = "TooManyTours",
                    Message = $"You have too many tours ({dayDto.TimeBlocks.Count}) planned for this day",
                    AffectedBlockIds = dayDto.TimeBlocks.Select(b => b.Id).ToList()
                };
            }
        }

        private IEnumerable<PlannerWarningDto> CheckNoBreaks(PlannerDayDto dayDto)
        {
            var ordered = dayDto.TimeBlocks.OrderBy(b => b.StartTime).ToList();

            for (int i = 1; i < ordered.Count; i++)
            {
                var gap = ordered[i].StartTime - ordered[i - 1].EndTime;
                if (gap.TotalMinutes < MIN_BREAK_MINUTES)
                {
                    yield return new PlannerWarningDto
                    {
                        Type = "NoBreaks",
                        Message = "There are not enough breaks between some tours",
                        AffectedBlockIds = new List<long> { ordered[i - 1].Id, ordered[i].Id }
                    };
                }
            }
        }

        private IEnumerable<PlannerWarningDto> CheckLateNight(PlannerDayDto dayDto)
        {
            var lateBlocks = dayDto.TimeBlocks
                .Where(b => b.StartTime >= NIGHT_START)
                .ToList();

            if (lateBlocks.Any())
            {
                yield return new PlannerWarningDto
                {
                    Type = "LateNightActivity",
                    Message = "You have planned tours late at night (after 22:00)",
                    AffectedBlockIds = lateBlocks.Select(b => b.Id).ToList()
                };
            }
        }

        private IEnumerable<PlannerWarningDto> CheckDurationMismatch(PlannerDayDto dayDto, Dictionary<long, int> systemDurations)
        {
            foreach (var block in dayDto.TimeBlocks)
            {
                if (!systemDurations.TryGetValue(block.TourId, out var expected))
                    continue;

                var actualMinutes = (block.EndTime - block.StartTime).TotalMinutes;

                if (actualMinutes < expected * 0.6)
                {
                    yield return new PlannerWarningDto
                    {
                        Type = "DurationTooShort",
                        Message = "Planned duration is much shorter than recommended",
                        AffectedBlockIds = new List<long> { block.Id }
                    };
                }
            }
        }
    }
}
