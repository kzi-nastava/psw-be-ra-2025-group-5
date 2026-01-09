using Explorer.Encounters.API.Dtos;
using Explorer.Encounters.API.Internal;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Encounters.API.Public.Tourist
{
    public interface IChallengeCreationService
    {
        ChallengeResponseDto CreateByTourist(ChallengeResponseDto dto, long creatorId, IFormFile? image);
        ChallengeResponseDto Update(UpdateTouristChallengeDto entity, long userId);
        List<ChallengeResponseDto> GetByTourist(long touristId);

    }
}
