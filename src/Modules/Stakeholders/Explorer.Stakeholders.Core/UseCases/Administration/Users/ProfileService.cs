using AutoMapper;
using Explorer.BuildingBlocks.Core.Exceptions;
using Explorer.Stakeholders.API.Dtos.Users;
using Explorer.Stakeholders.API.Public.Statistics;
using Explorer.Stakeholders.API.Public.Users;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces.Users;
using System;
using System.Net.Mail;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Explorer.Stakeholders.Core.UseCases.Administration.Users;

public class ProfileService : IProfileService
{
    private readonly IPersonRepository _personRepository;
    private readonly IMapper _mapper;
    ITouristStatisticsService _touristStatisticsService;

    public ProfileService(IPersonRepository personRepository, IMapper mapper, ITouristStatisticsService touristStatisticsService)
    {
        _personRepository = personRepository;
        _mapper = mapper;
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


    public ProfileDto Update(ProfileDto profile)
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

        if (!string.IsNullOrEmpty(profile.ProfileImageBase64))
        {
            existing.ProfileImage = Convert.FromBase64String(profile.ProfileImageBase64);
        }

        var updated = _personRepository.Update(existing);
        return _mapper.Map<ProfileDto>(updated);
    }
}
