using Explorer.Stakeholders.API.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Core.UseCases
{
    public interface IUserService
    {
        UserDto Create(CreateUserDto userDto);
        List<UserDto> GetAll();
        UserDto Block(long id);
        UserDto Unblock(long id);
        UserDto GetById(long id);
    }
}
