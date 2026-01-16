using Explorer.BuildingBlocks.Core.Exceptions;
using Explorer.Payments.API.Internal;
using Explorer.Stakeholders.API.Dtos.Authentication;
using Explorer.Stakeholders.API.Public.Users;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces.Users;
using Explorer.Stakeholders.Core.Domain.Users;

namespace Explorer.Stakeholders.Core.UseCases.Administration.Users;

public class AuthenticationService : IAuthenticationService
{
    private readonly ITokenGenerator _tokenGenerator;
    private readonly IUserRepository _userRepository;
    private readonly IPersonRepository _personRepository;
    private readonly IInternalWalletService _walletService;

    public AuthenticationService(IUserRepository userRepository, IPersonRepository personRepository, ITokenGenerator tokenGenerator, IInternalWalletService walletService)
    {
        _tokenGenerator = tokenGenerator;
        _userRepository = userRepository;
        _personRepository = personRepository;
        _walletService = walletService;
    }

    public AuthenticationTokensDto Login(CredentialsDto credentials)
    {
        var user = _userRepository.GetActiveByName(credentials.Username);
        if (user == null || credentials.Password != user.Password)
        {
            throw new UnauthorizedAccessException("Invalid credentials");
        }

        long personId;
        try
        {
            personId = _userRepository.GetPersonId(user.Id);
        }
        catch (KeyNotFoundException)
        {
            personId = 0;
        }
        return _tokenGenerator.GenerateAccessToken(user, personId);
    }

    public AuthenticationTokensDto RegisterTourist(AccountRegistrationDto account)
    {
        if(_userRepository.Exists(account.Username))
            throw new EntityValidationException("Provided username already exists.");

        var user = _userRepository.Create(new User(account.Username, account.Password, account.Email, UserRole.Tourist, true));
        var person = _personRepository.Create(new Person(user.Id, account.Name, account.Surname, account.Email));

        if(user.Role == UserRole.Tourist)
            _walletService.CreateWalletForPerson(user.Id);

        return _tokenGenerator.GenerateAccessToken(user, person.Id);
    }
}