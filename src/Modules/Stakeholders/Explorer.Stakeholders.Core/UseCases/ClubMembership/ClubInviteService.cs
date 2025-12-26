using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces.Clubs;
using Explorer.Stakeholders.API.Public.Notifications;
using Explorer.Stakeholders.Core.UseCases.Administration.Users;
using Explorer.Stakeholders.API.Dtos.Notifications;
using Explorer.Stakeholders.API.Dtos.Users;
using Explorer.Stakeholders.Core.Domain.Users;

namespace Explorer.Stakeholders.Core.UseCases.ClubMembership
{
    public class ClubInviteService : IClubInviteService
    {
        private readonly IClubRepository _clubRepository;
        private readonly INotificationService _notificationService;
        private readonly IClubInviteRepository _clubInviteRepository;
        private readonly IUserService _userService;

        public ClubInviteService(IClubRepository clubRepository, INotificationService notificationService, IClubInviteRepository clubInviteRepository, IUserService userService)
        {
            _clubRepository = clubRepository;
            _notificationService = notificationService;
            _clubInviteRepository = clubInviteRepository;
            _userService = userService;
        }

        public void InviteTourist(long clubId, long touristId, long ownerId)
        {
            var club = _clubRepository.GetById(clubId);
            if (club == null)
                throw new KeyNotFoundException("Club not found");

            if (club.CreatorId != ownerId)
                throw new UnauthorizedAccessException("Only the club owner can send invites");

            if(touristId == ownerId)
                throw new InvalidOperationException("Owner cannot invite themselves");

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
                CreatedAt = DateTime.UtcNow,
                ClubId = clubId
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
        public List<ClubInviteDto> GetInvitesForClub(long clubId, long ownerId)
        {
            var club = _clubRepository.GetById(clubId);
            if (club == null)
                throw new KeyNotFoundException("Club not found");

            if (club.CreatorId != ownerId)
                throw new UnauthorizedAccessException("Only the club owner can view invites");

            var invites = _clubInviteRepository.GetByClubId(clubId);

            var result = new List<ClubInviteDto>();

            foreach (var invite in invites)
            {
                var user = _userService.GetById(invite.TouristId);

                result.Add(new ClubInviteDto
                {
                    Id = invite.Id,
                    ClubId = invite.ClubId,
                    TouristUsername = user.Username,
                    CreatedAt = invite.CreatedAt
                });
            }

            return result;
        }
        public List<UserDto> GetAvailableTourists(long clubId, long ownerId)
        {
            var club = _clubRepository.GetById(clubId);
            if (club == null)
                throw new KeyNotFoundException("Club not found");

            if (club.CreatorId != ownerId)
                throw new UnauthorizedAccessException("Only owner can invite tourists");

            if (!club.IsActive())
                throw new InvalidOperationException("Club is closed");

            var allUsers = _userService.GetAll();

            var result = allUsers
                .Where(u =>
                    u.Id != ownerId &&
                    Enum.TryParse<UserRole>(u.Role, out var role) && role == UserRole.Tourist &&
                    !club.IsMember(u.Id) &&           
                    !_clubInviteRepository.Exists(clubId, u.Id)) 
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    Username = u.Username
                })
                .ToList();

            return result;
        }

        public ClubInviteDto? GetInviteForUserAndClub(long clubId, long touristId)
        {
            var invite = _clubInviteRepository
                .GetByClubId(clubId)
                .FirstOrDefault(i => i.TouristId == touristId);

            if (invite == null)
                return null;

            var user = _userService.GetById(touristId);

            return new ClubInviteDto
            {
                Id = invite.Id,
                ClubId = invite.ClubId,
                TouristUsername = user.Username,
                CreatedAt = invite.CreatedAt
            };
        }



    }
}
