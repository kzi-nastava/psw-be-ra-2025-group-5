using Explorer.Blog.API.Dtos;
using Explorer.Blog.API.Public;
using Explorer.Blog.Core.Domain;
using Explorer.Blog.Core.UseCases;
using Explorer.Blog.Infrastructure.Database;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
namespace Explorer.Blog.Tests.Integration
{
    [Collection("Sequential")]
    public class BlogCommandTest: BaseBlogIntegrationTest
    {
        public BlogCommandTest(BlogTestFactory factory) : base(factory) { }

        [Fact]
        public void Successfully_creates_blog_without_images()
        {
            using var scope = Factory.Services.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IBlogService>();
            var dbContext = scope.ServiceProvider.GetRequiredService<BlogContext>();

            var dto = new CreateBlogPostDto
            {
                Title = "Test Blog",
                Description = "Opis test bloga"
            };

            var created = service.Create(dto, authorId: 1);

            created.ShouldNotBeNull();
            created.Id.ShouldNotBe(0);
            created.Title.ShouldBe(dto.Title);
            created.Description.ShouldBe(dto.Description);

            var stored = dbContext.BlogPosts.Find(created.Id);
            stored.ShouldNotBeNull();
            stored.Images.ShouldBeEmpty();
        }

        [Fact]
        public void Fails_when_missing_title()
        {
            using var scope = Factory.Services.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IBlogService>();

            var dto = new CreateBlogPostDto
            {
                Title = "",
                Description = "Opis"
            };

            Should.Throw<ArgumentException>(() => service.Create(dto, authorId: 1));
        }

        

        [Fact]
        public void Successfully_adds_images_to_blog()
        {
            using var scope = Factory.Services.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IBlogService>();
            var dbContext = scope.ServiceProvider.GetRequiredService<BlogContext>();

            var blogDto = new CreateBlogPostDto
            {
                Title = "Blog sa slikama",
                Description = "Opis bloga"
            };
            var created = service.Create(blogDto, authorId: 1);

            var image1 = new BlogImageDto
            {
                Base64 = Convert.ToBase64String(new byte[] { 1, 2, 3 }),
                ContentType = "image/png",
                Order = 0
            };
            var image2 = new BlogImageDto
            {
                Base64 = Convert.ToBase64String(new byte[] { 4, 5, 6 }),
                ContentType = "image/jpeg",
                Order = 1
            };

            var added1 = service.AddImage(created.Id, image1);
            var added2 = service.AddImage(created.Id, image2);

            var stored = dbContext.BlogPosts.Find(created.Id);
            stored.Images.Count.ShouldBe(2);
        }

        [Fact]
        public void Deletes_blog_image_successfully()
        {
            using var scope = Factory.Services.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IBlogService>();
            var dbContext = scope.ServiceProvider.GetRequiredService<BlogContext>();

            var blogDto = new CreateBlogPostDto
            {
                Title = "Blog sa slikom za brisanje",
                Description = "Opis"
            };
            var created = service.Create(blogDto, authorId: 1);

            var imageDto = new BlogImageDto
            {
                Base64 = Convert.ToBase64String(new byte[] { 1, 2, 3 }),
                ContentType = "image/png",
                Order = 0
            };
            var addedImage = service.AddImage(created.Id, imageDto);

            var result = service.DeleteImage(addedImage.Id);

            result.ShouldBeTrue();

            var storedBlog = dbContext.BlogPosts.Find(created.Id);
            storedBlog.Images.ShouldBeEmpty();
        }
        [Fact]
        public void Successfully_creates_blog_in_draft_state()
        {
            using var scope = Factory.Services.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IBlogService>();
            var dbContext = scope.ServiceProvider.GetRequiredService<BlogContext>();

            var createDto = new CreateBlogPostDto
            {
                Title = "Test title",
                Description = "Test description"
            };

            var created = service.Create(createDto, authorId: 1);

            created.ShouldNotBeNull();
            created.Status.ShouldBe(BlogPost.BlogStatus.Draft.ToString());

            var stored = dbContext.BlogPosts.Find(created.Id);
            stored.ShouldNotBeNull();
            stored.Status.ShouldBe(BlogPost.BlogStatus.Draft);
        }

        [Fact]
        public void Successfully_publishes_blog()
        {
            using var scope = Factory.Services.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IBlogService>();
            var dbContext = scope.ServiceProvider.GetRequiredService<BlogContext>();

            var createDto = new CreateBlogPostDto
            {
                Title = "Draft Blog",
                Description = "Opis"
            };
            var created = service.Create(createDto, authorId: 1);

            var published = service.Publish(created.Id, authorId: 1);

            published.ShouldNotBeNull();
            published.Status.ShouldBe(BlogPost.BlogStatus.Published.ToString());

            var stored = dbContext.BlogPosts.Find(created.Id);
            stored.Status.ShouldBe(BlogPost.BlogStatus.Published);
        }

