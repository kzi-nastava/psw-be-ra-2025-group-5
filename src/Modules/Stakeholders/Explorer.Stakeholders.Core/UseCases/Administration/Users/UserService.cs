using AutoMapper;
using Explorer.Stakeholders.Core.Domain.Users;
using Explorer.Stakeholders.API.Dtos.Users;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces.Users;
using Explorer.Stakeholders.API.Internal;


namespace Explorer.Stakeholders.Core.UseCases.Administration.Users
{
    public class UserService : IUserService, IUserInfoService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPersonRepository _personRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IPersonRepository personRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _personRepository = personRepository;
            _mapper = mapper;
        }

        public UserDto Create(CreateUserDto dto)
        {
            if (_userRepository.Exists(dto.Username))
                throw new Exception("Username already exists.");

            if (!Enum.TryParse(dto.Role, true, out UserRole role) || role == UserRole.Tourist)
            {
                throw new ArgumentException("Administrator can only create 'Administrator' or 'Author' accounts.");
            }

            User user = _mapper.Map<User>(dto);
            user.Activate();

            User created = _userRepository.Create(user);

            var person = new Person(
                created.Id,
                null,
                null,
                created.Email
            );

            _personRepository.Create(person);

            return _mapper.Map<UserDto>(created);
        }

        public List<UserDto> GetAll()
        {
            List<User> users = _userRepository.GetAll();
            return _mapper.Map<List<UserDto>>(users);
        }

        public UserDto Block(long id)
        {
            User user = _userRepository.Get(id);
            if (user == null)
                throw new Exception("User not found.");

            if (user.Role == UserRole.Administrator)
            {
                throw new InvalidOperationException("Cannot block an Administrator account.");
            }

            if (!user.IsActive)
            {
                throw new InvalidOperationException("User is already blocked.");
            }

            user.Deactivate();

            User updated = _userRepository.Update(user);
            return _mapper.Map<UserDto>(updated);
        }

        public UserDto Unblock(long id)
        {
            User user = _userRepository.Get(id);
            if (user == null)
                throw new Exception("User not found.");
            if (user.Role == UserRole.Administrator)
            {
                throw new InvalidOperationException("Cannot unblock an Administrator account.");
            }

            if (user.IsActive)
            {
                throw new InvalidOperationException("User is already active.");
            }

            user.Activate();

            User updated = _userRepository.Update(user);
            return _mapper.Map<UserDto>(updated);
        }

        public UserDto GetById(long id)
        {
            User user = _userRepository.Get(id);
            if (user == null)
                throw new Exception("User not found.");
            return _mapper.Map<UserDto>(user);
        }
        public List<ProfileDto> GetTourists()
        {
            var users = _userRepository.GetAll()
                .Where(u => u.Role == UserRole.Tourist)
                .ToList();

            var result = new List<ProfileDto>();

            foreach (var user in users)
            {
                if (user.IsActive)
                {
                    var person = _personRepository.GetByUserId(user.Id);

                    result.Add(new ProfileDto
                    {
                        Id = user.Id,
                        Username = user.Username,
                        ProfileImagePath = person.ProfileImagePath,
                        Name = person.Name,
                        Surname = person.Surname
                    });
                }
            }

            return result;
        }

        public List<ProfileDto> GetAuthors()
        {
            var users = _userRepository.GetAll()
                .Where(u => u.Role == UserRole.Author)
                .ToList();

            var result = new List<ProfileDto>();

            foreach (var user in users)
            {
                if (user.IsActive)
                {
                    var person = _personRepository.GetByUserId(user.Id);

                    if (person != null)
                    {
                        result.Add(new ProfileDto
                        {
                            Id = user.Id,
                            Username = user.Username,
                            ProfileImagePath = person.ProfileImagePath,
                            Name = person.Name,
                            Surname = person.Surname
                        });
                    }
                }
            }

            return result;
        }

    }
}
