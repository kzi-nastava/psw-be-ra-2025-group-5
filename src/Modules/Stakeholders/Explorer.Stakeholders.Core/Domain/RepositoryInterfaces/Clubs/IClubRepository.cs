using Explorer.Stakeholders.Core.Domain.Clubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Core.Domain.RepositoryInterfaces.Clubs
{
    public interface IClubRepository
    {
        Club Create(Club club);
        Club Update(Club club);
        void Delete(long id);
        Club? GetById(long id);
        List<Club> GetAll();
    }
}
