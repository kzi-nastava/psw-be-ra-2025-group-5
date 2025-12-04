using Explorer.Stakeholders.API.Public;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using AutoMapper;

namespace Explorer.Stakeholders.Core.UseCases
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IMapper _mapper;

        public CommentService(ICommentRepository repository, IMapper mapper)
        {
            _commentRepository = repository;
            _mapper = mapper;
        }

        public CommentDto Create(long authorId, CreateCommentDto dto)
        {
            var comment = new Comment(authorId, dto.Content);
            var result = _commentRepository.CreateComment(comment);
            return _mapper.Map<CommentDto>(result);
        }

        public CommentDto GetByCommentId(long id)
        {
            var comment = _commentRepository.GetByCommentId(id);
            if (comment == null)
                throw new KeyNotFoundException("Comment not found.");

            return _mapper.Map<CommentDto>(comment);
        }
    }
}
