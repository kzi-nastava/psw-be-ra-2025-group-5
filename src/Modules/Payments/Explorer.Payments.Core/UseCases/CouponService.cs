using AutoMapper;
using Explorer.Payments.API.Dtos;
using Explorer.Payments.API.Public;
using Explorer.Payments.Core.Domain;
using Explorer.Payments.Core.Domain.RepositoryInterfaces;

namespace Explorer.Payments.Core.UseCases
{
    public class CouponService: ICouponService
    {
        private readonly ICouponRepository _CouponRepository;
        private readonly IMapper _mapper;

        public CouponService(ICouponRepository repository, IMapper mapper)
        {
            _CouponRepository = repository;
            _mapper = mapper;
        }

        public CouponDto Create(CouponDto coupon)
        {
            var result = _CouponRepository.Create(_mapper.Map<Coupon>(coupon));
            return _mapper.Map<CouponDto>(result);
        }

        public CouponDto Update(CouponDto coupon)
        {
            var result = _CouponRepository.Update(_mapper.Map<Coupon>(coupon));
            return _mapper.Map<CouponDto>(result);
        }

        public void Delete(long couponId)
        {
            _CouponRepository.Delete(couponId);
        }

        public CouponDto Get(long couponId)
        {
            var result = _CouponRepository.Get(couponId);
            return _mapper.Map<CouponDto>(result);
        }

        public CouponDto GetByCode(string code)
        {
            var result = _CouponRepository.GetByCode(code);
            return _mapper.Map<CouponDto>(result);
        }

        public List<CouponDto> GetByAuthor(long authorId)
        {
            var result = _CouponRepository.GetByAuthor(authorId);
            return _mapper.Map<List<CouponDto>>(result);
        }

    }
}
