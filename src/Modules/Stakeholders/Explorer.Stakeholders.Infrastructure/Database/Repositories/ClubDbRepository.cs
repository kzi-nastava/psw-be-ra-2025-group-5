using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Infrastructure.Database.Repositories
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
            var existingClub = _dbContext.Clubs.FirstOrDefault(c => c.Id == club.Id);
            if (existingClub == null)
                throw new KeyNotFoundException("Club not found");

            existingClub.Name = club.Name;
            existingClub.Description = club.Description;
            existingClub.CreatorId = club.CreatorId;

            _dbContext.SaveChanges();
            return existingClub;
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
            return _dbContext.Clubs.Find(id);
        }

        public List<Club> GetAll()
        {
            return _dbContext.Clubs.ToList();
        }
    }
}
