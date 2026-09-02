using MultiShop.DtoLayer.DiscountDtos;

namespace MultiShop.WebUI.Services.DiscountServices
{
    public interface IDiscountService
    {
        Task<GetDiscountCodeDetailByCode> GetDiscountCode(string code);
        Task<int> GetDiscountCouponCountRate(string code);
        Task<List<ResultDiscountCouponDto>> GetAllCouponAsync();
        Task CreateCouponAsync(CreateDiscountCouponDto createCouponDto);
        Task UpdateCouponAsync(UpdateDiscountCouponDto updateCouponDto);
        Task DeleteCouponAsync(int id);
        Task<GetByIdDiscountCouponDto> GetByIdCouponAsync(int id);
        Task<ResultDiscountCouponDto?> GetDiscountByProductIdAsync(string productId);
        Task<List<ResultDiscountCouponDto>> GetActiveProductDiscountsAsync();
        Task SetProductDiscountAsync(string productId, int rate, DateTime validDate, bool isActive);
        Task DeleteDiscountByProductIdAsync(string productId);
    }
}
