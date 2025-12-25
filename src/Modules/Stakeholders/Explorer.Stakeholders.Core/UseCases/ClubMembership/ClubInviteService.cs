using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using Explorer.Stakeholders.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Explorer.Stakeholders.Core.Domain.Club;
using Explorer.Stakeholders.API.Public;

namespace Explorer.Stakeholders.Core.UseCases.ClubMembership
{
    public class ClubInviteService : IClubInviteService
    {
        private readonly IClubRepository _clubRepository;
        private readonly INotificationService _notificationService;
        private readonly IClubInviteRepository _clubInviteRepository;

        public ClubInviteService(IClubRepository clubRepository, INotificationService notificationService, IClubInviteRepository clubInviteRepository)
        {
            _clubRepository = clubRepository;
            _notificationService = notificationService;
            _clubInviteRepository = clubInviteRepository;
        }

        public void InviteTourist(long clubId, long touristId, long ownerId)
        {
            var club = _clubRepository.GetById(clubId);
            if (club == null)
                throw new KeyNotFoundException("Club not found");

            if (club.CreatorId != ownerId)
                throw new UnauthorizedAccessException("Only the club owner can send invites");

            if (!club.IsActive())
                throw new InvalidOperationException("Cannot invite to a closed club");

            if (club.IsMember(touristId))
                throw new InvalidOperationException("Tourist is already a member");

            if (_clubInviteRepository.Exists(clubId, touristId))
                throw new InvalidOperationException("Invite already sent");

            var notification = _notificationService.Create(new NotificationDto
            {
                UserId = touristId,
                Title = "Club invitation",
                Message = $"You have been invited to join the club '{club.Name}'",
                Type = "ClubInvite",
                CreatedAt = DateTime.UtcNow
            });

            var invite = new ClubInvite(clubId, touristId, DateTime.UtcNow, notification.Id);
            _clubInviteRepository.Create(invite);
        }

        public void AcceptInvite(long inviteId, long touristId)
        {
            var invite = _clubInviteRepository.GetById(inviteId);
            if (invite == null)
                throw new KeyNotFoundException("Invite not found");

            if (invite.TouristId != touristId)
                throw new UnauthorizedAccessException();

            var club = _clubRepository.GetById(invite.ClubId);
            if (club == null)
                throw new KeyNotFoundException("Club not found");

            if (club.IsMember(touristId))
                throw new InvalidOperationException("Already a member");

            club.AddMember(touristId);
            _clubRepository.Update(club);

            _clubInviteRepository.Delete(invite);

            var notification = _notificationService.GetById(invite.NotificationId);
            _notificationService.Delete(notification.Id);
        }

        public void RejectInvite(long inviteId, long touristId)
        {
            var invite = _clubInviteRepository.GetById(inviteId);
            if (invite == null)
                throw new KeyNotFoundException("Invite not found");

            if (invite.TouristId != touristId)
                throw new UnauthorizedAccessException();

            invite = _clubInviteRepository.GetById(inviteId);
            _clubInviteRepository.Delete(invite);

            var notification = _notificationService.GetById(invite.NotificationId);
            _notificationService.Delete(notification.Id);
        }

    }
}
