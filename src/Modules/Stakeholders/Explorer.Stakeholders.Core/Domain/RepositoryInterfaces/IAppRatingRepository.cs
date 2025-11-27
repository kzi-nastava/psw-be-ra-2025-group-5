using Explorer.Stakeholders.Core.Domain;
using System.Collections.Generic;

namespace Explorer.Stakeholders.Core.Domain.RepositoryInterfaces
{
    public interface IAppRatingRepository
    {
        AppRating Create(AppRating rating);
        AppRating Update(AppRating rating);
        void Delete(long id);
        IEnumerable<AppRating> GetAll();

        AppRating Get(long id);
    }
}
