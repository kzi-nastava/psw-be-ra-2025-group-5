using AutoMapper;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.Core.Domain;

namespace Explorer.Stakeholders.Core.Mappers;

public class StakeholderProfile : Profile
{
    public StakeholderProfile()
    {
        CreateMap<Person, ProfileDto>()
     .ForMember(dest => dest.ProfileImageBase64, opt => opt.MapFrom(src => src.ProfileImage != null ? Convert.ToBase64String(src.ProfileImage) : string.Empty))
     .ReverseMap();

        CreateMap<UserDto, User>().ReverseMap();

        CreateMap<CreateUserDto, User>()
            .ConstructUsing(dto => new User(
                dto.Username,
                dto.Password,
                dto.Email,
                Enum.Parse<UserRole>(dto.Role, true),
                true
            ));
    }
}