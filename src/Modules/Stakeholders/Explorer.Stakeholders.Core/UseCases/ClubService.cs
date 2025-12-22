using AutoMapper;
using Explorer.BuildingBlocks.Core.Domain;
using Explorer.BuildingBlocks.Core.Exceptions;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using Explorer.Stakeholders.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using Explorer.Stakeholders.API.Dtos;
using Microsoft.AspNetCore.Http;
using Explorer.BuildingBlocks.Core.FileStorage;

namespace Explorer.Stakeholders.Core.UseCases
{
    public class ClubService : IClubService
    {
        private readonly IClubRepository _clubRepository;
        private readonly IMapper _mapper;
        private readonly IImageStorage _imageStorage;

        public ClubService(IClubRepository clubRepository, IMapper mapper, IImageStorage imageStorage)
        {
            _clubRepository = clubRepository;
            _mapper = mapper;
            _imageStorage = imageStorage;
        }
        public ClubDto Create(ClubDto clubDto, List<IFormFile> images)
        {
            if (images == null || !images.Any())
                throw new ArgumentException("Club must have at least one image.");

            var savedPaths = SaveImages(clubDto.CreatorId, images);

            var club = new Club(
                clubDto.Name,
                clubDto.Description,
                savedPaths,
                clubDto.CreatorId
            );

            var createdClub = _clubRepository.Create(club);
            return _mapper.Map<ClubDto>(createdClub);
        }

        public ClubDto Update(ClubDto clubDto, List<IFormFile>? images)
        {
            var existingClub = _clubRepository.GetById(clubDto.Id);

            if (existingClub == null)
                throw new NotFoundException($"Club with ID {clubDto.Id} not found.");

            existingClub.Name = clubDto.Name;
            existingClub.Description = clubDto.Description;

            if (images != null && images.Any())
            {
                var savedPaths = SaveImages(clubDto.CreatorId, images);
                existingClub.ImagePaths.AddRange(savedPaths);
            }

            existingClub.Validate();
            var updatedClub = _clubRepository.Update(existingClub);

            return _mapper.Map<ClubDto>(updatedClub);
        }

        private List<string> SaveImages(long clubId, List<IFormFile> images)
        {
            var paths = new List<string>();
            foreach (var file in images)
            {
                using var ms = new MemoryStream();
                file.CopyTo(ms);
                var bytes = ms.ToArray();
                var path = _imageStorage.SaveImage("club", clubId, bytes, file.ContentType);
                paths.Add(path);
            }
            return paths;
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

        public ClubDto RemoveImage(long userId, long clubId, string imagePath)
        {
            var club = _clubRepository.GetById(clubId);
            if (club == null)
                throw new KeyNotFoundException($"Club with ID {clubId} not found.");

            if (club.CreatorId != userId)
                throw new UnauthorizedAccessException("You can only modify your own clubs.");

            club.ImagePaths.Remove(imagePath);

            var updatedClub = _clubRepository.Update(club);
            return _mapper.Map<ClubDto>(updatedClub);
        }

    }
}