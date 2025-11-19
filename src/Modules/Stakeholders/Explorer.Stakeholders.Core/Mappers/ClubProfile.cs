using AutoMapper;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Core.Mappers
{
    public class ClubProfile : Profile
    {
        public ClubProfile()
        {
            //CreateMap<ClubDto, Club>().ReverseMap();

            // Club -> ClubDto (byte[] -> Base64 string za transport)
            CreateMap<Club, ClubDto>()
                .ForMember(dest => dest.Images,
                    opt => opt.MapFrom(src => src.Images.Select(img => Convert.ToBase64String(img)).ToList()));

            // ClubDto -> Club (Base64 string -> byte[])
            CreateMap<ClubDto, Club>()
                .ForMember(dest => dest.Images,
                    opt => opt.MapFrom(src => src.Images.Select(img => Convert.FromBase64String(img)).ToList()));
        }

    }


}
