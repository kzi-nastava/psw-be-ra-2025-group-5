using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Core.Domain.RepositoryInterfaces
{
    public interface IClubInviteRepository
    {
        ClubInvite Create(ClubInvite invite);
        void Delete(ClubInvite invite);
        ClubInvite? GetById(long id);
        List<ClubInvite> GetByClubId(long clubId);
        List<ClubInvite> GetByTouristId(long touristId);
        bool Exists(long clubId, long touristId);

    }
}
