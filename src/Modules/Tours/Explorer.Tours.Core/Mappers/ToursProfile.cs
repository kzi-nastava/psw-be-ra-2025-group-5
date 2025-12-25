using AutoMapper;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.Core.Domain;
using System.Linq;
using TourDifficulty = Explorer.Tours.Core.Domain.TourDifficulty;

namespace Explorer.Tours.Core.Mappers;

public class ToursProfile : Profile
{
    public ToursProfile()
    {
        CreateMap<EquipmentDto, Equipment>().ReverseMap();

        CreateMap<TouristPreferencesDto, TouristPreferences>().ReverseMap();

        CreateMap<Domain.TourDifficulty, string>().ConvertUsing(src => src.ToString());
        CreateMap<string, Domain.TourDifficulty>().ConvertUsing(src => Enum.Parse<Domain.TourDifficulty>(src, true));

        CreateMap<TourStatus, string>().ConvertUsing(src => src.ToString());
        CreateMap<string, TourStatus>().ConvertUsing(static src => Enum.Parse<TourStatus>(src, true));

        CreateMap<Tour, TourDto>()
            .ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status.ToString()))
            .ForMember(d => d.Difficulty, opt => opt.MapFrom(s => s.Difficulty.ToString()))
            .ForMember(d => d.ThumbnailUrl, opt => opt.MapFrom(s => s.ThumbnailPath))
            .ForMember(d => d.ThumbnailContentType, opt => opt.MapFrom(s => s.ThumbnailContentType))
            .ForMember(d => d.RequiredEquipmentIds,
                opt => opt.MapFrom(s => s.RequiredEquipment.Select(re => re.EquipmentId).ToList()));

        CreateMap<CreateTourDto, Tour>()
            .ForMember(d => d.Id, opt => opt.Ignore())
            .ForMember(d => d.Status, opt => opt.Ignore())
            .ForMember(d => d.PublishedDate, opt => opt.Ignore())
            .ForMember(d => d.ArchivedDate, opt => opt.Ignore())
            .ForMember(d => d.KeyPoints, opt => opt.Ignore())
            .ForMember(d => d.Reviews, opt => opt.Ignore())
            .ForMember(d => d.AverageRating, opt => opt.Ignore())
            .ForMember(d => d.Durations, opt => opt.Ignore())
            .ForMember(d => d.RequiredEquipment, opt => opt.Ignore())
            .ForMember(d => d.TourLength, opt => opt.Ignore())
            .ForMember(d => d.ThumbnailPath, opt => opt.Ignore())
            .ForMember(d => d.ThumbnailContentType, opt => opt.Ignore());

        CreateMap<UpdateTourDto, Tour>()
            .ForMember(d => d.Id, opt => opt.Ignore())
            .ForMember(d => d.AuthorId, opt => opt.Ignore())
            .ForMember(d => d.Status, opt => opt.Ignore())
            .ForMember(d => d.PublishedDate, opt => opt.Ignore())
            .ForMember(d => d.ArchivedDate, opt => opt.Ignore())
            .ForMember(d => d.KeyPoints, opt => opt.Ignore())
            .ForMember(d => d.Reviews, opt => opt.Ignore())
            .ForMember(d => d.AverageRating, opt => opt.Ignore())
            .ForMember(d => d.RequiredEquipment, opt => opt.Ignore())
            .ForMember(d => d.TourLength, opt => opt.Ignore())
            .ForMember(d => d.ThumbnailPath, opt => opt.Ignore())
            .ForMember(d => d.ThumbnailContentType, opt => opt.Ignore());

        CreateMap<MonumentLocationDto, MonumentLocation>().ReverseMap();
        CreateMap<MonumentDto, Monument>().ReverseMap();

        CreateMap<TouristEquipmentDto, TouristEquipment>().ReverseMap();
        CreateMap<FacilityDto, Facility>().ReverseMap();

        CreateMap<Location, LocationDto>().ReverseMap();

        CreateMap<KeyPoint, KeyPointDto>().ReverseMap();
        
        CreateMap<CreateKeyPointDto, KeyPointDto>();

        CreateMap<KeyPointCompletion, KeyPointCompletionDto>().ReverseMap();

        CreateMap<TourExecution, TourExecutionDto>();

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
            .ForMember(dest => dest.TransportType, opt => opt.MapFrom(src => Enum.Parse<TransportType>(src.TransportType)));

        CreateMap<TourReviewDto, TourReview>()
            .ForMember(d => d.Progress, opt => opt.MapFrom(src => new TourProgress(src.Progress)))
            .ForMember(d => d.Images, opt => opt.Ignore());

        CreateMap<TourStatisticsItem, TourStatisticsItemDto>().ReverseMap();
    }
}