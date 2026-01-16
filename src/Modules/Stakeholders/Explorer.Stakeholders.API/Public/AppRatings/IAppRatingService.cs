using Explorer.Stakeholders.API.Dtos.AppRatings;

namespace Explorer.Stakeholders.API.Public.AppRatings
    {
        public interface IAppRatingService
        {
            AppRatingDto Create(AppRatingDto dto);
            AppRatingDto Update(AppRatingDto dto);
            void Delete(long id);
            IEnumerable<AppRatingDto> GetAll();
            AppRatingDto Get(long id);
           IEnumerable<AppRatingDto> GetByUserId(long userId);

    }
}
