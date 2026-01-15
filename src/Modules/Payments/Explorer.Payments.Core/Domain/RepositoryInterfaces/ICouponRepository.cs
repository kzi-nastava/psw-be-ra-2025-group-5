using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payments.Core.Domain.RepositoryInterfaces
{
    public interface ICouponRepository
    {
        Coupon Create(Coupon entity);
        Coupon Update(Coupon entity);
        void Delete(long id);

        Coupon Get(long id);
        Coupon GetByCode(string code);
        List<Coupon> GetByAuthor(long authorId);

    }
}
