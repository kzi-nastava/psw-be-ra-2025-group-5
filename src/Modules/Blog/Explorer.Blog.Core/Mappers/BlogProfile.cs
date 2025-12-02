using AutoMapper;
using Explorer.Blog.API.Dtos;
using Explorer.Blog.Core.Domain;

namespace Explorer.Blog.Core.Mappers;

public class BlogProfile : Profile
{
    public BlogProfile()
    {
        CreateMap<BlogPost, BlogPostDto>().ReverseMap();

        CreateMap<BlogImage, BlogImageDto>()
           .ForMember(dest => dest.Base64,
               opt => opt.MapFrom(src => Convert.ToBase64String(src.Data)));

        CreateMap<BlogImageDto, BlogImage>().ForMember(dest => dest.Id, opt => opt.Ignore());
        CreateMap<BlogPost, BlogPostDto>().ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

    }
}