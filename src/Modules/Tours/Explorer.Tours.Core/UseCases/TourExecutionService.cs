using Explorer.Tours.Core.Domain;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using Explorer.Tours.API.Dtos;
using AutoMapper; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.Tours.API.Public;

namespace Explorer.Tours.Core.UseCases
{
    public class TourExecutionService : ITourExecutionService
    {
        private readonly ITourExecutionRepository _repo;
        private readonly ITourRepository _tourRepo;
        private readonly IMapper _mapper;
        private const double DefaultThresholdMeters = 20.0;

        public TourExecutionService(ITourExecutionRepository repo, ITourRepository tourRepo, IMapper mapper)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _tourRepo = tourRepo ?? throw new ArgumentNullException(nameof(tourRepo));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public StartExecutionResultDto StartExecution(long userId, long tourId)
        {
            // TODO: dodati proveru kupovine 
            var existing = _repo.GetActiveForUser(userId, tourId);
            if (existing != null)
            {
                var existingNext = GetNextKeyPointForExecution(existing);
                return new StartExecutionResultDto
                {
                    ExecutionId = existing.Id,
                    NextKeyPoint = existingNext != null ? _mapper.Map<KeyPointDto>(existingNext) : null,
                    StartTime = existing.StartTime
                };
            }
            var execution = TourExecution.StartNew(userId, tourId);
            _repo.Add(execution);
            var tour = _tourRepo.Get(tourId);
            var nextKeyPoint = tour?.KeyPoints.OrderBy(k => k.Position).FirstOrDefault();
            return new StartExecutionResultDto
            {
                ExecutionId = execution.Id,
                NextKeyPoint = nextKeyPoint != null ? _mapper.Map<KeyPointDto>(nextKeyPoint) : null,
                StartTime = execution.StartTime
            };
        }

        public CheckProximityDto CheckProximity(long executionId, LocationDto location)
        {
            if (location == null) throw new ArgumentNullException(nameof(location));
            ValidateCoordinates(location.Latitude, location.Longitude);

            var execution = _repo.Get(executionId);
            if (execution == null) throw new KeyNotFoundException("TourExecution not found.");

            execution.UpdateActivity();

            var tour = _tourRepo.Get(execution.TourId);
            var ordered = tour?.KeyPoints.OrderBy(k => k.Position).ToList() ?? new List<KeyPoint>();

            KeyPoint next = null;
            if (execution.CurrentKeyPoint >= 0 && execution.CurrentKeyPoint < ordered.Count)
                next = ordered[execution.CurrentKeyPoint];

            bool isNear = false;
            long? completedKeyPointId = null;
            DateTime? completedAt = null;
            bool unlockedSecret = false;
            string? secretText = null;

            if (next != null)
            {
                double dist = HaversineDistanceMeters(
                    location.Latitude,
                    location.Longitude,
                    next.Location.Latitude,
                    next.Location.Longitude
                );

                if (dist <= DefaultThresholdMeters)
                {
                    execution.CompleteKeyPoint(next.Id, dist);

                    isNear = true;
                    completedKeyPointId = next.Id;
                    completedAt = DateTime.UtcNow;

                    if (!string.IsNullOrWhiteSpace(next.Secret))
                    {
                        unlockedSecret = true;
                        secretText = next.Secret;
                    }
                }
            }

            _repo.Update(execution);

            double percent = 0.0;
            if (ordered.Count > 0)
                percent = (double)execution.CompletedKeyPoints.Count / ordered.Count * 100.0;

            KeyPoint upcoming = null;
            if (execution.CurrentKeyPoint >= 0 && execution.CurrentKeyPoint < ordered.Count)
                upcoming = ordered[execution.CurrentKeyPoint];

            return new CheckProximityDto
            {
                IsNearKeyPoint = isNear,
                CompletedKeyPointId = completedKeyPointId,
                CompletedAt = completedAt,
                UnlockedSecret = unlockedSecret,
                SecretText = secretText,
                PercentCompleted = percent,
                LastActivity = execution.LastActivity,
                NextKeyPoint = upcoming != null ? _mapper.Map<KeyPointDto>(upcoming) : null
            };
        }

        public void CompleteExecution(long executionId)
        {
            var ex = _repo.Get(executionId);
            if (ex == null) throw new KeyNotFoundException("Execution not found");
            ex.CompleteTour();
            _repo.Update(ex);
        }

        public void AbandonExecution(long executionId)
        {
            var ex = _repo.Get(executionId);
            if (ex == null) throw new KeyNotFoundException("Execution not found");
            ex.AbandonTour();
            _repo.Update(ex);
        }

        public TourExecutionDto GetExecution(long executionId)
        {
            var ex = _repo.Get(executionId);
            if (ex == null) return null;
            return _mapper.Map<TourExecutionDto>(ex);
        }

        private KeyPoint GetNextKeyPointForExecution(TourExecution execution)
        {
            var tour = _tourRepo.Get(execution.TourId);
            var ordered = tour?.KeyPoints.OrderBy(k => k.Position).ToList() ?? new List<KeyPoint>();
            if (execution.CurrentKeyPoint >= 0 && execution.CurrentKeyPoint < ordered.Count)
                return ordered[execution.CurrentKeyPoint];
            return null;
        }

        private void ValidateCoordinates(double lat, double lon)
        {
            if (double.IsNaN(lat) || double.IsNaN(lon))
                throw new ArgumentException("Invalid coordinates");
            if (lat < -90 || lat > 90) throw new ArgumentOutOfRangeException(nameof(lat));
            if (lon < -180 || lon > 180) throw new ArgumentOutOfRangeException(nameof(lon));
        }


        // pomocne funkcije za izracunavanje blizine korisnika kljucnoj tacki 
        private double ToRad(double degrees) => degrees * Math.PI / 180.0;

        private double HaversineDistanceMeters(double lat1, double lon1, double lat2, double lon2)
        {
            const double EarthRadiusMeters = 6371000.0; 

            var dLat = ToRad(lat2 - lat1);
            var dLon = ToRad(lon2 - lon1);

            var rLat1 = ToRad(lat1);
            var rLat2 = ToRad(lat2);

            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(rLat1) * Math.Cos(rLat2) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return EarthRadiusMeters * c;
        }

    }
}
