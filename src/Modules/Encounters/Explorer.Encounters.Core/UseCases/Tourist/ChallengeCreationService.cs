using AutoMapper;
using Explorer.Encounters.API.Dtos;
using Explorer.Encounters.API.Public.Tourist;
using Explorer.Encounters.Core.Domain;
using Explorer.Encounters.Core.Domain.RepositoryInterfaces;

namespace Explorer.Encounters.Core.UseCases.Tourist
{
    public class ChallengeCreationService : IChallengeCreationService
    {
        private readonly IChallengeRepository _challengeRepository;
        private readonly IMapper _mapper;

        public ChallengeCreationService(IChallengeRepository repository, IMapper mapper)
        {
            _challengeRepository = repository;
            _mapper = mapper;
        }

        public ChallengeDto CreateByTourist(CreateTouristChallengeDto dto, long creatorId)
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
            return _mapper.Map<ChallengeDto>(result);
        }

    }
}
