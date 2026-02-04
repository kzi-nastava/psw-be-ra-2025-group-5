using Explorer.Stakeholders.API.Dtos.Streaks;

namespace Explorer.Stakeholders.API.Public.Streaks
{
    public interface IStreakService
    {
        void RecordActivity(long userId);
        StreakDto GetStreakForUser(long userId);
    }
}
