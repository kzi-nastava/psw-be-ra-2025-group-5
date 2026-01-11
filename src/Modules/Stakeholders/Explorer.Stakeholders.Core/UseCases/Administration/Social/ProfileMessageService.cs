using AutoMapper;
using Explorer.BuildingBlocks.Core.Exceptions;
using Explorer.Stakeholders.API.Dtos.ClubMessages;
using Explorer.Stakeholders.API.Dtos.ProfileMessages;
using Explorer.Stakeholders.API.Dtos.Users;
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
    public class ProfileMessageService
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

        public ProfileMessageDto Create(long receiverId, long authorId, ProfileMessageDto dto)
        {
            var message = new ProfileMessage(
                receiverId,
                authorId,
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

        public ProfileMessageDto Update(long messageId, long authorId, ProfileMessageDto dto)
        {
            var message = _profileMessageRepository.GetById(messageId);
            if (message == null)
                throw new NotFoundException($"Profile message with ID {messageId} not found.");
            if (message.AuthorId != authorId)
                throw new UnauthorizedAccessException("Only the author can update this profile message.");
            message.UpdateContent(dto.Content, (ProfileMessage.ResourceType)dto.AttachedResourceType, dto.AttachedResourceId);
            var updatedMessage = _profileMessageRepository.Update(message);
            var result = _mapper.Map<ProfileMessageDto>(updatedMessage);
            // Populate author name
            var author = _userRepository.GetById(authorId);

            return result;
        }

        public ProfileMessageDto Delete(long messageId) {
            var message = _profileMessageRepository.GetById(messageId);
            if (message == null)
                throw new NotFoundException($"Profile message with ID {messageId} not found.");
            _profileMessageRepository.Delete(messageId);
            var result = _mapper.Map<ProfileMessageDto>(message);
            // Populate author name
            var author = _userRepository.GetById(message.AuthorId);
            result.AuthorName = author?.Username ?? string.Empty;
            return result;
        }

        public ProfileMessageDto Get(long messageId) {
            var message = _profileMessageRepository.GetById(messageId);
            if (message == null)
                throw new NotFoundException($"Profile message with ID {messageId} not found.");
            var result = _mapper.Map<ProfileMessageDto>(message);
            // Populate author name
            var author = _userRepository.GetById(message.AuthorId);
            result.AuthorName = author?.Username ?? string.Empty;
            return result;
        }
    }
