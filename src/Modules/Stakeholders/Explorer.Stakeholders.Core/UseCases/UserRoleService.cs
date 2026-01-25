using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.Stakeholders.API.Internal;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces.Users;
using Explorer.Stakeholders.Core.Domain.Users;
namespace Explorer.Stakeholders.Core.UseCases
{
    public class UserRoleService : IUserRoleService
    {
        private readonly IUserRepository _userRepository;

        public UserRoleService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public bool IsTourist(long userId)
        {
            var user = _userRepository.Get(userId);
            return user != null && user.Role == UserRole.Tourist;
        }

        public string GetUserRole(long userId)
        {
            var user = _userRepository.Get(userId);
            return user?.GetPrimaryRoleName() ?? "unknown";
        }
    }
}
