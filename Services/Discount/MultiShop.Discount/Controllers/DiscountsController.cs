using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MultiShop.Discount.DTOs;
using MultiShop.Discount.Services;

namespace MultiShop.Discount.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountsController : ControllerBase
    {
        private readonly IDiscountService _discountService;

        public DiscountsController(IDiscountService discountService)
        {
            _discountService = discountService;
        }

        [HttpGet]
        public async Task<IActionResult> DiscountCouponList()
        {
            var values = await _discountService.GetAllCouponAsync();
            return Ok(values);
        }   

        [HttpGet("{id}")]
        public async Task<IActionResult> DiscountCouponById(int id)
        {
            var values = await _discountService.GetByIdCouponAsync(id);
            return Ok(values);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDiscountCoupon(CreateDiscountCouponDto createCouponDto)
        {
            await _discountService.CreateCouponAsync(createCouponDto);
            return Ok("Kupon oluşturuldu");
        }


        [HttpGet("GetCodeDetailByCode/{code}")]
        public async Task<IActionResult> GetCodeDetailByCode(string code)
        {
            var values = await _discountService.GetCodeDetailByCode(code);
            return Ok(values);
        }

        [HttpGet("GetDiscountCouponCountRate/{code}")]
        public IActionResult GetDiscountCouponCountRate(string code)
        {
            var values = _discountService.GetDiscountCouponCountRate(code);
            return Ok(values);
        }

       

        [HttpDelete]
        public async Task<IActionResult> DeleteDiscountCoupon(int id)
        {
            await  _discountService.DeleteCouponAsync(id);
            return Ok("Kupon silindi");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateDiscountCoupon(UpdateDiscountCouponDto updateCouponDto)
        {
            await _discountService.UpdateCouponAsync(updateCouponDto);
            return Ok("Kupon güncellendi");
        }

        [HttpGet("GetDiscountCouponCount")]
        public async Task<IActionResult> GetDiscountCouponCount()
        {
            var values = await _discountService.GetDiscountCouponCount();
            return Ok(values);
        }

        [HttpGet("GetDiscountByProductId/{productId}")]
        public async Task<IActionResult> GetDiscountByProductId(string productId)
        {
            var value = await _discountService.GetDiscountByProductIdAsync(productId);
            return Ok(value);
        }

        [HttpGet("GetActiveProductDiscounts")]
        public async Task<IActionResult> GetActiveProductDiscounts()
        {
            var values = await _discountService.GetActiveProductDiscountsAsync();
            return Ok(values);
        }

        [HttpPost("SetProductDiscount")]
        public async Task<IActionResult> SetProductDiscount([FromBody] CreateDiscountCouponDto dto)
        {
            if (string.IsNullOrEmpty(dto.ProductId))
            {
                return BadRequest("ProductId gereklidir.");
            }
            await _discountService.SetProductDiscountAsync(dto.ProductId, dto.Rate, dto.ValidDate, dto.IsActive);
            return Ok("Ürün indirimi başarıyla güncellendi.");
        }

        [HttpDelete("DeleteDiscountByProductId/{productId}")]
        public async Task<IActionResult> DeleteDiscountByProductId(string productId)
        {
            await _discountService.DeleteDiscountByProductIdAsync(productId);
            return Ok("Ürün indirimi silindi.");
        }
    }
}
