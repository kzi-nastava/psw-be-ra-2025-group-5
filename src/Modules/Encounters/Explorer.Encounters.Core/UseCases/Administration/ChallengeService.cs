using AutoMapper;
using Explorer.BuildingBlocks.Core.FileStorage;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Encounters.API.Dtos;
using Explorer.Encounters.API.Public.Administration;
using Explorer.Encounters.Core.Domain;
using Explorer.Encounters.Core.Domain.RepositoryInterfaces;
using Microsoft.AspNetCore.Http;

namespace Explorer.Encounters.Core.UseCases.Administration;

public class ChallengeService : IChallengeService
{
    private readonly IChallengeRepository _challengeRepository;
    private readonly IMapper _mapper;
    private readonly IImageStorage _imageStorage;

    public ChallengeService(IChallengeRepository repository, IMapper mapper, IImageStorage imageStorage)
    {
        _challengeRepository = repository;
        _mapper = mapper;
        _imageStorage = imageStorage;
    }

    public PagedResult<ChallengeResponseDto> GetPaged(int page, int pageSize)
    {
        var result = _challengeRepository.GetPaged(page, pageSize);
        var items = result.Results.Select(_mapper.Map<ChallengeResponseDto>).ToList();
        return new PagedResult<ChallengeResponseDto>(items, result.TotalCount);
    }

    public ChallengeResponseDto Create(ChallengeResponseDto entity, IFormFile? image)
    {
        var result = _challengeRepository.Create(_mapper.Map<Challenge>(entity));
        if (image != null)
        {
            var imagePath = SaveImage(result.Id, image);
            result.Update(
                result.Name,
                result.Description,
                result.Latitude,
                result.Longitude,
                result.ExperiencePoints,
                result.Status,
                result.Type,
                result.RequiredParticipants,
                result.RadiusInMeters,
                imagePath
            );
            _challengeRepository.Update(result);
        }
        return _mapper.Map<ChallengeResponseDto>(result);
    }

    public ChallengeResponseDto Update(ChallengeResponseDto entity, IFormFile? image)
    {

        var result = _challengeRepository.Update(_mapper.Map<Challenge>(entity));
        var imagePath = SaveImage(result.Id, image);
        result.Update(
            result.Name,
            result.Description,
            result.Latitude,
            result.Longitude,
            result.ExperiencePoints,
            result.Status,
            result.Type,
            result.RequiredParticipants,
            result.RadiusInMeters,
            imagePath
        );
        _challengeRepository.Update(result);
        return _mapper.Map<ChallengeResponseDto>(result);
    }

    public void Delete(long id)
    {
        _challengeRepository.Delete(id);
    }

    public List<ChallengeResponseDto> GetAllActive()
    {
        var result = _challengeRepository.GetAll().Where(c => c.Status == ChallengeStatus.Active);
        return result.Select(_mapper.Map<ChallengeResponseDto>).ToList();
    }

    public ChallengeResponseDto GetById(long challengeId)
    {
        var result = _challengeRepository.Get(challengeId);
        return _mapper.Map<ChallengeResponseDto>(result);
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
            challenge.RadiusInMeters,
            challenge.ImageUrl
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
            challenge.RadiusInMeters,
            challenge.ImageUrl
        );

        _challengeRepository.Update(challenge);
    }

    private string SaveImage(long challengeId, IFormFile file)
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
