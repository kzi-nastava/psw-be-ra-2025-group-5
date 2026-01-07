using AutoMapper;
using Explorer.BuildingBlocks.Core.Exceptions;
using Explorer.Stakeholders.API.Dtos.ClubMessages;
using Explorer.Stakeholders.API.Public.ClubMessages;
using Explorer.Stakeholders.Core.Domain.ClubMessages;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces.ClubMessages;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces.Clubs;
using System;
using System.Collections.Generic;

namespace Explorer.Stakeholders.Core.UseCases.Administration.Social
{
    public class ClubMessageService : IClubMessageService
    {
        private readonly IClubMessageRepository _messageRepository;
        private readonly IClubRepository _clubRepository;
        private readonly IMapper _mapper;

        public ClubMessageService(IClubMessageRepository messageRepository, IClubRepository clubRepository, IMapper mapper)
        {
            _messageRepository = messageRepository;
            _clubRepository = clubRepository;
            _mapper = mapper;
        }

        public ClubMessageDto Create(long clubId, long authorId, CreateClubMessageDto dto)
        {
            var club = _clubRepository.GetById(clubId);
            if (club == null)
                throw new NotFoundException($"Club with ID {clubId} not found.");

            if (!club.IsMember(authorId))
                throw new UnauthorizedAccessException("Only club members can post messages.");

            var message = new ClubMessage(
                clubId,
                authorId,
                dto.Content,
                (ClubMessage.ResourceType)dto.AttachedResourceType,
                dto.AttachedResourceId
            );

            var createdMessage = _messageRepository.Create(message);
            return _mapper.Map<ClubMessageDto>(createdMessage);
        }

        public ClubMessageDto Update(long messageId, long authorId, UpdateClubMessageDto dto)
        {
            var message = _messageRepository.GetById(messageId);
            if (message == null)
                throw new NotFoundException($"Message with ID {messageId} not found.");

            if (message.AuthorId != authorId)
                throw new UnauthorizedAccessException("Only the message author can update the message.");

            message.UpdateContent(
                dto.Content,
                (ClubMessage.ResourceType)dto.AttachedResourceType,
                dto.AttachedResourceId
            );

            var updatedMessage = _messageRepository.Update(message);
            return _mapper.Map<ClubMessageDto>(updatedMessage);
        }

        public void Delete(long messageId, long userId, bool isOwner)
        {
            var message = _messageRepository.GetById(messageId);
            if (message == null)
                throw new NotFoundException($"Message with ID {messageId} not found.");

            var club = _clubRepository.GetById(message.ClubId);
            if (club == null)
                throw new NotFoundException($"Club not found.");

            bool canDelete = isOwner || (club.CreatorId == userId);

            if (!canDelete)
                throw new UnauthorizedAccessException("Only the club owner can delete messages.");

            _messageRepository.Delete(messageId);
        }

        public ClubMessageDto GetById(long id)
        {
            var message = _messageRepository.GetById(id);
            if (message == null)
                throw new NotFoundException($"Message with ID {id} not found.");

            return _mapper.Map<ClubMessageDto>(message);
        }

        public List<ClubMessageDto> GetByClubId(long clubId)
        {
            var messages = _messageRepository.GetByClubId(clubId);
            return _mapper.Map<List<ClubMessageDto>>(messages);
        }
    }
}
