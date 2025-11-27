using Explorer.API.Controllers.Blog;
using Explorer.Blog.API.Dtos;
using Explorer.Blog.API.Public;
using Explorer.Blog.Core.Domain;
using Explorer.Blog.Core.Domain.RepositoryInterfaces;
using Explorer.Blog.Infrastructure.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Xunit;

namespace Explorer.Blog.Tests.Integration
{
    [Collection("Sequential")]
    public class BlogQueryTest : BaseBlogIntegrationTest
    {
        public BlogQueryTest(BlogTestFactory factory) : base(factory) { }

        [Fact]
        public void Retrieves_all_blog_posts()
        {
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);

            var actionResult = controller.GetAll();
            var result = GetValue<List<BlogPostDto>>(actionResult); 

            result.ShouldNotBeNull();
            result.Count.ShouldBeGreaterThan(0);
        }

        [Fact]
        public void Retrieves_blog_post_by_id()
        {
            using var scope = Factory.Services.CreateScope();
            var serviceProvider = scope.ServiceProvider;
            var context = serviceProvider.GetRequiredService<BlogContext>();

            var testPost = new BlogPost(1, "Prvi test blog", "Opis testa", DateTime.UtcNow);
            context.BlogPosts.Add(testPost);
            context.SaveChanges();

            var blogService = serviceProvider.GetRequiredService<IBlogService>();

            var controller = new BlogController(blogService)
            {
                ControllerContext = BuildContext("1") 
            };

            // Act
            var actionResult = controller.GetPost((int)testPost.Id);
            var result = GetValue<BlogPostDto>(actionResult);

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldBe(testPost.Id);
            result.Title.ShouldBe(testPost.Title);
        }




        [Fact]
        public void Retrieves_blog_posts_by_author()
        {
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);

            var actionResult = controller.GetByAuthor(1);
            var result = GetValue<List<BlogPostDto>>(actionResult);

            result.ShouldNotBeNull();
            result.All(p => p.AuthorId == 1).ShouldBeTrue();
        }

        private T GetValue<T>(IActionResult actionResult)
        {
            switch (actionResult)
            {
                case OkObjectResult okResult:
                    return (T)okResult.Value!;
                case NotFoundResult:
                case NotFoundObjectResult:
                    throw new Exception("Resource not found");
                default:
                    throw new Exception($"Unexpected result type: {actionResult.GetType().Name}");
            }
        }

        private static ControllerContext BuildContext(string userId)
        {
            var claims = new List<Claim>
            {
                new Claim("id", userId)
            };

            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var user = new ClaimsPrincipal(identity);

            return new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
        }



        private static BlogController CreateController(IServiceScope scope)
        {
            var serviceProvider = scope.ServiceProvider;
            var blogService = serviceProvider.GetRequiredService<IBlogService>();

            return new BlogController(blogService)
            {
                ControllerContext = BuildContext("1") 
            };
        }
    }
}
