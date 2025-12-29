using Explorer.Stakeholders.API.Dtos.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.API.Internal
{
    public interface IUserInfoService
    {
        List<ProfileDto> GetTourists();
    }
}
