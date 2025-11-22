using AutoMapper;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.API.Dtos;

using Explorer.Stakeholders.API.Dtos;
namespace Explorer.Stakeholders.Core.Mappers;

public class StakeholderProfile : Profile
{
    public StakeholderProfile()
    {
        CreateMap<UserDto, User>().ReverseMap();

        CreateMap<CreateUserDto, User>()
            .ConstructUsing(dto => new User(
                dto.Username,
                dto.Password,
                dto.Email,
                Enum.Parse<UserRole>(dto.Role, true),
                true
            ));

        CreateMap<Club, ClubDto>()
            .ForMember(dest => dest.Images,
                opt => opt.MapFrom(src => src.Images.Select(img => Convert.ToBase64String(img)).ToList()));

        CreateMap<ClubDto, Club>()
            .ForMember(dest => dest.Images,
                opt => opt.MapFrom(src => src.Images.Select(img => Convert.FromBase64String(img)).ToList()));
    }
}