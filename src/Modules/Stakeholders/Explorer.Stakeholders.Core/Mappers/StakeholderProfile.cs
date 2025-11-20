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
    }
}