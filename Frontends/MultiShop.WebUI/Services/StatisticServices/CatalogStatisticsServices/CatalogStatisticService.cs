
namespace MultiShop.WebUI.Services.StatisticServices.CatalogStatisticsServices
{
    public class CatalogStatisticService : ICatalogStatisticService
    {
        private readonly HttpClient _httpClient;
        public CatalogStatisticService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<long> GetBrandCount()
        {
            var resp = await _httpClient.GetAsync("Statistics/GetBrandCount");
            var value = await resp.Content.ReadFromJsonAsync<long>();
            return value;
        }   

        public async Task<long> GetCategoryCount()
        {
            var resp = await _httpClient.GetAsync("Statistics/GetCategoryCount");
            var value = await resp.Content.ReadFromJsonAsync<long>();
            return value;
        }

        public async Task<string> GetMaxPriceProductName()
        {
            var resp = await _httpClient.GetAsync("Statistics/GetMaxPriceProductName");
            var value = await resp.Content.ReadAsStringAsync();
            return value;
        }

        public async Task<string> GetMinPriceProductName()
        {
            var resp = await _httpClient.GetAsync("Statistics/GetMinPriceProductName");
            var value = await resp.Content.ReadAsStringAsync();
            return value;
        }

        public async Task<decimal> GetProductAvgPrice()
        {
            var resp = await _httpClient.GetAsync("Statistics/GetProductAvgPrice");
            var value = await resp.Content.ReadFromJsonAsync<decimal>();
            return value;
        }

        public async Task<long> GetProductCount()
        {
            var resp = await _httpClient.GetAsync("Statistics/GetProductCount");
            var value = await resp.Content.ReadFromJsonAsync<long>();
            return value;
        }
    }
}
