using AutoMapper;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.Core.Domain;
using System;
using System.Linq;

namespace Explorer.Stakeholders.Core.Mappers
{
    public class StakeholderProfile : Profile
    {
        public StakeholderProfile()
        {
            // ========================= Person <-> ProfileDto =========================
            CreateMap<Person, ProfileDto>()
                .ForMember(
                    dest => dest.ProfileImageBase64,
                    opt => opt.MapFrom(src => src.ProfileImage != null
                        ? Convert.ToBase64String(src.ProfileImage)
                        : string.Empty)
                )
                .ReverseMap();

            // ========================= AppRating <-> AppRatingDto =========================
            CreateMap<AppRating, AppRatingDto>();

            CreateMap<AppRatingDto, AppRating>()
                .ConstructUsing(dto => new AppRating(dto.UserId, dto.Rating, dto.Comment));

            // ========================= User mapiranja =========================
            CreateMap<UserDto, User>().ReverseMap();

            CreateMap<CreateUserDto, User>()
                .ConstructUsing(dto => new User(
                    dto.Username,
                    dto.Password,
                    dto.Email,
                    Enum.Parse<UserRole>(dto.Role, true),
                    true
                ));

            // ========================= Club <-> ClubDto =========================
            CreateMap<Club, ClubDto>()
                .ForMember(
                    dest => dest.Images,
                    opt => opt.MapFrom(src =>
                        src.Images != null
                            ? src.Images.Select(img => Convert.ToBase64String(img)).ToList()
                            : new List<string>())
                );

            CreateMap<ClubDto, Club>()
                .ForMember(
                    dest => dest.Images,
                    opt => opt.MapFrom(src =>
                        src.Images != null
                            ? src.Images.Select(img => Convert.FromBase64String(img)).ToList()
                            : new List<byte[]>())
                );

            CreateMap<TourProblem, TourProblemDto>()
    .ForMember(dest => dest.Comments, opt => opt.Ignore())
    .ConstructUsing(src => new TourProblemDto
    {
        Id = src.Id,
        TourId = src.TourId,
        ReporterId = src.ReporterId,
        Category = (API.Dtos.ProblemCategory)src.Category,
        Priority = (API.Dtos.ProblemPriority)src.Priority,
        Description = src.Description,
        OccurredAt = src.OccurredAt,
        CreatedAt = src.CreatedAt,
        IsResolved = src.IsResolved,
        Deadline = src.Deadline, // direktno mapira vrednost
        Comments = new List<CommentDto>() // ili mapiranje komentara
    });



            CreateMap<TourProblemDto, TourProblem>();

            // ========================= Position <-> PositionDto =========================
            CreateMap<PositionDto, Position>().ReverseMap();

            CreateMap<Comment, CommentDto>();

        }
    }
}
