using Explorer.Blog.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using System;
using System.Collections.Generic;
using Explorer.Blog.API.Dtos;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;

namespace Explorer.API.Controllers
{
    [ApiController]
    [Route("api/blogs/{blogId:long}/comments")]
    [Authorize(Roles = "author,tourist")]
    public class BlogCommentsController : ControllerBase
    {
        private readonly BlogCommentService _commentService;
        private readonly IMapper _mapper;
        private readonly IPersonRepository _personRepository;

        public BlogCommentsController(
         BlogCommentService commentService,
         IPersonRepository personRepository, 
         IMapper mapper)
        {
            _commentService = commentService;
            _personRepository = personRepository;  
            _mapper = mapper;
        }

        // GET: api/blogs/{blogId}/comments
        [HttpGet]
        public ActionResult<List<CommentBlogDto>> GetAll(long blogId)
        {
            try
            {
                var comments = _commentService.GetAll(blogId);
                var dtos = _mapper.Map<List<CommentBlogDto>>(comments);

                foreach (var dto in dtos)
                {
                    var person = _personRepository.Get(dto.AuthorId);
                    if (person != null)
                    {
                        dto.AuthorName = $"{person.Name} {person.Surname}";
                    }
                }

                return Ok(dtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // POST: api/blogs/{blogId}/comments
        [HttpPost]
        public IActionResult Add(long blogId, CreateCommentDto dto)
        {
            var userId = User.PersonId();
            if (userId == 0)
                return BadRequest("personId claim missing in token");

            try
            {
                _commentService.AddComment(blogId, userId, dto.Content);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // PUT: api/blogs/{blogId}/comments/{commentId}
        [HttpPut("{commentId:long}")]
        public IActionResult Update(long blogId, long commentId, UpdateCommentDto dto)
        {
            var userId = User.PersonId();

            try
            {
                _commentService.UpdateComment(blogId, commentId, userId, dto.Content);
                return Ok();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE: api/blogs/{blogId}/comments/{commentId}
        [HttpDelete("{commentId:long}")]
        public IActionResult Delete(long blogId, long commentId)
        {
            var userId = User.PersonId();

            try
            {
                _commentService.DeleteComment(blogId, commentId, userId);
                return Ok();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
