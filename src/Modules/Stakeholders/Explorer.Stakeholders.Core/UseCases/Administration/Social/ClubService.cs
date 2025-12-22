using AutoMapper;
using Explorer.BuildingBlocks.Core.Domain;
using Explorer.BuildingBlocks.Core.Exceptions;
using Explorer.Stakeholders.Core.Domain.Clubs;
using System;
using System.Collections.Generic;
using System.Linq;
using Explorer.Stakeholders.API.Dtos.Clubs;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces.Clubs;
using Explorer.Stakeholders.API.Public.Clubs;

namespace Explorer.Stakeholders.Core.UseCases.Administration.Social
{
    public class ClubService : IClubService
    {
        private readonly IClubRepository _clubRepository;
        private readonly IMapper _mapper;

        public ClubService(IClubRepository clubRepository, IMapper mapper)
        {
            _clubRepository = clubRepository;
            _mapper = mapper;
        }

        public ClubDto Create(ClubDto clubDto)
        {
            try
            {
                // Konvertuj Base64 stringove u byte[]
                var imageBytes = clubDto.Images
                    .Select(base64 => Convert.FromBase64String(base64))
                    .ToList();

                var club = new Club(
                    clubDto.Name,
                    clubDto.Description,
                    imageBytes,
                    clubDto.CreatorId
                );

                var createdClub = _clubRepository.Create(club);
                return _mapper.Map<ClubDto>(createdClub);
            }
            catch (ArgumentException ex)
            {
                throw new EntityValidationException(ex.Message);
            }
            catch (FormatException ex)
            {
                throw new EntityValidationException("Invalid image format: " + ex.Message);
            }
        }

        public ClubDto Update(ClubDto clubDto)
        {
            var existingClub = _clubRepository.GetById(clubDto.Id);
            if (existingClub == null)
            {
                throw new NotFoundException($"Club with ID {clubDto.Id} not found.");
            }

            if (existingClub.CreatorId != clubDto.CreatorId)
            {
                throw new UnauthorizedAccessException("You can only update your own clubs.");
            }

            try
            {
                if (clubDto.Images == null || clubDto.Images.Count == 0)
                {
                    throw new ArgumentException("Club must have at least one image.");
                }

                List<byte[]> imageBytes = clubDto.Images
                    .Select(base64 => Convert.FromBase64String(base64))
                    .ToList();

                var updatedClub = new Club(
                    clubDto.Name,
                    clubDto.Description,
                    imageBytes,
                    clubDto.CreatorId
                );

                // Postavljanje ID-ja za update
                typeof(Entity).GetProperty("Id")?.SetValue(updatedClub, clubDto.Id);

                var result = _clubRepository.Update(updatedClub);
                return _mapper.Map<ClubDto>(result);
            }
            catch (ArgumentException ex)
            {
                throw new EntityValidationException(ex.Message);
            }
            catch (FormatException ex)
            {
                throw new EntityValidationException("Invalid image format: " + ex.Message);
            }
        }

        public void Delete(long userId, long id)
        {
            var club = _clubRepository.GetById(id);
            if (club == null)
            {
                throw new NotFoundException($"Club with ID {id} not found.");
            }

            if (club.CreatorId != userId)
                throw new UnauthorizedAccessException("You can only delete your own clubs.");

            _clubRepository.Delete(id);
        }

        public ClubDto GetById(long id)
        {
            var club = _clubRepository.GetById(id);
            if (club == null)
            {
                throw new KeyNotFoundException($"Club with ID {id} not found.");
            }
            return _mapper.Map<ClubDto>(club);
        }

        public List<ClubDto> GetAll()
        {
            var clubs = _clubRepository.GetAll();
            return _mapper.Map<List<ClubDto>>(clubs);
        }
    }
}