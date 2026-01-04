using Explorer.Encounters.API.Dtos;
using Explorer.Encounters.API.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Encounters.API.Public.Tourist
{
    public interface IChallengeCreationService
    {
        ChallengeDto CreateByTourist(CreateTouristChallengeDto dto, long creatorId);
        ChallengeDto Update(UpdateTouristChallengeDto entity, long userId);
    }
}
