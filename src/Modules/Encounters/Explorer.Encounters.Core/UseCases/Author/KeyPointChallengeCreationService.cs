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
        private readonly IMapper _mapper;

        public KeyPointChallengeCreationService(IKeyPointChallengeRepository keyPointChallengeRepository, IMapper mapper)
        {
            _keyPointChallengeRepository = keyPointChallengeRepository;
            _mapper = mapper;
        }

        public CreateAuthorChallengeDto CreateByAuthor(CreateAuthorChallengeDto dto, long authorId)
        {
            var result = _keyPointChallengeRepository.Create(_mapper.Map<KeyPointChallenge>(dto));
            return _mapper.Map<CreateAuthorChallengeDto>(result);
        }
    }
}
