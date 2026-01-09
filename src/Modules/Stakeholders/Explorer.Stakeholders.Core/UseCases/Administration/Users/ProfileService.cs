using AutoMapper;
using Explorer.BuildingBlocks.Core.Exceptions;
using Explorer.BuildingBlocks.Core.FileStorage;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos.Tours.Problems;
using Explorer.Stakeholders.API.Dtos.Users;
using Explorer.Stakeholders.API.Public.Statistics;
using Explorer.Stakeholders.API.Public.Users;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces.Users;
using Explorer.Stakeholders.Core.Domain.Users;
using Microsoft.AspNetCore.Http;
using System;
using System.Net.Mail;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Explorer.Stakeholders.Core.UseCases.Administration.Users;

public class ProfileService : IProfileService
{
    private readonly IPersonRepository _personRepository;
    private readonly IMapper _mapper;
    private readonly IImageStorage _imageStorage;
    ITouristStatisticsService _touristStatisticsService;
    private readonly IUserRepository _userRepository;

    public ProfileService(IPersonRepository personRepository, IMapper mapper, ITouristStatisticsService touristStatisticsService, IImageStorage imageStorage, IUserRepository userRepository)
    {
        _personRepository = personRepository;
        _mapper = mapper;
        _imageStorage = imageStorage;
        _touristStatisticsService = touristStatisticsService;
        _userRepository = userRepository;
    }

    public ProfileDto GetByUserId(long userId)
    {
        try
        {
            var person = _personRepository.GetByUserId(userId);
            if (person == null)
                throw new KeyNotFoundException("Profile not found for this user.");

            var profileDto = _mapper.Map<ProfileDto>(person);
            profileDto.Statistics = _touristStatisticsService.GetStatistics(userId);
            return profileDto;

        }
        catch (Exception ex)
        {
            Console.WriteLine("PROFILE ERROR: " + ex.ToString());
            throw;
        }
    }

    public ProfileDto GetPublicProfile(long userId)
    {
        var person = _personRepository.GetByUserId(userId);
        if (person == null)
            throw new KeyNotFoundException("Profile not found.");

        var user = _userRepository.GetById(person.UserId);

        if (user.Role == UserRole.Administrator)
            throw new ForbiddenException("You are not allowed to view admin profiles.");

        var profileDto = _mapper.Map<ProfileDto>(person);
        profileDto.Statistics = _touristStatisticsService.GetStatistics(userId);

        return profileDto;
    }


    public ProfileDto Update(ProfileDto profile, IFormFile? profileImage)
    {
        var existing = _personRepository.Get(profile.Id);

        if (existing == null)
        {
            existing = _personRepository.GetByUserId(profile.Id);
        }

        if (existing == null)
            throw new KeyNotFoundException("Profile not found.");

        existing.Name = profile.Name;
        existing.Surname = profile.Surname;

        if (!string.IsNullOrWhiteSpace(profile.Email))
        {
            if (!MailAddress.TryCreate(profile.Email, out _))
                throw new ArgumentException("Invalid Email");
            existing.Email = profile.Email;
        }

        existing.Biography = profile.Biography;
        existing.Motto = profile.Motto;

        if (profileImage != null && profileImage.Length > 0)
        {

            try
            {
                var imagePath = SaveImages(profile.Id, new List<IFormFile> { profileImage }).First();
                existing.ProfileImagePath = imagePath;
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Failed to save profile image: {ex.Message}", ex);
            }
        }

        var updated = _personRepository.Update(existing);
        return _mapper.Map<ProfileDto>(updated);
    }
    
    private List<string> SaveImages(long personId, List<IFormFile> images)
    {
        var paths = new List<string>();
        foreach (var file in images)
        {
            using var ms = new MemoryStream();
            file.CopyTo(ms);
            var bytes = ms.ToArray();
            var path = _imageStorage.SaveImage("profiles", personId, bytes, file.ContentType);
            paths.Add(path);
        }
        return paths;
    }

    public PagedResult<ProfileDto> GetPaged(int page, int pageSize)
    {
        var pagedPersons = _personRepository.GetPaged(page, pageSize);

        var profileDtos = pagedPersons.Results.Select(person =>
        {
            var dto = _mapper.Map<ProfileDto>(person);
            dto.Statistics = _touristStatisticsService.GetStatistics(person.UserId);

            return dto;
        }).ToList();

        return new PagedResult<ProfileDto>(profileDtos, pagedPersons.TotalCount);
    }

}
