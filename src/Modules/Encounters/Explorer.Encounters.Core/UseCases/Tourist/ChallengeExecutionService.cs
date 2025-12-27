using AutoMapper;
using Explorer.Encounters.API.Dtos;
using Explorer.Encounters.API.Public.Tourist;
using Explorer.Encounters.API.Internal;
using Explorer.Encounters.Core.Domain;
using Explorer.Encounters.Core.Domain.RepositoryInterfaces;

namespace Explorer.Encounters.Core.UseCases.Tourist;

public class ChallengeExecutionService : IChallengeExecutionService
{
    private readonly IChallengeExecutionRepository _executionRepository;
    private readonly IChallengeRepository _challengeRepository;
    private readonly IInternalPersonExperienceService _personExperienceService;
    private readonly IMapper _mapper;

    public ChallengeExecutionService(
        IChallengeExecutionRepository executionRepository,
        IChallengeRepository challengeRepository,
        IInternalPersonExperienceService personExperienceService,
        IMapper mapper)
    {
        _executionRepository = executionRepository;
        _challengeRepository = challengeRepository;
        _personExperienceService = personExperienceService;
        _mapper = mapper;
    }

    public ChallengeExecutionDto StartChallenge(long challengeId, long touristId)
    {
        var challenges = _challengeRepository.GetAll();
        var challenge = challenges.FirstOrDefault(c => c.Id == challengeId);
        
        if (challenge == null)
            throw new KeyNotFoundException("Challenge not found.");

        if (challenge.Status != ChallengeStatus.Active)
            throw new InvalidOperationException("Challenge is not active.");

        var existingExecution = _executionRepository.GetActiveByChallengeAndTourist(challengeId, touristId);
        if (existingExecution != null)
            throw new InvalidOperationException("You already have an active execution for this challenge.");

        var completedExecution = _executionRepository.GetCompletedByChallengeAndTourist(challengeId, touristId);
        if (completedExecution != null)
            throw new InvalidOperationException("You already completed this challenge.");

        var execution = new ChallengeExecution(challengeId, touristId);
        var result = _executionRepository.Create(execution);
        
        return _mapper.Map<ChallengeExecutionDto>(result);
    }

    public ChallengeExecutionDto CompleteChallenge(long executionId, long touristId)
    {
        var execution = _executionRepository.Get(executionId);
        
        if (execution.TouristId != touristId)
            throw new UnauthorizedAccessException("You are not authorized to complete this challenge execution.");

        var challenge = _challengeRepository.GetAll().FirstOrDefault(c => c.Id == execution.ChallengeId);
        if (challenge == null)
            throw new KeyNotFoundException("Challenge not found.");

        execution.Complete();
        var result = _executionRepository.Update(execution);

        _personExperienceService.AddExperience(touristId, challenge.ExperiencePoints);
        
        return _mapper.Map<ChallengeExecutionDto>(result);
    }

    public ChallengeExecutionDto AbandonChallenge(long executionId, long touristId)
    {
        var execution = _executionRepository.Get(executionId);
        
        if (execution.TouristId != touristId)
            throw new UnauthorizedAccessException("You are not authorized to abandon this challenge execution.");

        execution.Abandon();
        var result = _executionRepository.Update(execution);
        
        return _mapper.Map<ChallengeExecutionDto>(result);
    }

    public ChallengeExecutionDto GetById(long id)
    {
        var execution = _executionRepository.Get(id);
        return _mapper.Map<ChallengeExecutionDto>(execution);
    }

    public List<ChallengeExecutionDto> GetByTourist(long touristId)
    {
        var executions = _executionRepository.GetByTourist(touristId);
        return executions.Select(_mapper.Map<ChallengeExecutionDto>).ToList();
    }
}

