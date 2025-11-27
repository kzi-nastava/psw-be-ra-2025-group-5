using Explorer.Blog.API.Dtos;
using Explorer.Blog.API.Public;
using Explorer.Blog.Core.Domain;
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
        public void Successfully_updates_blog()
        {
            using var scope = Factory.Services.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IBlogService>();
            var dbContext = scope.ServiceProvider.GetRequiredService<BlogContext>();

            var originalDto = new CreateBlogPostDto
            {
                Title = "Original Blog",
                Description = "Opis originalnog bloga"
            };
            var created = service.Create(originalDto, authorId: 1);

            var updateDto = new UpdateBlogPostDto
            {
                Title = "Updated Blog",
                Description = "Opis izmenjenog bloga"
            };

            var updated = service.Update(created.Id, updateDto, authorId: 1);

            updated.Title.ShouldBe("Updated Blog");
            updated.Description.ShouldBe("Opis izmenjenog bloga");

            var stored = dbContext.BlogPosts.Find(created.Id);
            stored.Title.ShouldBe("Updated Blog");
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

    }
}

