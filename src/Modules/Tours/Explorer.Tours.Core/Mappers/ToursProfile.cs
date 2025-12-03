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

        CreateMap<Location, LocationDto>().ReverseMap();

        CreateMap<KeyPoint, KeyPointDto>().ReverseMap();
        
        CreateMap<CreateKeyPointDto, KeyPointDto>();

        CreateMap<TourProgress, double>().ConvertUsing(tp => tp.Percentage);
        CreateMap<double, TourProgress>().ConvertUsing(d => new TourProgress(d));

        CreateMap<ReviewImage, ReviewImageDto>().ReverseMap();

        CreateMap<TourReview, TourReviewDto>()
            .ForMember(d => d.Progress, opt => opt.MapFrom(s => s.Progress.Percentage))
            .ReverseMap()
            .ForMember(d => d.Progress, opt => opt.MapFrom(src => new TourProgress(src.Progress)))
            .ForMember(d => d.Images, opt => opt.MapFrom(src =>
                src.Images != null ? src.Images : new List<ReviewImageDto>()));
    }
}