
namespace MultiShop.WebUI.Services.StatisticServices.UserStatisticsServices
{
    public class UserStatisticService : IUserStatisticService
    {
        private readonly HttpClient _httpClient;
        public UserStatisticService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<int> GetUserCount()
        {
            var resp = await _httpClient.GetAsync("api/Statistics");
            var value = await resp.Content.ReadFromJsonAsync<int>();
            return value;
        }
    }
}
