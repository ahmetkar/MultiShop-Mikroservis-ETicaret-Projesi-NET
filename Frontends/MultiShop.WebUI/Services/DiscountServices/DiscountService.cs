using MultiShop.DtoLayer.DiscountDtos;
using System.Text.Json;

namespace MultiShop.WebUI.Services.DiscountServices
{
    public class DiscountService : IDiscountService
    {
        private readonly HttpClient _httpClient;
        private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public DiscountService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<GetDiscountCodeDetailByCode> GetDiscountCode(string code)
        {
            try
            {
                var responseMessage = await _httpClient.GetAsync($"discounts/GetCodeDetailByCode/{code}");
                if (responseMessage.IsSuccessStatusCode)
                {
                    var content = await responseMessage.Content.ReadAsStringAsync();
                    if (!string.IsNullOrWhiteSpace(content) && content != "null")
                    {
                        return JsonSerializer.Deserialize<GetDiscountCodeDetailByCode>(content, _jsonOptions);
                    }
                }
            }
            catch { }
            return null;
        }

        public async Task<int> GetDiscountCouponCountRate(string code)
        {
            try
            {
                var responseMessage = await _httpClient.GetAsync($"discounts/GetDiscountCouponCountRate/{code}");
                if (responseMessage.IsSuccessStatusCode)
                {
                    var content = await responseMessage.Content.ReadAsStringAsync();
                    if (int.TryParse(content, out int val))
                    {
                        return val;
                    }
                }
            }
            catch { }
            return 0;
        }

        public async Task<List<ResultDiscountCouponDto>> GetAllCouponAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("discounts");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    if (!string.IsNullOrWhiteSpace(content) && content != "null")
                    {
                        return JsonSerializer.Deserialize<List<ResultDiscountCouponDto>>(content, _jsonOptions) ?? new List<ResultDiscountCouponDto>();
                    }
                }
            }
            catch { }
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
            try
            {
                var response = await _httpClient.GetAsync("discounts/" + id);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    if (!string.IsNullOrWhiteSpace(content) && content != "null")
                    {
                        return JsonSerializer.Deserialize<GetByIdDiscountCouponDto>(content, _jsonOptions);
                    }
                }
            }
            catch { }
            return null;
        }

        public async Task<ResultDiscountCouponDto?> GetDiscountByProductIdAsync(string productId)
        {
            try
            {
                var response = await _httpClient.GetAsync("discounts/GetDiscountByProductId/" + productId);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    if (!string.IsNullOrWhiteSpace(content) && content != "null")
                    {
                        return JsonSerializer.Deserialize<ResultDiscountCouponDto>(content, _jsonOptions);
                    }
                }
            }
            catch { }
            return null;
        }

        public async Task<List<ResultDiscountCouponDto>> GetActiveProductDiscountsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("discounts/GetActiveProductDiscounts");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    if (!string.IsNullOrWhiteSpace(content) && content != "null")
                    {
                        return JsonSerializer.Deserialize<List<ResultDiscountCouponDto>>(content, _jsonOptions) ?? new List<ResultDiscountCouponDto>();
                    }
                }
            }
            catch { }
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
