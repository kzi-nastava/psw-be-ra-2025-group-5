using Explorer.Stakeholders.Core.Domain.Clubs;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces.Clubs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Infrastructure.Database.Repositories.Clubs
{
    public class ClubDbRepository : IClubRepository
    {
        private readonly StakeholdersContext _dbContext;

        public ClubDbRepository(StakeholdersContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Club Create(Club club)
        {
            _dbContext.Clubs.Add(club);
            _dbContext.SaveChanges();
            return club;
        }

        public Club Update(Club club)
        {
            _dbContext.Clubs.Update(club);
            _dbContext.SaveChanges();
            return club;
        }


        public void Delete(long id)
        {
            var club = _dbContext.Clubs.Find(id);
            if (club != null)
            {
                _dbContext.Clubs.Remove(club);
                _dbContext.SaveChanges();
            }
        }

        public Club? GetById(long id)
        {
            return _dbContext.Clubs
                .Include(c => c.Members)
                .AsTracking()
                .FirstOrDefault(c => c.Id == id);
        }


        public List<Club> GetAll()
        {
            return _dbContext.Clubs.ToList();
        }
    }
}
