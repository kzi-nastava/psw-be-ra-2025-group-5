using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Encounters.API.Dtos;
using Explorer.Encounters.API.Public.Administration;
using Explorer.Encounters.Core.Domain;
using Explorer.Encounters.Core.Domain.RepositoryInterfaces;

namespace Explorer.Encounters.Core.UseCases.Administration;

public class ChallengeService : IChallengeService
{
    private readonly IChallengeRepository _challengeRepository;
    private readonly IMapper _mapper;

    public ChallengeService(IChallengeRepository repository, IMapper mapper)
    {
        _challengeRepository = repository;
        _mapper = mapper;
    }

    public PagedResult<ChallengeDto> GetPaged(int page, int pageSize)
    {
        var result = _challengeRepository.GetPaged(page, pageSize);
        var items = result.Results.Select(_mapper.Map<ChallengeDto>).ToList();
        return new PagedResult<ChallengeDto>(items, result.TotalCount);
    }

    public ChallengeDto Create(ChallengeDto entity)
    {
        var result = _challengeRepository.Create(_mapper.Map<Challenge>(entity));
        return _mapper.Map<ChallengeDto>(result);
    }

    public ChallengeDto Update(ChallengeDto entity)
    {
        var result = _challengeRepository.Update(_mapper.Map<Challenge>(entity));
        return _mapper.Map<ChallengeDto>(result);
    }

    public void Delete(long id)
    {
        _challengeRepository.Delete(id);
    }

    public List<ChallengeDto> GetAllActive()
    {
        var result = _challengeRepository.GetAll().Where(c => c.Status == ChallengeStatus.Active);
        return result.Select(_mapper.Map<ChallengeDto>).ToList();
    }

    public ChallengeDto GetById(long challengeId)
    {
        var result = _challengeRepository.Get(challengeId);
        return _mapper.Map<ChallengeDto>(result);
    }

    public void Approve(long challengeId)
    {
        Challenge challenge = _challengeRepository.Get(challengeId);
        if (challenge == null) 
            throw new KeyNotFoundException();

        if (challenge.Status != ChallengeStatus.Pending)
            throw new InvalidOperationException("Challenge is not pending approval.");

        challenge.Update(
            challenge.Name,
            challenge.Description,
            challenge.Latitude,
            challenge.Longitude,
            challenge.ExperiencePoints,
            ChallengeStatus.Active,
            challenge.Type,
            challenge.RequiredParticipants,
            challenge.RadiusInMeters
        );

        _challengeRepository.Update( challenge );
    }

    public void Reject(long challengeId)
    {
        Challenge challenge = _challengeRepository.Get(challengeId);
        if (challenge == null)
            throw new KeyNotFoundException();

        if (challenge.Status != ChallengeStatus.Pending)
            throw new InvalidOperationException("Challenge is not pending approval.");

        challenge.Update(
            challenge.Name,
            challenge.Description,
            challenge.Latitude,
            challenge.Longitude,
            challenge.ExperiencePoints,
            ChallengeStatus.Archived,
            challenge.Type,
            challenge.RequiredParticipants,
            challenge.RadiusInMeters
        );

        _challengeRepository.Update(challenge);
    }
}
