using AutoMapper;
using Explorer.Blog.API.Dtos.Comments;
using Explorer.Blog.API.Dtos.Images;
using Explorer.Blog.API.Dtos.Posts;
using Explorer.Blog.API.Dtos.Votes;
using Explorer.Blog.Core.Domain.BlogPosts;
using Explorer.Blog.Core.Domain.BlogPosts.Entities;

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

            CreateMap<BlogImage, BlogImageDto>()
               .ForMember(dest => dest.Base64,
                   opt => opt.MapFrom(src => Convert.ToBase64String(src.Data)));

            CreateMap<BlogImageDto, BlogImage>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            // Mapiranje komentara (feat/blog-comments)
            CreateMap<Comment, CommentBlogDto>();

            CreateMap<BlogVote, BlogVoteDto>().ReverseMap();
        }
    }
}
