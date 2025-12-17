using AutoMapper;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.Core.Domain;
using TourDifficulty = Explorer.Tours.Core.Domain.TourDifficulty;

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

        CreateMap<Tour, TourDto>().ReverseMap();

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

        CreateMap<OrderItem, OrderItemDto>().ReverseMap();

        CreateMap<ShoppingCartDto, ShoppingCart>().ReverseMap();

        CreateMap<CreateShoppingCartDto, ShoppingCart>()
            .ConstructUsing(src => new ShoppingCart(src.TouristId));

        CreateMap<Location, LocationDto>().ReverseMap();

        CreateMap<KeyPoint, KeyPointDto>().ReverseMap();
        
        CreateMap<CreateKeyPointDto, KeyPointDto>();

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
            .ForMember(d => d.Images, opt => opt.MapFrom(s => s.Images));
            .ReverseMap()
            .ForMember(d => d.Progress, opt => opt.MapFrom(src => new TourProgress(src.Progress)));

        CreateMap<TourDuration, TourDurationDto>()
            .ForMember(dest => dest.TransportType, opt => opt.MapFrom(src => src.TransportType.ToString()));

        CreateMap<TourDurationDto, TourDuration>()
            .ForMember(dest => dest.TransportType, opt => opt.MapFrom(src => Enum.Parse<TransportType>(src.TransportType)));

            

        CreateMap<TourReviewDto, TourReview>()
            .ForMember(d => d.Progress,opt => opt.MapFrom(src => new TourProgress(src.Progress)))
            .ForMember(d => d.Images,opt => opt.Ignore());
            

        CreateMap<TourPurchaseTokenDto, TourPurchaseToken>().ReverseMap();
        CreateMap<CreateTourPurchaseTokenDto, TourPurchaseToken>();
    }
}