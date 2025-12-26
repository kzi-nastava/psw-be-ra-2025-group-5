using Explorer.Stakeholders.API.Dtos.Users;
using Microsoft.AspNetCore.Http;

namespace Explorer.Stakeholders.API.Public.Users;

public interface IProfileService
{
    ProfileDto GetByUserId(long userId);
    ProfileDto Update(ProfileDto profile, IFormFile? profileImage);
}
