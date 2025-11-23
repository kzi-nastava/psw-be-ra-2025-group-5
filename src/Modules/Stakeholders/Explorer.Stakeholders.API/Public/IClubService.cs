using Explorer.Stakeholders.API.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.API.Public
{
    public interface IClubService
    {
        ClubDto Create(ClubDto clubDto);
        ClubDto Update(ClubDto clubDto);
        void Delete(long userId, long id);
        ClubDto GetById(long id);
        List<ClubDto> GetAll();
    }
}
