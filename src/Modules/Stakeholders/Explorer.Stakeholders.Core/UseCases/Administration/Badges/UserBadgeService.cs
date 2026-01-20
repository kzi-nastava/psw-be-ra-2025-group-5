using AutoMapper;
using Explorer.Stakeholders.API.Dtos.Badges;
using Explorer.Stakeholders.API.Public.Badges;
using Explorer.Stakeholders.Core.Domain.Badges;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces.Badges;

namespace Explorer.Stakeholders.Core.UseCases.Administration.Badges;

public class UserBadgeService : IUserBadgeService
{
    private readonly IUserBadgeRepository _repository;
    private readonly IBadgeRepository _badgeRepository;
    private readonly IMapper _mapper;

    public UserBadgeService(IUserBadgeRepository repository, IBadgeRepository badgeRepository, IMapper mapper)
    {
        _repository = repository;
        _badgeRepository = badgeRepository;
        _mapper = mapper;
    }

    public UserBadgeDto AwardBadge(long userId, long badgeId)
    {
        if (_repository.HasBadge(userId, badgeId))
        {
            throw new InvalidOperationException($"User {userId} already has badge {badgeId}");
        }

        var userBadge = new UserBadge(userId, badgeId);
        var result = _repository.Create(userBadge);
        return _mapper.Map<UserBadgeDto>(result);
    }

    public UserBadgeDto Get(long id)
    {
        var entity = _repository.Get(id);
        if (entity == null)
            throw new KeyNotFoundException($"UserBadge with Id={id} not found.");
        return _mapper.Map<UserBadgeDto>(entity);
    }

    public List<UserBadgeDto> GetByUserId(long userId)
    {
        var entities = _repository.GetByUserId(userId);
        var dtos = _mapper.Map<List<UserBadgeDto>>(entities);

        foreach (var dto in dtos)
        {
            var badge = _badgeRepository.Get(dto.BadgeId);
            if (badge != null)
            {
                dto.Badge = _mapper.Map<BadgeDto>(badge);
            }
        }

        return dtos;
    }

    public List<UserBadgeDto> GetBestBadgesByUserId(long userId)
    {
        var allUserBadges = GetByUserId(userId);
        
        // Grupisanje po tipu bedža
        var badgesByType = allUserBadges
            .Where(ub => ub.Badge != null)
            .GroupBy(ub => ub.Badge.Type)
            .Select(group => group
                .OrderByDescending(ub => ub.Badge.Rank)
                .ThenByDescending(ub => ub.Badge.RequiredValue)
                .First())
            .ToList();

        return badgesByType;
    }

    public bool HasBadge(long userId, long badgeId)
    {
        return _repository.HasBadge(userId, badgeId);
    }
}
