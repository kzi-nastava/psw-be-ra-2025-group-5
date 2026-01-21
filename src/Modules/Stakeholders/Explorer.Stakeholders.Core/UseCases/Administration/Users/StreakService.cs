using Explorer.Stakeholders.API.Dtos.Streaks;
using Explorer.Stakeholders.API.Public.Streaks;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces.Streaks;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces.Users;
using Explorer.Stakeholders.Core.Domain.Streaks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Core.UseCases.Administration.Users
{
    public class StreakService : IStreakService
    {
        private readonly IStreakRepository _streakRepository;
        private readonly IPersonRepository _personRepository;

        public StreakService(IStreakRepository streakRepository, IPersonRepository personRepository)
        {
            _streakRepository = streakRepository;
            _personRepository = personRepository;
        }

        public void RecordActivity(long userId)
        {
            var streak = _streakRepository.GetByUserId(userId);
            var person = _personRepository.GetByUserId(userId);

            if (person == null) return; // Ne možemo dodati XP ako nema osobe

            if (streak == null)
            {
                streak = new Streak(userId);
                _streakRepository.Add(streak);

                person.AddExperience(10); // Inicijalni XP
                _personRepository.Update(person);
            }
            else
            {
                var today = DateOnly.FromDateTime(DateTime.UtcNow);
                bool isContinuous = streak.LastActivity.AddDays(1) == today;
                bool isAlreadyRecordedToday = streak.LastActivity == today;

                if (!isAlreadyRecordedToday)
                {
                    streak.RecordActivity();
                    _streakRepository.Update(streak);

                    if (isContinuous)
                    {
                        // Bonus XP: npr. 7 po danu niza
                        int bonusXp = 7 * streak.GetCurrentStreak();
                        person.AddExperience(bonusXp);
                        _personRepository.Update(person);
                    }
                }
            }

            // Osiguraj da su obe baze (ako su odvojene) snimljene
            _streakRepository.Save();
        }

        public StreakDto GetStreakForUser(long userId)
        {
            var streak = _streakRepository.GetByUserId(userId);

            if (streak == null)
            {
                return new StreakDto
                {
                    UserId = userId,
                    StartDate = DateOnly.FromDateTime(DateTime.UtcNow),
                    CurrentStreak = 0,
                    LongestStreak = 0
                };
            }

            return new StreakDto
            {
                UserId = streak.UserId,
                StartDate = streak.StartDate,
                CurrentStreak = streak.GetCurrentStreak(),
                LongestStreak = streak.LongestStreak,
                LastActivityDate = streak.LastActivity
            };
        }

    }
}
