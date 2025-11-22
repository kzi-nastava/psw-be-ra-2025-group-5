using Explorer.Stakeholders.API.Dtos;

namespace Explorer.Stakeholders.API.Public;

public interface IProfileService
{
    ProfileDto GetByUserId(long userId);
    ProfileDto Update(ProfileDto profile);
}
