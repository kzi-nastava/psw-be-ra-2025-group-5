using Explorer.Payments.API.Dtos;
using Explorer.Payments.API.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Shopping
{
    [Route("api/coupon")]
    [ApiController]
    public class CouponController : ControllerBase
    {
        private readonly ICouponService _service;

        public CouponController(ICouponService service)
        {
            _service = service;
        }

        [Authorize(Policy = "authorPolicy")]
        [HttpPost]
        public ActionResult<CouponDto> Create([FromBody] CouponDto coupon)
        {
            var result = _service.Create(coupon);
            return Ok(result);
        }

        [Authorize(Policy = "authorPolicy")]
        [HttpPut]
        public ActionResult<CouponDto> Update([FromBody] CouponDto coupon)
        {
            var result = _service.Update(coupon);
            return Ok(result);
        }

        [Authorize(Policy = "authorPolicy")]
        [HttpDelete("{id:long}")]
        public IActionResult Delete(long id)
        {
            _service.Delete(id);
            return NoContent();
        }

        [Authorize(Policy = "authorPolicy")]
        [HttpGet("author/{authorId:long}")]
        public ActionResult<List<CouponDto>> GetByAuthor(long authorId)
        {
            var result = _service.GetByAuthor(authorId);
            return Ok(result);
        }

        [Authorize]
        [HttpGet("{id:long}")]
        public ActionResult<CouponDto> Get(long id)
        {
            var result = _service.Get(id);
            return Ok(result);
        }

        [Authorize(Policy = "touristPolicy")]
        [HttpGet("code/{code}")]
        public ActionResult<CouponDto> GetByCode(string code)
        {
            var result = _service.GetByCode(code);
            return Ok(result);
        }

    }
}
