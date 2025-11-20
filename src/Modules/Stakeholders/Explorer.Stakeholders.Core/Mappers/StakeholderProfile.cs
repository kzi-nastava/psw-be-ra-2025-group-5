using AutoMapper;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.Core.Domain;
using System;

namespace Explorer.Stakeholders.Core.Mappers
{
    public class StakeholderProfile : Profile
    {
        public StakeholderProfile()
        {
            // AppRating mapiranje
            CreateMap<AppRating, AppRatingDto>().ReverseMap();
            CreateMap<AppRatingDto, AppRating>()
                .ConstructUsing(dto => new AppRating(dto.UserId, dto.Rating, dto.Comment));

            // User mapiranje
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
}
