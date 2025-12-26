using Explorer.Blog.API.Dtos.Posts;
using Explorer.Blog.API.Public;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Blog.Tests.Integration
{
    [Collection("Sequential")]
    public class ShowBlogTest : BaseBlogIntegrationTest
    {
        public ShowBlogTest(BlogTestFactory factory) : base(factory) { }
        [Fact]
        public void User_sees_only_his_own_drafts()
        {
            using var scope = Factory.Services.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IBlogService>();

            long user1 = 1;
            long user2 = 2;

            var draftUser1 = service.Create(new CreateBlogPostDto
            {
                Title = "Draft U1",
                Description = "desc"
            }, user1);

            var draftUser2 = service.Create(new CreateBlogPostDto
            {
                Title = "Draft U2",
                Description = "desc"
            }, user2);

            var visibleForUser1 = service.GetAll(user1);

            visibleForUser1.Any(p => p.Id == draftUser1.Id).ShouldBeTrue();
            visibleForUser1.Any(p => p.Id == draftUser2.Id).ShouldBeFalse();
        }
        [Fact]
        public void User_sees_published_posts_from_others()
        {
            using var scope = Factory.Services.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IBlogService>();

            long user1 = 1;
            long user2 = 2;

            var post = service.Create(new CreateBlogPostDto
            {
                Title = "Published Test",
                Description = "desc"
            }, user2);

            // publish
            service.Publish(post.Id, user2);

            var visibleForUser1 = service.GetAll(user1);

            visibleForUser1.Any(p => p.Id == post.Id).ShouldBeTrue();
        }


    }
}
