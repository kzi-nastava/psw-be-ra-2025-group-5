using Explorer.Payments.API.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payments.API.Public
{
    public interface ICouponService
    {
        CouponDto Create(CouponDto coupon);
        CouponDto Update(CouponDto coupon);
        void Delete(long couponId);

        CouponDto Get(long couponId);
        CouponDto GetByCode(string code);
        List<CouponDto> GetByAuthor(long authorId);
    }
}
