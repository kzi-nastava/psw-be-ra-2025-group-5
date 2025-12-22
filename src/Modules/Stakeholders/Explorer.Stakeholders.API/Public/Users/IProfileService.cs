using Explorer.Stakeholders.API.Dtos.Users;

namespace Explorer.Stakeholders.API.Public.Users;

public interface IProfileService
{
    ProfileDto GetByUserId(long userId);
    ProfileDto Update(ProfileDto profile);
}