        [Fact]
        public void Blog_becomes_closed_when_score_drops_below_minus_10()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var blogService = scope.ServiceProvider.GetRequiredService<IBlogService>();
            var dbContext = scope.ServiceProvider.GetRequiredService<BlogContext>();

            var dto = new CreateBlogPostDto 
            { 
                Title = "Blog", 
                Description = "Description" 
            };
            var blog = blogService.Create(dto, 1);
            blogService.Publish(blog.Id, 1);

            // Act
            for (int i = 1; i < 12; i++)
            {
                blogService.Vote(blog.Id, i, "Downvote");
            }

            // Assert
            var stored = dbContext.BlogPosts.Find(blog.Id);
            stored.ShouldNotBeNull();
            stored.Status.ShouldBe(BlogPost.BlogStatus.ReadOnly);
        }

        [Fact]
        public void Blog_becomes_active_when_score_is_high()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var blogService = scope.ServiceProvider.GetRequiredService<IBlogService>();
            var dbContext = scope.ServiceProvider.GetRequiredService<BlogContext>();

            var dto = new CreateBlogPostDto 
            { 
                Title = "Blog", 
                Description = "Description" 
            };
            var blog = blogService.Create(dto, 1);
            blogService.Publish(blog.Id, 1);

            // Act 
            for (int i = 1; i < 102; i++)
            {
                blogService.Vote(blog.Id, i, "Upvote");
            }

            // Assert
            var stored = dbContext.BlogPosts.Find(blog.Id);
            stored.Status.ShouldBe(BlogPost.BlogStatus.Active);
        }

        [Fact]
        public void Blog_becomes_active_with_many_comments()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var blogService = scope.ServiceProvider.GetRequiredService<IBlogService>();

            var commentService = scope.ServiceProvider.GetRequiredService<BlogCommentService>();
            var dbContext = scope.ServiceProvider.GetRequiredService<BlogContext>();

            var dto = new CreateBlogPostDto
            {
                Title = "Blog",
                Description = "Description"
            };
            var blog = blogService.Create(dto, 1);
            blogService.Publish(blog.Id, 1);

            // Act
            for (int i = 0; i < 11; i++)
            {
                commentService.AddComment(blog.Id, 2, "Komentar");
            }

            // Assert
            var stored = dbContext.BlogPosts.Find(blog.Id);
            stored.Status.ShouldBe(BlogPost.BlogStatus.Active);
        }

        [Fact]
        public void Blog_becomes_famous_with_high_score_and_comments()
        {
            // Arrange
            using var scope = Factory.Services.CreateScope();
            var blogService = scope.ServiceProvider.GetRequiredService<IBlogService>();

            var commentService = scope.ServiceProvider.GetRequiredService<BlogCommentService>();
            var dbContext = scope.ServiceProvider.GetRequiredService<BlogContext>();

            var dto = new CreateBlogPostDto
            {
                Title = "Blog",
                Description = "Description"
            };
            var blog = blogService.Create(dto, 1);
            blogService.Publish(blog.Id, 1);

            // Act
            for (int i = 0; i < 501; i++) blogService.Vote(blog.Id, 1000 + i, "Upvote");

            for (int i = 0; i < 31; i++) commentService.AddComment(blog.Id, 2, "Komentar ");

            // Assert
            var stored = dbContext.BlogPosts.Find(blog.Id);
            stored.Status.ShouldBe(BlogPost.BlogStatus.Famous);
        }

        [Fact]
        public void Successfully_creates_and_publishes_blog_with_images()
        {
            using var scope = Factory.Services.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IBlogService>();
            var dbContext = scope.ServiceProvider.GetRequiredService<BlogContext>();

            // Arrange
            var dto = new CreateAndPublishBlogPostDto
            {
                Title = "Test Create & Publish",
                Description = "Opis",
                Images = new List<BlogImageDto>
        {
            new BlogImageDto
            {
                Base64 = Convert.ToBase64String(new byte[] { 1, 2, 3 }),
                ContentType = "image/png",
                Order = 0
            },
            new BlogImageDto
            {
                Base64 = Convert.ToBase64String(new byte[] { 4, 5, 6 }),
                ContentType = "image/jpeg",
                Order = 1
            }
        }
            };

            // Act
            var result = service.CreateAndPublish(dto, authorId: 1);

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldBeGreaterThan(0);
            result.Title.ShouldBe(dto.Title);
            result.Description.ShouldBe(dto.Description);
            result.Status.ShouldBe(BlogPost.BlogStatus.Published.ToString());

            result.Images.Count.ShouldBe(2);

        }


    }

}

