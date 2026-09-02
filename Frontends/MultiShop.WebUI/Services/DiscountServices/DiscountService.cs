using MultiShop.DtoLayer.DiscountDtos;

namespace MultiShop.WebUI.Services.DiscountServices
{
    public class DiscountService : IDiscountService
    {
        private readonly HttpClient _httpClient;

        public DiscountService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<GetDiscountCodeDetailByCode> GetDiscountCode(string code)
        {
            var responseMessage = await _httpClient.GetAsync($"discounts/GetCodeDetailByCode/{code}");
            var values = await responseMessage.Content.ReadFromJsonAsync<GetDiscountCodeDetailByCode>();
            return values;
        }

        public async Task<int> GetDiscountCouponCountRate(string code)
        {
            var responseMessage = await _httpClient.GetAsync($"discounts/GetDiscountCouponCountRate/{code}");
            var value = await responseMessage.Content.ReadFromJsonAsync<int>();
            return value;
        }

        public async Task<List<ResultDiscountCouponDto>> GetAllCouponAsync()
        {
            var response = await _httpClient.GetAsync("discounts");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<ResultDiscountCouponDto>>() ?? new List<ResultDiscountCouponDto>();
            }
            return new List<ResultDiscountCouponDto>();
        }

        public async Task CreateCouponAsync(CreateDiscountCouponDto createCouponDto)
        {
            await _httpClient.PostAsJsonAsync("discounts", createCouponDto);
        }

        public async Task UpdateCouponAsync(UpdateDiscountCouponDto updateCouponDto)
        {
            await _httpClient.PutAsJsonAsync("discounts", updateCouponDto);
        }

        public async Task DeleteCouponAsync(int id)
        {
            await _httpClient.DeleteAsync("discounts?id=" + id);
        }

        public async Task<GetByIdDiscountCouponDto> GetByIdCouponAsync(int id)
        {
            var response = await _httpClient.GetAsync("discounts/" + id);
            return await response.Content.ReadFromJsonAsync<GetByIdDiscountCouponDto>();
        }

        public async Task<ResultDiscountCouponDto?> GetDiscountByProductIdAsync(string productId)
        {
            var response = await _httpClient.GetAsync("discounts/GetDiscountByProductId/" + productId);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ResultDiscountCouponDto>();
            }
            return null;
        }

        public async Task<List<ResultDiscountCouponDto>> GetActiveProductDiscountsAsync()
        {
            var response = await _httpClient.GetAsync("discounts/GetActiveProductDiscounts");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<ResultDiscountCouponDto>>() ?? new List<ResultDiscountCouponDto>();
            }
            return new List<ResultDiscountCouponDto>();
        }

        public async Task SetProductDiscountAsync(string productId, int rate, DateTime validDate, bool isActive)
        {
            var dto = new CreateDiscountCouponDto
            {
                ProductId = productId,
                Rate = rate,
                ValidDate = validDate,
                IsActive = isActive
            };
            await _httpClient.PostAsJsonAsync("discounts/SetProductDiscount", dto);
        }

        public async Task DeleteDiscountByProductIdAsync(string productId)
        {
            await _httpClient.DeleteAsync("discounts/DeleteDiscountByProductId/" + productId);
        }
    }
}
