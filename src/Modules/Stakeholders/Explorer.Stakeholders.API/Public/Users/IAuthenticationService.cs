using Explorer.Stakeholders.API.Dtos.Authentication;

namespace Explorer.Stakeholders.API.Public.Users;

public interface IAuthenticationService
{
    AuthenticationTokensDto Login(CredentialsDto credentials);
    AuthenticationTokensDto RegisterTourist(AccountRegistrationDto account);
}