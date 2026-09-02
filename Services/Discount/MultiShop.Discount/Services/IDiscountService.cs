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
        Task<ResultDiscountCouponDto> GetCodeDetailByCode(string code);
        int GetDiscountCouponCountRate(string code);
        Task<int> GetDiscountCouponCount();
        Task<ResultDiscountCouponDto?> GetDiscountByProductIdAsync(string productId);
        Task<List<ResultDiscountCouponDto>> GetActiveProductDiscountsAsync();
        Task SetProductDiscountAsync(string productId, int rate, DateTime validDate, bool isActive);
        Task DeleteDiscountByProductIdAsync(string productId);
    }
}

