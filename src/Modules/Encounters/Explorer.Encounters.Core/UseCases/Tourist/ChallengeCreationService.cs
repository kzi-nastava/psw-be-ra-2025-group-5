using AutoMapper;
using Explorer.BuildingBlocks.Core.FileStorage;
using Explorer.Encounters.API.Dtos;
using Explorer.Encounters.API.Public.Tourist;
using Explorer.Encounters.Core.Domain;
using Explorer.Encounters.Core.Domain.RepositoryInterfaces;
using Microsoft.AspNetCore.Http;

namespace Explorer.Encounters.Core.UseCases.Tourist
{
    public class ChallengeCreationService : IChallengeCreationService
    {
        private readonly IChallengeRepository _challengeRepository;
        private readonly IMapper _mapper;
        private readonly IImageStorage _imageStorage;

        public ChallengeCreationService(IChallengeRepository repository, IMapper mapper, IImageStorage imageStorage)
        {
            _challengeRepository = repository;
            _mapper = mapper;
            _imageStorage = imageStorage;
        }

        public ChallengeResponseDto CreateByTourist(ChallengeResponseDto dto, long creatorId, IFormFile? image)
        {
            if (!Enum.TryParse<ChallengeType>(dto.Type, true, out var type))
            {
                throw new ArgumentException("Invalid ChallengeType");
            }
            var challenge = new Challenge(
                dto.Name,
                dto.Description,
                dto.Latitude,
                dto.Longitude,
                dto.ExperiencePoints,
                ChallengeStatus.Pending,
                type,
                creatorId,
                dto.RequiredParticipants ?? null,
                dto.RadiusInMeters ?? null
            );

            var result = _challengeRepository.Create(challenge);
            string? imagePath = SaveImage(result.Id, image);
            result.UpdateImage(imagePath);
            _challengeRepository.Update(result);
            return _mapper.Map<ChallengeResponseDto>(result);
        }

        public ChallengeResponseDto Update(UpdateTouristChallengeDto entity, long userId)
        {
            var existingChallenge = _challengeRepository.Get(entity.Id);
            if (existingChallenge == null)
                throw new KeyNotFoundException($"Challenge with id {entity.Id} not found.");

            if (existingChallenge.CreatedByTouristId != userId)
                throw new UnauthorizedAccessException("You can only edit your own challenges.");

            string? imagePath;
            if (entity.Image != null)
            {
                imagePath = SaveImage(entity.Id, entity.Image);
            }
            else
            {
                imagePath = existingChallenge.ImageUrl;
            }
            existingChallenge.UpdateImage(imagePath);
            _mapper.Map(entity, existingChallenge);

            var updatedChallenge = _challengeRepository.Update(existingChallenge);
            return _mapper.Map<ChallengeResponseDto>(updatedChallenge);
        }

        public List<ChallengeResponseDto> GetByTourist(long touristId)
        {
            var challenges = _challengeRepository.GetAll()
                .Where(c => c.CreatedByTouristId == touristId)
                .ToList();

            return _mapper.Map<List<ChallengeResponseDto>>(challenges);
        }

        private string? SaveImage(long challengeId, IFormFile file)
        {
            if (file == null)
                return null;
            using var ms = new MemoryStream();
            file.CopyTo(ms);
            var bytes = ms.ToArray();
            string path = _imageStorage.SaveImage("challenges", challengeId, bytes, file.ContentType);
            return path;
        }

    }
}
