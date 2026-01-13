using Explorer.Stakeholders.API.Dtos.Authentication;
using Explorer.Stakeholders.Core.Domain.Users;

namespace Explorer.Stakeholders.Core.UseCases;

public interface ITokenGenerator
{
    AuthenticationTokensDto GenerateAccessToken(User user, long personId);
}