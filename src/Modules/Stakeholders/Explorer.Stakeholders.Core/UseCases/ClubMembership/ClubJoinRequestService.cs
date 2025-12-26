using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.API.Public.Notifications;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces.Clubs;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces.Users;
using Explorer.Stakeholders.API.Dtos.Notifications;

namespace Explorer.Stakeholders.Core.UseCases.ClubMembership
{
    public class ClubJoinRequestService : IClubJoinRequestService
    {
        private readonly IClubRepository _clubRepository;
        private readonly IClubJoinRequestRepository _requestRepository;
        private readonly INotificationService _notificationService;
        private readonly IPersonRepository _personRepository;


        public ClubJoinRequestService(
       IClubRepository clubRepository,
       IClubJoinRequestRepository requestRepository,
       INotificationService notificationService,
       IPersonRepository personRepository)
        {
            _clubRepository = clubRepository;
            _requestRepository = requestRepository;
            _notificationService = notificationService;
            _personRepository = personRepository;
        }


        public void RequestToJoin(long clubId, long touristId)
        {
            var club = _clubRepository.GetById(clubId)
                ?? throw new KeyNotFoundException("Club not found");

            if (!club.IsActive())
                throw new InvalidOperationException("Club is closed");

            if (club.IsMember(touristId))
                throw new InvalidOperationException("Already a member");

            if (_requestRepository.Exists(clubId, touristId))
                throw new InvalidOperationException("Request already exists");

            var notification = _notificationService.Create(new NotificationDto
            {
                UserId = club.CreatorId,
                Title = "New join request",
                Message = "A tourist requested to join your club",
                Type = "ClubInvite",
                ClubId = clubId
            });

            var request = new ClubJoinRequest(clubId, touristId, notification.Id);
            _requestRepository.Create(request);
        }

       
        public void CancelRequest(long clubId, long touristId)
        {
            var request = _requestRepository.GetByClubAndTourist(clubId, touristId)
                ?? throw new KeyNotFoundException("Request not found");

            _requestRepository.Delete(request);
            _notificationService.Delete(request.NotificationId);
        }

        public string GetMembershipStatus(long clubId, long touristId)
        {
            var club = _clubRepository.GetById(clubId)
                ?? throw new KeyNotFoundException();

            if (club.IsMember(touristId))
                return "Member";

            if (_requestRepository.Exists(clubId, touristId))
                return "Pending";

            return "None";
        }

        public void AcceptRequest(long clubId, long touristId, long ownerId)
        {
            var club = _clubRepository.GetById(clubId)
                ?? throw new KeyNotFoundException("Club not found");

            if (club.CreatorId != ownerId)
                throw new UnauthorizedAccessException("Only owner can accept requests");

            if (!club.IsActive())
                throw new InvalidOperationException("Club is closed");

            var request = _requestRepository.GetByClubAndTourist(clubId, touristId)
                ?? throw new KeyNotFoundException("Request not found");

            club.AddMember(touristId);
            _clubRepository.Update(club);

            _requestRepository.Delete(request);

            _notificationService.Create(new NotificationDto
            {
                UserId = touristId,
                Title = "Join request accepted",
                Message = "Your request to join the club has been accepted",
                Type = "ClubJoin",
                ClubId = clubId
            });
        }

        public void RejectRequest(long clubId, long touristId, long ownerId)
        {
            var club = _clubRepository.GetById(clubId)
                ?? throw new KeyNotFoundException("Club not found");

            if (club.CreatorId != ownerId)
                throw new UnauthorizedAccessException("Only owner can reject requests");

            var request = _requestRepository.GetByClubAndTourist(clubId, touristId)
                ?? throw new KeyNotFoundException("Request not found");

            _requestRepository.Delete(request);

            _notificationService.Create(new NotificationDto
            {
                UserId = touristId,
                Title = "Join request rejected",
                Message = "Your request to join the club has been rejected",
                Type = "ClubJoin",
                ClubId = clubId
            });
        }

        public List<ClubJoinRequestDto> GetPendingRequests(long clubId, long ownerId)
        {
            var club = _clubRepository.GetById(clubId)
                ?? throw new KeyNotFoundException("Club not found");

            if (club.CreatorId != ownerId)
                throw new UnauthorizedAccessException("Only owner can view requests");

            var requests = _requestRepository.GetByClubId(clubId);

            return requests.Select(r =>
            {
                var person = _personRepository.GetByUserId(r.TouristId);

                var touristName = person != null
                    ? $"{person.Name} {person.Surname}".Trim()
                    : ""; 

                return new ClubJoinRequestDto
                {
                    Id = r.Id,
                    ClubId = r.ClubId,
                    TouristId = r.TouristId,
                    TouristUsername = touristName, 
                    TouristName = touristName,
                    RequestedAt = r.CreatedAt
                };
            }).ToList();

        }
    }

}
