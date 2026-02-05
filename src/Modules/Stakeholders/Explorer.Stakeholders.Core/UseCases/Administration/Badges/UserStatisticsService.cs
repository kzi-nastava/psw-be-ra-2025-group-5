using AutoMapper;
using Explorer.Stakeholders.API.Dtos.Badges;
using Explorer.Stakeholders.API.Public.Badges;
using Explorer.Stakeholders.Core.Domain.Badges;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces.Badges;

namespace Explorer.Stakeholders.Core.UseCases.Administration.Badges;

public class UserStatisticsService : IUserStatisticsService
{
    private readonly IUserStatisticsRepository _repository;
    private readonly IMapper _mapper;

    public UserStatisticsService(IUserStatisticsRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public UserStatisticsDto Create(long userId)
    {
        var existing = _repository.GetByUserId(userId);
        if (existing != null)
        {
            throw new InvalidOperationException($"User statistics for user {userId} already exist.");
        }

        var userStatistics = new UserStatistics(userId);
        var result = _repository.Create(userStatistics);
        return _mapper.Map<UserStatisticsDto>(result);
    }

    public UserStatisticsDto Get(long id)
    {
        var entity = _repository.Get(id);
        if (entity == null)
            throw new KeyNotFoundException($"UserStatistics with Id={id} not found.");
        return _mapper.Map<UserStatisticsDto>(entity);
    }

    public UserStatisticsDto GetByUserId(long userId)
    {
        var entity = _repository.GetByUserId(userId);
        if (entity == null)
        {
            return Create(userId);
        }
        return _mapper.Map<UserStatisticsDto>(entity);
    }

    public UserStatisticsDto Update(UserStatisticsDto dto)
    {
        var entity = _repository.Get(dto.Id);
        if (entity == null)
            throw new KeyNotFoundException($"UserStatistics with Id={dto.Id} not found.");

        var result = _repository.Update(entity);
        return _mapper.Map<UserStatisticsDto>(result);
    }

    public void Delete(long id)
    {
        _repository.Delete(id);
    }

    public UserStatisticsDto UpdateLevel(long userId, int level)
    {
        var entity = _repository.GetByUserId(userId);
        if (entity == null)
        {
            entity = new UserStatistics(userId);
            entity = _repository.Create(entity);
        }

        entity.UpdateLevel(level);
        var result = _repository.Update(entity);
        return _mapper.Map<UserStatisticsDto>(result);
    }

    public UserStatisticsDto UpdateAccountAgeDays(long userId, int days)
    {
        var entity = _repository.GetByUserId(userId);
        if (entity == null)
        {
            entity = new UserStatistics(userId);
            entity = _repository.Create(entity);
        }

        entity.UpdateAccountAgeDays(days);
        var result = _repository.Update(entity);
        return _mapper.Map<UserStatisticsDto>(result);
    }

    public UserStatisticsDto IncrementCompletedTours(long userId)
    {
        var entity = _repository.GetByUserId(userId);
        if (entity == null)
        {
            entity = new UserStatistics(userId);
            entity = _repository.Create(entity);
        }

        entity.IncrementCompletedTours();
        var result = _repository.Update(entity);
        return _mapper.Map<UserStatisticsDto>(result);
    }

    public UserStatisticsDto IncrementCompletedChallenges(long userId)
    {
        var entity = _repository.GetByUserId(userId);
        if (entity == null)
        {
            entity = new UserStatistics(userId);
            entity = _repository.Create(entity);
        }

        entity.IncrementCompletedChallenges();
        var result = _repository.Update(entity);
        return _mapper.Map<UserStatisticsDto>(result);
    }

    public UserStatisticsDto MarkChallengeTypeCompleted(long userId, int challengeType)
    {
        var entity = _repository.GetByUserId(userId);
        if (entity == null)
        {
            entity = new UserStatistics(userId);
            entity = _repository.Create(entity);
        }

        entity.MarkChallengeTypeCompleted(challengeType);
        var result = _repository.Update(entity);
        return _mapper.Map<UserStatisticsDto>(result);
    }

    public UserStatisticsDto IncrementPublishedTours(long userId)
    {
        var entity = _repository.GetByUserId(userId);
        if (entity == null)
        {
            entity = new UserStatistics(userId);
            entity = _repository.Create(entity);
        }

        entity.IncrementPublishedTours();
        var result = _repository.Update(entity);
        return _mapper.Map<UserStatisticsDto>(result);
    }

    public UserStatisticsDto IncrementSoldTours(long userId)
    {
        var entity = _repository.GetByUserId(userId);
        if (entity == null)
        {
            entity = new UserStatistics(userId);
            entity = _repository.Create(entity);
        }

        entity.IncrementSoldTours();
        var result = _repository.Update(entity);
        return _mapper.Map<UserStatisticsDto>(result);
    }

    public UserStatisticsDto IncrementBlogPosts(long userId)
    {
        var entity = _repository.GetByUserId(userId);
        if (entity == null)
        {
            entity = new UserStatistics(userId);
            entity = _repository.Create(entity);
        }

        entity.IncrementBlogPosts();
        var result = _repository.Update(entity);
        return _mapper.Map<UserStatisticsDto>(result);
    }

    public UserStatisticsDto SetJoinedClub(long userId, bool joined)
    {
        var entity = _repository.GetByUserId(userId);
        if (entity == null)
        {
            entity = new UserStatistics(userId);
            entity = _repository.Create(entity);
        }

        entity.SetJoinedClub(joined);
        var result = _repository.Update(entity);
        return _mapper.Map<UserStatisticsDto>(result);
    }
}
