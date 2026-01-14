using AutoMapper;
using TourDifficulty = Explorer.Tours.Core.Domain.Tours.TourDifficulty;
using Explorer.Tours.Core.Domain.Tours;
using Explorer.Tours.Core.Domain.Tours.Entities;
using Explorer.Tours.Core.Domain.Tours.ValueObjects;
using Explorer.Tours.Core.Domain.TourExecutions;
using Explorer.Tours.Core.Domain.TourExecutions.ValueObejcts;
using Explorer.Tours.Core.Domain.Equipments;
using Explorer.Tours.Core.Domain.Equipments.Entities;
using Explorer.Tours.Core.Domain.Monuments;
using Explorer.Tours.Core.Domain.Monuments.ValueObjects;
using Explorer.Tours.Core.Domain.Facilities;
using Explorer.Tours.Core.Domain.Preferences;
using Explorer.Tours.API.Dtos.Monuments;
using Explorer.Tours.API.Dtos.Equipments;
using Explorer.Tours.API.Dtos.Facilities;
using Explorer.Tours.API.Dtos.Tours.Executions;
using Explorer.Tours.API.Dtos.Tours;
using Explorer.Tours.API.Dtos.KeyPoints;
using Explorer.Tours.API.Dtos.Preferences;
using Explorer.Tours.API.Dtos.Tours.Reviews;
using Explorer.Tours.API.Dtos.Locations;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.Core.Domain;

namespace Explorer.Tours.Core.Mappers;

public class ToursProfile : Profile
{
    public ToursProfile()
    {
        CreateMap<EquipmentDto, Equipment>().ReverseMap();

        CreateMap<TouristPreferencesDto, TouristPreferences>().ReverseMap();

        CreateMap<TourDifficulty, string>().ConvertUsing(src => src.ToString());
        CreateMap<string, TourDifficulty>().ConvertUsing(src => Enum.Parse<TourDifficulty>(src, true));

        CreateMap<TourStatus, string>().ConvertUsing(src => src.ToString());
        CreateMap<string, TourStatus>().ConvertUsing(static src => Enum.Parse<TourStatus>(src, true));

        CreateMap<Tour, TourDto>()
        .ForMember(d => d.RequiredEquipmentIds,
            opt => opt.MapFrom(s => s.RequiredEquipment.Select(re => re.EquipmentId)))
        .ReverseMap()
        .ForMember(d => d.RequiredEquipment, opt => opt.Ignore());

        CreateMap<CreateTourDto, Tour>()
            .ConstructUsing(dto => new Tour(
                dto.AuthorId,
                dto.Name,
                dto.Description,
                Enum.Parse<TourDifficulty>(dto.Difficulty, true),
                dto.Tags,
                dto.Price
            ));

        CreateMap<MonumentLocationDto, MonumentLocation>().ReverseMap();
        CreateMap<MonumentDto, Monument>().ReverseMap();

        CreateMap<TouristEquipmentDto, TouristEquipment>().ReverseMap();
        CreateMap<FacilityDto, Facility>().ReverseMap();

        CreateMap<Location, LocationDto>().ReverseMap();

        CreateMap<KeyPoint, KeyPointDto>();

        CreateMap<KeyPointCompletion, KeyPointCompletionDto>().ReverseMap();

        CreateMap<TourExecution, TourExecutionDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

        CreateMap<TourExecution, StartExecutionResultDto>()
            .ForMember(dest => dest.ExecutionId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.StartTime));

        CreateMap<CheckProximityResult, CheckProximityDto>(); 

        CreateMap<TourProgress, double>().ConvertUsing(tp => tp.Percentage);
        CreateMap<double, TourProgress>().ConvertUsing(d => new TourProgress(d));

        CreateMap<ReviewImage, ReviewImageDto>()
            .ForMember(d => d.Data, opt => opt.MapFrom(s => Convert.ToBase64String(s.Data)))
            .ForMember(d => d.ContentType, opt => opt.MapFrom(s => s.ContentType))
            .ForMember(d => d.Order, opt => opt.MapFrom(s => s.Order));

        CreateMap<ReviewImageDto, ReviewImage>()
            .ConstructUsing(dto => new ReviewImage(0, Convert.FromBase64String(dto.Data), dto.ContentType, dto.Order));

        CreateMap<TourReview, TourReviewDto>()
            .ForMember(d => d.Progress, opt => opt.MapFrom(s => s.Progress.Percentage))
            .ForMember(d => d.Images, opt => opt.MapFrom(s => s.Images))
            .ReverseMap()
            .ForMember(d => d.Progress, opt => opt.MapFrom(src => new TourProgress(src.Progress)));

        CreateMap<TourDuration, TourDurationDto>()
            .ForMember(dest => dest.TransportType, opt => opt.MapFrom(src => src.TransportType.ToString()));

        CreateMap<TourDurationDto, TourDuration>()
            .ForMember(dest => dest.TransportType, opt => opt.MapFrom(src => Enum.Parse<TourDuration.TransportTypeEnum>(src.TransportType)));

        CreateMap<TourReviewDto, TourReview>()
            .ForMember(d => d.Progress,opt => opt.MapFrom(src => new TourProgress(src.Progress)))
            .ForMember(d => d.Images,opt => opt.Ignore());

        CreateMap<TourStatisticsItem, TourStatisticsItemDto>().ReverseMap();

        CreateMap<TourSearchHistory, TourSearchHistoryDto>().ReverseMap();

        CreateMap<Tour, TourDto>()
            .ForMember(d => d.RequiredEquipmentIds,
                opt => opt.MapFrom(s => s.RequiredEquipment.Select(re => re.EquipmentId)))
            .ForMember(d => d.ThumbnailPath,
                opt => opt.MapFrom(s => s.ThumbnailPath))
            .ReverseMap()
            .ForMember(d => d.RequiredEquipment, opt => opt.Ignore())
            .ForMember(d => d.ThumbnailPath, opt => opt.Ignore());
    }
}