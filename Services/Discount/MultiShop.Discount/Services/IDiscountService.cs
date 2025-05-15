using MultiShop.Discount.DTOs;

namespace MultiShop.Discount.Services
{
    public interface IDiscountService
    {
        Task<List<ResultDiscountCouponDto>> GetAllCouponAsync();
        Task CreateCouponAsync(CreateDiscountCouponDto createCouponDto);
        Task UpdateCouponAsync(UpdateDiscountCouponDto updateCouponDto);
        Task DeleteCouponAsync(int id);
        Task<GetByIdDiscountCouponDto> GetByIdCouponAsync(int id);
    }
}
