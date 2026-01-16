using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Core.Domain.RepositoryInterfaces
{
    public interface IClubJoinRequestRepository
    {
        ClubJoinRequest Create(ClubJoinRequest request);
        void Delete(ClubJoinRequest request);
        ClubJoinRequest? GetById(long id);
        ClubJoinRequest? GetByClubAndTourist(long clubId, long touristId);
        List<ClubJoinRequest> GetByClubId(long clubId);
        bool Exists(long clubId, long touristId);
      
    }

}
