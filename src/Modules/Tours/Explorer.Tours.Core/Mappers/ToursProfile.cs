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

        CreateMap<ReviewImage, ReviewImageDto>().ReverseMap();

        CreateMap<TourReview, TourReviewDto>()
            .ForMember(d => d.Progress, opt => opt.MapFrom(s => s.Progress.Percentage))
            .ForMember(d => d.Images, opt => opt.MapFrom(s => s.Images))
            .ReverseMap()
            .ForMember(d => d.Progress, opt => opt.MapFrom(src => new TourProgress(src.Progress)));

        CreateMap<TourPurchaseTokenDto, TourPurchaseToken>().ReverseMap();
        CreateMap<CreateTourPurchaseTokenDto, TourPurchaseToken>();
    }
}