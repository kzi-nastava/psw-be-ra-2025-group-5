using Explorer.Blog.API.Dtos;
using Explorer.Blog.API.Public;
using Explorer.Blog.Core.Domain;
using Explorer.Blog.Infrastructure.Database;
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
    public class BlogUpdateTest : BaseBlogIntegrationTest
    {
        public BlogUpdateTest(BlogTestFactory factory) : base(factory) { }

        [Fact]
        public void Successfully_updates_draft_blog()
        {
            using var scope = Factory.Services.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IBlogService>();
            var dbContext = scope.ServiceProvider.GetRequiredService<BlogContext>();

            var createDto = new CreateBlogPostDto
            {
                Title = "Original Blog",
                Description = "Originalni opis"
            };
            var created = service.Create(createDto, authorId: 1);

            var draftDto = new UpdateDraftBlogPostDto
            {
                Title = "Novi Naslov",
                Description = "Novi opis"
            };

            var updated = service.UpdateDraft(created.Id, draftDto, authorId: 1);

            updated.Title.ShouldBe(draftDto.Title);
            updated.Description.ShouldBe(draftDto.Description);

            var stored = dbContext.BlogPosts.Find(created.Id);
            stored.Title.ShouldBe(draftDto.Title);
            stored.Description.ShouldBe(draftDto.Description);
        }
        [Fact]
        public void Successfully_archives_published_blog()
        {
            using var scope = Factory.Services.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IBlogService>();
            var dbContext = scope.ServiceProvider.GetRequiredService<BlogContext>();

            var createDto = new CreateBlogPostDto
            {
                Title = "Blog",
                Description = "Opis"
            };
            var created = service.Create(createDto, authorId: 1);

            service.Publish(created.Id, authorId: 1);

            var archived = service.Archive(created.Id, authorId: 1);

            archived.ShouldNotBeNull();
            archived.Status.ShouldBe(BlogPost.BlogStatus.Archived.ToString());

            var stored = dbContext.BlogPosts.Find(created.Id);
            stored.Status.ShouldBe(BlogPost.BlogStatus.Archived);
        }
        [Fact]
        public void Fails_to_modify_images_when_published()
        {
            using var scope = Factory.Services.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IBlogService>();
            var dbContext = scope.ServiceProvider.GetRequiredService<BlogContext>();

            var createDto = new CreateBlogPostDto
            {
                Title = "Blog sa slikama",
                Description = "Opis"
            };
            var created = service.Create(createDto, authorId: 1);

            var imageDto = new BlogImageDto
            {
                Base64 = Convert.ToBase64String(new byte[] { 1, 2, 3 }),
                ContentType = "image/png",
                Order = 0
            };
            var added = service.AddImage(created.Id, imageDto);

            service.Publish(created.Id, 1);

            Should.Throw<InvalidOperationException>(() =>
            {
                service.DeleteImage(added.Id);
            });

            Should.Throw<InvalidOperationException>(() =>
            {
                service.UpdateImage(new BlogImageDto
                {
                    Id = added.Id,
                    Base64 = added.Base64,
                    ContentType = "image/png",
                    Order = 0
                });
            });
        }
        [Fact]
        public void Cannot_publish_if_not_in_draft()
        {
            using var scope = Factory.Services.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IBlogService>();

            var created = service.Create(new CreateBlogPostDto
            {
                Title = "Blog",
                Description = "Desc"
            }, 1);

            service.Publish(created.Id, 1); // sada je Published

            Should.Throw<InvalidOperationException>(() =>
            {
                service.Publish(created.Id, 1);
            });
        }
        [Fact]
        public void Cannot_archive_if_not_published()
        {
            using var scope = Factory.Services.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IBlogService>();

            var created = service.Create(new CreateBlogPostDto
            {
                Title = "Blog",
                Description = "Desc"
            }, 1);

            Should.Throw<InvalidOperationException>(() =>
            {
                service.Archive(created.Id, 1);
            });
        }
        [Fact]
        public void Cannot_update_draft_after_publish()
        {
            using var scope = Factory.Services.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IBlogService>();

            var created = service.Create(new CreateBlogPostDto
            {
                Title = "Blog",
                Description = "Desc"
            }, 1);

            service.Publish(created.Id, 1);

            Should.Throw<InvalidOperationException>(() =>
            {
                service.UpdateDraft(created.Id, new UpdateDraftBlogPostDto
                {
                    Title = "Izmena",
                    Description = "Nova"
                }, 1);
            });
        }

        [Fact]
        public void Cannot_update_published_fields_while_in_draft()
        {
            using var scope = Factory.Services.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IBlogService>();

            var created = service.Create(new CreateBlogPostDto
            {
                Title = "Blog",
                Description = "Desc"
            }, 1);

            Should.Throw<InvalidOperationException>(() =>
            {
                service.Update(created.Id, new UpdateBlogPostDto
                {
                    Description = "Promena"
                }, 1);
            });

        }
    }
}
