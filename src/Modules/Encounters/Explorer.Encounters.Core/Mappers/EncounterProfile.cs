using AutoMapper;
using Explorer.Encounters.API.Dtos;
using Explorer.Encounters.Core.Domain;

namespace Explorer.Encounters.Core.Mappers
{
    public class EncounterProfile : Profile
    {
        public EncounterProfile()
        {
            CreateMap<ChallengeStatus, string>().ConvertUsing(src => src.ToString());
            CreateMap<string, ChallengeStatus>().ConvertUsing(src => Enum.Parse<ChallengeStatus>(src, true));

            CreateMap<ChallengeType, string>().ConvertUsing(src => src.ToString());
            CreateMap<string, ChallengeType>().ConvertUsing(src => Enum.Parse<ChallengeType>(src, true));

            CreateMap<ChallengeExecutionStatus, string>().ConvertUsing(src => src.ToString());
            CreateMap<string, ChallengeExecutionStatus>().ConvertUsing(src => Enum.Parse<ChallengeExecutionStatus>(src, true));

            CreateMap<ChallengeDto, Challenge>()
                .ConstructUsing(dto => new Challenge(
                    dto.Name,
                    dto.Description,
                    dto.Latitude,
                    dto.Longitude,
                    dto.ExperiencePoints,
                    Enum.Parse<ChallengeStatus>(dto.Status, true),
                    Enum.Parse<ChallengeType>(dto.Type, true),
                    dto.RequiredParticipants, dto.RadiusInMeters)
                )
                .AfterMap((dto, challenge) =>
                {
                    if (dto.Id != 0)
                    {
                        typeof(Challenge).GetProperty("Id")!.SetValue(challenge, dto.Id);
                    }
                });

            CreateMap<Challenge, ChallengeDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()));

            CreateMap<ChallengeExecution, ChallengeExecutionDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            CreateMap<ChallengeExecutionDto, ChallengeExecution>()
                .ConstructUsing(dto => new ChallengeExecution(dto.ChallengeId, dto.TouristId))
                .AfterMap((dto, execution) =>
                {
                    if (dto.Id != 0)
                    {
                        typeof(ChallengeExecution).GetProperty("Id")!.SetValue(execution, dto.Id);
                    }
                });
        }
    }
}
