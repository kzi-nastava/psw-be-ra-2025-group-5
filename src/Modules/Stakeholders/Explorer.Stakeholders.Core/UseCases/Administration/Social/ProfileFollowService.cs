using AutoMapper;
using Explorer.BuildingBlocks.Core.Exceptions;
using Explorer.Stakeholders.API.Dtos.Social;
using Explorer.Stakeholders.API.Public.Social;
using Explorer.Stakeholders.API.Public.Users;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces.Social;
using Explorer.Stakeholders.Core.Domain.Social;

namespace Explorer.Stakeholders.Core.UseCases.Administration.Social;

public class ProfileFollowService : IProfileFollowService
{
    private readonly IProfileFollowRepository _followRepository;
    private readonly IProfileService _personService;
    private readonly IMapper _mapper;

    public ProfileFollowService(IProfileFollowRepository followRepository, IProfileService profileService, IMapper mapper)
    {
        _followRepository = followRepository;
        _personService = profileService;
        _mapper = mapper;
    }

    public bool Exists(ProfileFollowDto follow)
    {
        return _followRepository.Exists(follow.FollowerId, follow.FollowingId);
    }

    public ProfileFollowDto Follow(ProfileFollowDto follow)
    {
        if (_personService.Get(follow.FollowerId) == null || _personService.Get(follow.FollowingId) == null)
            throw new NotFoundException("Follower or following profile does not exist.");

        if(_followRepository.Exists(follow.FollowerId, follow.FollowingId))
            throw new InvalidOperationException("The follow relationship already exists.");

        var entity = _followRepository.Add(_mapper.Map<ProfileFollow>(follow));
        return _mapper.Map<ProfileFollowDto>(entity);
    }

    public void Unfollow(ProfileFollowDto follow)
    {
        _followRepository.Remove(_mapper.Map<ProfileFollow>(follow));
    }

    public async Task<IEnumerable<FollowerDto>> GetFollowers(long profileId)
    {
        var followers = await _followRepository.GetFollowers(profileId);
        return _mapper.Map<List<FollowerDto>>(followers);
    }

    public async Task<IEnumerable<FollowingDto>> GetFollowing(long profileId)
    {
        var following = await _followRepository.GetFollowing(profileId);
        return _mapper.Map<List<FollowingDto>>(following);
    }
}
