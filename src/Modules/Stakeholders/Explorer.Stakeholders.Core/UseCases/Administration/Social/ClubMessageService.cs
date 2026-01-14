using AutoMapper;
using AutoMapper.Execution;
using Explorer.BuildingBlocks.Core.Exceptions;
using Explorer.Stakeholders.API.Dtos.ClubMessages;
using Explorer.Stakeholders.API.Dtos.Notifications;
using Explorer.Stakeholders.API.Public.ClubMessages;
using Explorer.Stakeholders.API.Public.Notifications;
using Explorer.Stakeholders.Core.Domain.ClubMessages;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces.ClubMessages;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces.Clubs;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces.Users;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Explorer.Stakeholders.Core.UseCases.Administration.Social
{
    public class ClubMessageService : IClubMessageService
    {
        private readonly IClubMessageRepository _messageRepository;
        private readonly IClubRepository _clubRepository;
        private readonly IUserRepository _userRepository;
        private readonly INotificationService _notificationService;
        private readonly IMapper _mapper;

        public ClubMessageService(IClubMessageRepository messageRepository, IClubRepository clubRepository, IUserRepository userRepository, INotificationService notificationService, IMapper mapper)
        {
            _messageRepository = messageRepository;
            _clubRepository = clubRepository;
            _userRepository = userRepository;
            _notificationService = notificationService;
            _mapper = mapper;
        }

        public ClubMessageDto Create(long clubId, long authorId, CreateClubMessageDto dto)
        {
            var club = _clubRepository.GetById(clubId);
            if (club == null)
                throw new NotFoundException($"Club with ID {clubId} not found.");

            if (!club.IsMember(authorId) && authorId != club.CreatorId)
                throw new UnauthorizedAccessException("Only club members can post messages.");

            var message = new ClubMessage(
                clubId,
                authorId,
                dto.Content,
                (ClubMessage.ResourceType)dto.AttachedResourceType,
                dto.AttachedResourceId
            );

            var createdMessage = _messageRepository.Create(message);
            var result = _mapper.Map<ClubMessageDto>(createdMessage);
            
            // Populate author name
            var author = _userRepository.GetById(authorId);
            result.AuthorName = author?.Username ?? string.Empty;

            var notification = $"There is a new message from {result.AuthorName} in {club.Name}";
            foreach (var member in club.Members)
            {
                if (member.TouristId != authorId)
                    _notificationService.Create(new NotificationDto
                    {
                        UserId = member.TouristId,
                        Title = "New message",
                        Message = notification,
                        Type = "NewMessage",
                        BlogId = dto.AttachedResourceType == 2 ? dto.AttachedResourceId : null,
                        TourId = dto.AttachedResourceType == 1 ? dto.AttachedResourceId : null,
                        ActionUrl = $"/clubs/details/{club.Id}/messages",
                        CreatedAt = DateTime.UtcNow
                    });
            }

            if(authorId != club.CreatorId)
                _notificationService.Create(new NotificationDto
                {
                    UserId = club.CreatorId,
                    Title = "New message",
                    Message = notification,
                    Type = "NewMessage",
                    BlogId = dto.AttachedResourceType == 2 ? dto.AttachedResourceId : null,
                    TourId = dto.AttachedResourceType == 1 ? dto.AttachedResourceId : null,
                    ActionUrl = $"/clubs/details/{club.Id}/messages",
                    CreatedAt = DateTime.UtcNow
                });

            return result;
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
            var result = _mapper.Map<ClubMessageDto>(updatedMessage);
            
            // Populate author name
            var author = _userRepository.GetById(authorId);
            result.AuthorName = author?.Username ?? string.Empty;
            
            return result;
        }

        public void Delete(long messageId, long userId, bool isOwner)
        {
            var message = _messageRepository.GetById(messageId);
            if (message == null)
                throw new NotFoundException($"Message with ID {messageId} not found.");

            var club = _clubRepository.GetById(message.ClubId);
            if (club == null)
                throw new NotFoundException($"Club not found.");

            // User can delete if they are the club owner OR if they are the message author
            bool isClubOwner = club.CreatorId == userId;
            bool isMessageAuthor = message.AuthorId == userId;

            if (!isClubOwner && !isMessageAuthor)
                throw new UnauthorizedAccessException("Only the message author or club owner can delete this message.");

            _messageRepository.Delete(messageId);
        }

        public ClubMessageDto GetById(long id)
        {
            var message = _messageRepository.GetById(id);
            if (message == null)
                throw new NotFoundException($"Message with ID {id} not found.");

            var result = _mapper.Map<ClubMessageDto>(message);
            
            // Populate author name
            var author = _userRepository.GetById(message.AuthorId);
            result.AuthorName = author?.Username ?? string.Empty;
            
            return result;
        }

        public List<ClubMessageDto> GetByClubId(long clubId)
        {
            var messages = _messageRepository.GetByClubId(clubId);
            
            return messages.Select(m =>
            {
                var dto = _mapper.Map<ClubMessageDto>(m);
                var author = _userRepository.GetById(m.AuthorId);
                dto.AuthorName = author?.Username ?? string.Empty;
                return dto;
            }).ToList();
        }
    }
}
