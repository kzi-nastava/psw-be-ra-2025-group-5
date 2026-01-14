using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Explorer.Encounters.API.Dtos;
using Explorer.Encounters.API.Public.Author;
using Explorer.Encounters.Core.Domain.RepositoryInterfaces;
using Explorer.Encounters.Core.Domain;

namespace Explorer.Encounters.Core.UseCases.Author
{
    public class KeyPointChallengeCreationService : IKeyPointChallengeCreationService
    {
        private readonly IKeyPointChallengeRepository _keyPointChallengeRepository;
        private readonly IChallengeRepository _challengeRepository;
        private readonly IMapper _mapper;

        public KeyPointChallengeCreationService(IKeyPointChallengeRepository keyPointChallengeRepository, IChallengeRepository challengeRepository, IMapper mapper)
        {
            _keyPointChallengeRepository = keyPointChallengeRepository;
            _challengeRepository = challengeRepository;
            _mapper = mapper;
        }

        public KeyPointChallengeDto CreateByAuthor(CreateAuthorChallengeDto dto, long profileId)
        {
            if (!Enum.TryParse<ChallengeType>(dto.Type, true, out var type))
            {
                throw new ArgumentException("Invalid ChallengeType");
            }

            var newChallange = new Challenge(dto.Name, dto.Description, dto.Latitude, dto.Longitude, dto.ExperiencePoints, ChallengeStatus.Draft, type, profileId);

            var addedChallenge = _challengeRepository.Create(newChallange);

            var newKeyPointChallenge = new KeyPointChallenge(addedChallenge.Id, dto.KeyPointId, dto.IsRequiredForSecret, dto.IsRequiredForCompletion);

            var result = _keyPointChallengeRepository.Create(newKeyPointChallenge);
            return _mapper.Map<KeyPointChallengeDto>(result);
        }
    }
}
