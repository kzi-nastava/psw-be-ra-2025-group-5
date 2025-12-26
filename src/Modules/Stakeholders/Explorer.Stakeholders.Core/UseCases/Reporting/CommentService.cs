using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.Stakeholders.Core.Domain.Comments;
using AutoMapper;
using Explorer.Stakeholders.API.Dtos.Comments;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces.TourProblems;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces.Users;
using Explorer.Stakeholders.API.Public.Reporting;

namespace Explorer.Stakeholders.Core.UseCases.Reporting
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public CommentService(ICommentRepository repository,
    IUserRepository userRepository, IMapper mapper)
        {
            _commentRepository = repository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public CommentDto Create(long authorId, CreateCommentDto dto)
        {
            var comment = new Comment(authorId, dto.Content);
            var result = _commentRepository.CreateComment(comment);

            var dtoResult = _mapper.Map<CommentDto>(result);

            var user = _userRepository.GetById(authorId);
            dtoResult.AuthorRole = user.Role.ToString();

            return dtoResult;
        }


        public CommentDto GetByCommentId(long id)
        {
            var comment = _commentRepository.GetByCommentId(id);
            if (comment == null)
                throw new KeyNotFoundException("Comment not found.");

            var dto = _mapper.Map<CommentDto>(comment);

            var user = _userRepository.GetById(dto.AuthorId);
            dto.AuthorRole = user.Role.ToString();

            return dto;
        }

    }
}
