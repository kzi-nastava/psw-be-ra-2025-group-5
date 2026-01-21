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
            try
            {
                existing = _userPremiumRepository.Get((int)userId);
            }
            catch (KeyNotFoundException)
            {
                
            }

            if (existing == null)
            {
                var newPremium = new UserPremium(userId, validUntil);
                _userPremiumRepository.Add(newPremium);
            }
            else
            {
                existing.Extend(validUntil);
                _userPremiumRepository.Update(existing);
            }
        }

        public void ExtendPremium(long userId, DateTime newUntil)
        {
            var premium = _userPremiumRepository.Get((int)userId);
            premium.Extend(newUntil);
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
                return premium.IsActive();
            }
            catch (KeyNotFoundException)
            {
                return false;
            }
        }
    }
}
