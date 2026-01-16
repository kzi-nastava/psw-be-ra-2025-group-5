using Explorer.Tours.API.Dtos.Tours;
using Explorer.Tours.Core.Domain.RepositoryInterfaces.Tours;
using Explorer.Tours.Core.Domain.Tours.Entities;
using Explorer.Tours.API.Public.Tour;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Core.UseCases.Tours
{
    public class TourManualService : ITourManualService
    {
        private readonly ITourManualRepository _repository;

        public TourManualService(ITourManualRepository repository)
        {
            _repository = repository;
        }

        public TourManualStatusDto GetStatus(long userId, string pageKey)
        {
            var progress = _repository.Get(userId, pageKey);

            if (progress == null)
            {
                return new TourManualStatusDto
                {
                    PageKey = pageKey,
                    Seen = false
                };
            }

            return new TourManualStatusDto
            {
                PageKey = pageKey,
                Seen = progress.Seen
            };
        }

        public void MarkAsSeen(long userId, string pageKey)
        {
            var progress = _repository.Get(userId, pageKey);

            if (progress == null)
            {
                progress = new TourManualProgress(userId, pageKey);
                progress.MarkAsSeen();
                _repository.Create(progress);
                return;
            }

            if (!progress.Seen)
            {
                progress.MarkAsSeen();
                _repository.Update(progress);
            }
        }
    }
}
