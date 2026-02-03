using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.API.Internal
{
    public interface IUserRoleService
    {
        bool IsTourist(long userId);
        string GetUserRole(long userId);
    }
}
