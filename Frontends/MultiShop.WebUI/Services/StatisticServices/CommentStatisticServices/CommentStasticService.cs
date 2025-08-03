
using System.Net.Http;

namespace MultiShop.WebUI.Services.StatisticServices.CommentStatisticServices
{
    public class CommentStasticService : ICommentStasticService
    {
        private readonly HttpClient _httpClient;
        public CommentStasticService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<int> GetActiveCommentCount()
        {
            var resp = await _httpClient.GetAsync("comments/GetActiveCommentCount");
            var value = await resp.Content.ReadFromJsonAsync<int>();
            return value;
        }

        public async Task<int> GetTotalCommentCount()
        {
            var resp = await _httpClient.GetAsync("comments/GetTotalCommentCount");
            var value = await resp.Content.ReadFromJsonAsync<int>();
            return value;
        }
    }
}
