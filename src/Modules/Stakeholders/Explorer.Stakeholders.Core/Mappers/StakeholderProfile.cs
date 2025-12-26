using AutoMapper;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Dtos.AppRatings;
using Explorer.Stakeholders.API.Dtos.Clubs;
using Explorer.Stakeholders.API.Dtos.Comments;
using Explorer.Stakeholders.API.Dtos.Diaries;
using Explorer.Stakeholders.API.Dtos.Locations;
using Explorer.Stakeholders.API.Dtos.Notifications;
using Explorer.Stakeholders.API.Dtos.Tours.Problems;
using Explorer.Stakeholders.API.Dtos.Users;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.Domain.AppRatings;
using Explorer.Stakeholders.Core.Domain.Clubs;
using Explorer.Stakeholders.Core.Domain.Comments;
using Explorer.Stakeholders.Core.Domain.Diaries;
using Explorer.Stakeholders.Core.Domain.Notifications;
using Explorer.Stakeholders.Core.Domain.Positions;
using Explorer.Stakeholders.Core.Domain.TourProblems;
using Explorer.Stakeholders.Core.Domain.Users;
using Explorer.Stakeholders.Core.Domain.Users.Entities;
using System;
using System.Linq;

namespace Explorer.Stakeholders.Core.Mappers
{
    public class StakeholderProfile : Profile
    {
        public StakeholderProfile()
        {
            CreateMap<Person, ProfileDto>()
                .ForMember(dest => dest.Statistics, opt => opt.Ignore()); 

            CreateMap<ProfileDto, Person>()
                .ForMember(dest => dest.UserId, opt => opt.Ignore()); 

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
                    dest => dest.ImagePaths,
                    opt => opt.MapFrom(src => src.ImagePaths)
                );

            CreateMap<ClubDto, Club>()
                .ForMember(dest => dest.ImagePaths, opt => opt.Ignore()); 
                                                                       


            CreateMap<TourProblem, TourProblemDto>()
                .ForMember(dest => dest.Comments, opt => opt.Ignore())
                .ConstructUsing(src => new TourProblemDto
                {
                    Id = src.Id,
                    TourId = src.TourId,
                    ReporterId = src.ReporterId,
                    Category = (API.Dtos.Tours.Problems.ProblemCategory)src.Category,
                    Priority = (API.Dtos.Tours.Problems.ProblemPriority)src.Priority,
                    Description = src.Description,
                    OccurredAt = src.OccurredAt,
                    CreatedAt = src.CreatedAt,
                    IsResolved = src.IsResolved,
                    Deadline = src.Deadline,
                    Comments = new List<CommentDto>(),
                    TourName = "",
                    ReporterName = ""
                });


            CreateMap<TourProblemDto, TourProblem>();

            // ========================= Position <-> PositionDto =========================
            CreateMap<PositionDto, Position>().ReverseMap();

            CreateMap<Comment, CommentDto>();

            // ========================= Notification <-> NotificationDto =========================
            CreateMap<Notification, NotificationDto>()
                .ForMember(
                    dest => dest.Type,
                    opt => opt.MapFrom(src => src.Type.ToString())
                );

            CreateMap<NotificationDto, Notification>()
                .ConstructUsing(dto => new Notification(
                    dto.UserId,
                    Enum.Parse<NotificationType>(dto.Type, true),
                    dto.Title,
                    dto.Message,
                    dto.TourProblemId,
                    dto.TourId,
                    dto.ActionUrl,
                    dto.ClubId
                ));

            // ========================= Diary <-> DiaryDto =========================
            CreateMap<Diary, DiaryDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => (int)src.Status));

            CreateMap<DiaryDto, Diary>()
                .ConstructUsing(dto => new Diary(
                    dto.Name,
                    dto.CreatedAt,
                    (DiaryStatus)dto.Status,
                    dto.Country,
                    dto.City,
                    dto.TouristId
                ))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));

            CreateMap<ClubInvite, ClubInviteDto>()
                .ForMember(dest => dest.TouristUsername, opt => opt.Ignore());

        }
    }
}
