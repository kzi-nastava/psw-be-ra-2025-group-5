using Explorer.Stakeholders.API.Public.Users;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces.Users;
using Explorer.Stakeholders.Core.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Core.UseCases.Administration.Users
{
    public class PremiumService: IPremiumService
    {
        private readonly IUserPremiumRepository _userPremiumRepository;

        public PremiumService(IUserPremiumRepository userPremiumRepository)
        {
            _userPremiumRepository = userPremiumRepository;
        }

        public void GrantPremium(long userId, DateTime validUntil)
        {
            UserPremium existing = null;

            try { existing = _userPremiumRepository.Get((int)userId); }
            catch (KeyNotFoundException) { }

            if (existing == null)
            {
                _userPremiumRepository.Add(new UserPremium(userId, validUntil));
                return;
            }

            if (existing.IsActive())
                throw new InvalidOperationException("User already has premium.");

            // istekao premium -> obriši red, pa dodaj novi
            _userPremiumRepository.Delete((int)userId);
            _userPremiumRepository.Add(new UserPremium(userId, validUntil));
        }


        public void ExtendPremium(long userId, DateTime _ignored)
        {
            var premium = _userPremiumRepository.Get((int)userId);

            if (!premium.IsActive())
            {
                _userPremiumRepository.Delete((int)userId);
                throw new InvalidOperationException("Premium expired. Purchase again.");
            }

            premium.ValidUntil = premium.ValidUntil!.Value.AddDays(30);
            _userPremiumRepository.Update(premium);
        }


        public void RemovePremium(long userId)
        {
            _userPremiumRepository.Delete((int)userId);
        }

        public bool IsPremium(long userId)
        {
            try
            {
                var premium = _userPremiumRepository.Get((int)userId);
                if (premium.IsActive()) return true;

                // expired -> obriši red
                _userPremiumRepository.Delete((int)userId);
                return false;
            }
            catch (KeyNotFoundException)
            {
                return false;
            }
        }
    }
}
