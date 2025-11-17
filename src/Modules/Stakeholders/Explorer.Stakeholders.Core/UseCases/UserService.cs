using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using AutoMapper;


namespace Explorer.Stakeholders.Core.UseCases
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
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

            user.Deactivate();

            User updated = _userRepository.Update(user);
            return _mapper.Map<UserDto>(updated);
        }
    }
}
