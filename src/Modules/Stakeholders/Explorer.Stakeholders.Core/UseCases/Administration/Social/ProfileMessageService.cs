using AutoMapper;
using Explorer.BuildingBlocks.Core.Exceptions;
using Explorer.Stakeholders.API.Dtos.ClubMessages;
using Explorer.Stakeholders.API.Dtos.ProfileMessages;
using Explorer.Stakeholders.API.Dtos.Users;
using Explorer.Stakeholders.API.Public.ProfileMessages;
using Explorer.Stakeholders.Core.Domain.ClubMessages;
using Explorer.Stakeholders.Core.Domain.ProfileMessages;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces.ProfileMessages;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Core.UseCases.Administration.Social
{
    public class ProfileMessageService : IProfileMessageService
    {
        private readonly IProfileMessageRepository _profileMessageRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public ProfileMessageService(IProfileMessageRepository profileMessageRepository, IUserRepository userRepository, IMapper mapper)
        {
            _profileMessageRepository = profileMessageRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public ProfileMessageDto Create(long receiverId, long authorId, CreateMessageDto dto)
        {
            var message = new ProfileMessage(          
                authorId,
                receiverId,
                dto.Content,
                (ProfileMessage.ResourceType)dto.AttachedResourceType,
                dto.AttachedResourceId
            );

            var createdMessage = _profileMessageRepository.Create(message);
            var result = _mapper.Map<ProfileMessageDto>(createdMessage);

            // Populate author name
            var author = _userRepository.GetById(authorId);
            result.AuthorName = author?.Username ?? string.Empty;

            return result;
        }

        public ProfileMessageDto Update(long messageId, long authorId, UpdateMessageDto dto)
        {
            var message = _profileMessageRepository.GetById(messageId);
            if (message == null)
                throw new NotFoundException($"Profile message with ID {messageId} not found.");
            if (message.AuthorId != authorId)
                throw new UnauthorizedAccessException("Only the author can update this profile message.");
            message.UpdateContent(dto.Content, (ProfileMessage.ResourceType)dto.AttachedResourceType, dto.AttachedResourceId);
            var updatedMessage = _profileMessageRepository.Update(message);
            var result = _mapper.Map<ProfileMessageDto>(updatedMessage);

            return result;
        }

        public void Delete(long messageId, long userId)
        {
            var message = _profileMessageRepository.GetById(messageId);
            if (message == null)
                throw new NotFoundException($"Profile message with ID {messageId} not found.");

            bool isMessageAuthor = message.AuthorId == userId;

            if (!isMessageAuthor)
                throw new UnauthorizedAccessException("Only the message author can delete this message.");

            _profileMessageRepository.Delete(messageId);
        }

        public ProfileMessageDto GetById(long messageId)
        {
            var message = _profileMessageRepository.GetById(messageId);
            if (message == null)
                throw new NotFoundException($"Profile message with ID {messageId} not found.");
            var result = _mapper.Map<ProfileMessageDto>(message);
            // Populate author name
            var author = _userRepository.GetById(message.AuthorId);
            result.AuthorName = author?.Username ?? string.Empty;
            return result;
        }

        public List<ProfileMessageDto> GetByReceiverId(long receiverId)
        {
            var messages = _profileMessageRepository.GetByReceiverId(receiverId);
            var result = _mapper.Map<List<ProfileMessageDto>>(messages);
            // Populate author names
            foreach (var msgDto in result)
            {
                var author = _userRepository.GetById(msgDto.AuthorId);
                msgDto.AuthorName = author?.Username ?? string.Empty;
            }
            return result;
        }
    }
}
