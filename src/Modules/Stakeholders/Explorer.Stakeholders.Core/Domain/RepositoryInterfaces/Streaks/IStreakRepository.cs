using Explorer.Stakeholders.Core.Domain.Streaks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Core.Domain.RepositoryInterfaces.Streaks
{
    public interface IStreakRepository
    {
        Streak? GetByUserId(long userId);
        void Add(Streak streak);
        void Update(Streak streak);
        void Save();
        bool Exists(long userId);

    }
}
