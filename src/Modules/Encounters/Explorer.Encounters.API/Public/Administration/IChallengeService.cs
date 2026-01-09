using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Encounters.API.Dtos;
using Microsoft.AspNetCore.Http;

namespace Explorer.Encounters.API.Public.Administration;

public interface IChallengeService
{
    PagedResult<ChallengeResponseDto> GetPaged(int page, int pageSize);
    ChallengeResponseDto Create(ChallengeResponseDto challenge, IFormFile? image);
    ChallengeResponseDto Update(ChallengeResponseDto challenge, IFormFile? image);
    void Delete(long id);
    List<ChallengeResponseDto> GetAllActive();
    ChallengeResponseDto GetById(long challengeId);
    void Approve(long challengeId);
    void Reject(long challengeId);
}
