using AutoMapper;
using Explorer.BuildingBlocks.Core.Exceptions;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using System.Net.Mail;

namespace Explorer.Stakeholders.Core.UseCases;

public class ProfileService : IProfileService
{
    private readonly IPersonRepository _personRepository;
    private readonly IMapper _mapper;

    public ProfileService(IPersonRepository personRepository, IMapper mapper)
    {
        _personRepository = personRepository;
        _mapper = mapper;
    }

    public ProfileDto GetByUserId(long userId)
    {
        try
        {
            var person = _personRepository.GetByUserId(userId);
            if (person == null)
                throw new KeyNotFoundException("Profile not found for this user.");

            return _mapper.Map<ProfileDto>(person);
        }
        catch (Exception ex)
        {
            Console.WriteLine("PROFILE ERROR: " + ex.ToString());
            throw;
        }
    }


    public ProfileDto Update(ProfileDto profile)
    {
        // Try to find the person by Person.Id first
        var existing = _personRepository.Get(profile.Id);

        // If not found, try to find by UserId (client may have provided User.Id instead)
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
