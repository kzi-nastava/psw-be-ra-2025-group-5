using Explorer.Stakeholders.API.Internal;
using Explorer.Stakeholders.API.Public.Users;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces.Users;
using Explorer.Stakeholders.Core.Domain.Users;

namespace Explorer.Stakeholders.Core.UseCases.Internal
{
    public class PremiumSharedService : IPremiumSharedService
    {
        private readonly IPremiumService _premiumService;

        public PremiumSharedService(IPremiumService premiumService)
        {
            _premiumService = premiumService;
        }

        public void GrantPremium(long userId, DateTime validUntil)
            => _premiumService.GrantPremium(userId, validUntil);

        public void ExtendPremium(long userId, DateTime validUntil)
            => _premiumService.ExtendPremium(userId, validUntil);

        public bool IsPremium(long userId)
            => _premiumService.IsPremium(userId);
    }
}
