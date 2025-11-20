using AutoMapper;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.Core.Domain;

namespace Explorer.Tours.Core.Mappers;

public class ToursProfile : Profile
{
    public ToursProfile()
    {
        CreateMap<EquipmentDto, Equipment>().ReverseMap();
        CreateMap<MonumentLocationDto, MonumentLocation>().ReverseMap();
        CreateMap<MonumentDto, Monument>().ReverseMap();

        CreateMap<TourProblemDto, TourProblem>().ReverseMap();
    }
}