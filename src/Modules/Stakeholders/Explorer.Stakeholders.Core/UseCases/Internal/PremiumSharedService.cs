using Explorer.Stakeholders.API.Internal;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces.Users;
using Explorer.Stakeholders.Core.Domain.Users;

namespace Explorer.Stakeholders.Core.UseCases.Internal
{
    public class PremiumSharedService : IPremiumSharedService
    {
        private readonly IUserPremiumRepository _userPremiumRepository;

        public PremiumSharedService(IUserPremiumRepository userPremiumRepository)
        {
            _userPremiumRepository = userPremiumRepository;
        }

        public void GrantPremium(long userId, DateTime validUntil)
        {
            UserPremium existing = null;

            try
            {
                existing = _userPremiumRepository.Get((int)userId);
            }
            catch (KeyNotFoundException)
            {
                // intentionally ignored
            }

            if (existing == null)
            {
                var premium = new UserPremium(userId, validUntil);
                _userPremiumRepository.Add(premium);
            }
            else
            {
                existing.Extend(validUntil);
                _userPremiumRepository.Update(existing);
            }
        }

        public void ExtendPremium(long userId, DateTime validUntil)
        {
            var premium = _userPremiumRepository.Get((int)userId);
            premium.Extend(validUntil);
            _userPremiumRepository.Update(premium);
        }

        public bool IsPremium(long userId)
        {
            try
            {
                var premium = _userPremiumRepository.Get((int)userId);
                return premium.IsActive();
            }
            catch (KeyNotFoundException)
            {
                return false;
            }
        }
    }
}
