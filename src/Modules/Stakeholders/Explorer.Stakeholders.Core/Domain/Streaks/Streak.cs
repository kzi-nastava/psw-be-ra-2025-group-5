using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Core.Domain.Streaks
{
    public class Streak : Entity
    {
        public long UserId { get; private set; }
        public DateOnly StartDate { get; private set; }
        public DateOnly LastActivity { get; private set; }
        public int LongestStreak { get; private set; }

        public Streak() { }
        public Streak(long userId)
        {
            UserId = userId;
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            StartDate = today;
            LastActivity = today;
            LongestStreak = 1;
        }

        public void RecordActivity()
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            if (LastActivity == default)
            {
                StartDate = today;
                LastActivity = today;
                LongestStreak = 1;
                return;
            }

            if (LastActivity == today)
                return;

            if (LastActivity.AddDays(1) == today)
            {
                int currentStreak = GetCurrentStreak() + 1;

                if (currentStreak > LongestStreak)
                    LongestStreak = currentStreak;
            }
            else
            {
                StartDate = today;
                LongestStreak = Math.Max(LongestStreak, 1);
            }

            LastActivity = today;
        }


        public int GetCurrentStreak()
        {
            return (LastActivity.DayNumber - StartDate.DayNumber) + 1;
        }


    }
}
