using AutoMapper;
using Explorer.Encounters.API.Dtos;
using Explorer.Encounters.API.Public.Tourist;
using Explorer.Encounters.API.Internal;
using Explorer.Encounters.Core.Domain;
using Explorer.Encounters.Core.Domain.RepositoryInterfaces;
using Explorer.Encounters.Core.Domain.RepositoryInterfaces.Social;
using Explorer.Stakeholders.API.Internal;

namespace Explorer.Encounters.Core.UseCases.Tourist;

public class ChallengeExecutionService : IChallengeExecutionService
{
    private readonly IChallengeExecutionRepository _executionRepository;
    private readonly IChallengeRepository _challengeRepository;
    private readonly IInternalPersonExperienceService _personExperienceService;
    private readonly IMapper _mapper;
    private readonly IChallengeParticipationRepository _presenceRepository;
    private readonly IInternalBadgeService _badgeService;
    private readonly IPremiumSharedService _premiumService;

    public ChallengeExecutionService(
        IChallengeExecutionRepository executionRepository,
        IChallengeRepository challengeRepository,
        IInternalPersonExperienceService personExperienceService, 
        IChallengeParticipationRepository presenceRepository,
        IMapper mapper,
        IInternalBadgeService badgeService,
        IPremiumSharedService premiumService)
    {
        _executionRepository = executionRepository;
        _challengeRepository = challengeRepository;
        _personExperienceService = personExperienceService;
        _presenceRepository = presenceRepository;
        _mapper = mapper;
        _badgeService = badgeService;
        _premiumService = premiumService;
    }

    public ChallengeExecutionDto StartChallenge(long challengeId, long touristId)
    {
        Console.WriteLine($"[StartChallenge] TouristId = {touristId}");
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

        if (challenge.Type == ChallengeType.Social)
        {
            var activeTourists = _presenceRepository.GetActiveTourists(challenge.Id);
            if (activeTourists.Count < challenge.RequiredParticipants)
                throw new InvalidOperationException("Challenge cannot be completed yet – not enough participants in range.");
        }

        execution.Complete();
        var result = _executionRepository.Update(execution);

        var xp = GetAwardedXp(touristId, challenge.ExperiencePoints);
        _personExperienceService.AddExperience(touristId, xp);

        _badgeService.OnChallengeCompleted(touristId, (int)challenge.Type);
        
        return _mapper.Map<ChallengeExecutionDto>(result);
    }

    private int GetAwardedXp(long touristId, int baseXp)
    {
        return _premiumService.IsPremium(touristId) ? baseXp * 2 : baseXp;
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

    public void UpdateTouristLocation(long challengeId, long touristId, double latitude, double longitude)
    {
        var execution = _executionRepository.GetActiveByChallengeAndTourist(challengeId, touristId);
        if (execution == null) return;

        var challenge = _challengeRepository.Get(challengeId);
        if (challenge.Type != ChallengeType.Social)
            throw new InvalidOperationException("Challenge must be social.");

        var isInRange = IsWithinRadius(challenge, latitude, longitude);

        if (isInRange)
        {
            _presenceRepository.MarkActive(challengeId, touristId);
        } else
        {
            _presenceRepository.Remove(challengeId, touristId);
        }

        var activeTourists = _presenceRepository.GetActiveTourists(challengeId);

        if (activeTourists.Count >= challenge.RequiredParticipants)
        {
            CompleteForAll(challenge, activeTourists);
            _presenceRepository.Clear(challengeId);
        }
    }

    private void CompleteForAll(Challenge challenge, List<long> touristIds)
    {
        foreach (var touristId in touristIds)
        {
            var execution = _executionRepository
                .GetActiveByChallengeAndTourist(challenge.Id, touristId);

            if (execution == null) continue;

            execution.Complete();
            _executionRepository.Update(execution);

            var xp = GetAwardedXp(touristId, challenge.ExperiencePoints);
            _personExperienceService.AddExperience(touristId, xp);

            _badgeService.OnChallengeCompleted(touristId, (int)challenge.Type);
        }
    }

    private bool IsWithinRadius(Challenge challenge, double lat, double lon)
    {
        const double R = 6371000;
        double dLat = DegreesToRadians(lat - challenge.Latitude);
        double dLon = DegreesToRadians(lon - challenge.Longitude);

        double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) + Math.Cos(DegreesToRadians(challenge.Latitude)) * Math.Cos(DegreesToRadians(lat)) * Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        return R * c <= challenge.RadiusInMeters;
    }

    private double DegreesToRadians(double deg) => deg * Math.PI / 180;

}

