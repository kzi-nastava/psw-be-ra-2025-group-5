using AutoMapper;
using Explorer.Blog.API.Dtos;
using Explorer.Blog.Core.Domain;

namespace Explorer.Blog.Core.Mappers
{
    public class BlogProfile : Profile
    {
        public BlogProfile()
        {
            // Mapiranje BlogPost sa posebnom logikom za Status
            CreateMap<BlogPost, BlogPostDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.VoteScore, opt => opt.MapFrom(src => src.GetScore()))
                .ReverseMap();

            CreateMap<BlogImage, BlogImageDto>().ForMember(d => d.Url, o => o.MapFrom(s => s.ImagePath));


            // Mapiranje komentara (feat/blog-comments)
            CreateMap<Comment, CommentBlogDto>();

            CreateMap<BlogVote, BlogVoteDto>().ReverseMap();
        }
    }
}
