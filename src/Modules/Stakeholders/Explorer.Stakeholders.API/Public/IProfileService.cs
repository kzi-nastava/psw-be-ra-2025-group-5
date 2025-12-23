using Explorer.Stakeholders.API.Dtos;
using Microsoft.AspNetCore.Http;

namespace Explorer.Stakeholders.API.Public;

public interface IProfileService
{
    ProfileDto GetByUserId(long userId);
    ProfileDto Update(ProfileDto profile, IFormFile? profileImage);
}
