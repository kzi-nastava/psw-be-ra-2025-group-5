using AutoMapper;
using Explorer.BuildingBlocks.Core.Exceptions;
using Explorer.BuildingBlocks.Core.FileStorage;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.API.Public.Statistics;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Net.Mail;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Explorer.Stakeholders.Core.UseCases;

public class ProfileService : IProfileService
{
    private readonly IPersonRepository _personRepository;
    private readonly IMapper _mapper;
    private readonly IImageStorage _imageStorage;
    ITouristStatisticsService _touristStatisticsService;

    public ProfileService(IPersonRepository personRepository, IMapper mapper, ITouristStatisticsService touristStatisticsService, IImageStorage imageStorage)
    {
        _personRepository = personRepository;
        _mapper = mapper;
        _imageStorage = imageStorage;
        _touristStatisticsService = touristStatisticsService;
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
    
}
