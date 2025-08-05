namespace MultiShop.SignalRRealTimeApi.Services.SignalRCommentServices
{
    public class SignalRCommentService : ISignalRCommentService
    {
        private readonly HttpClient _httpClient;
        public SignalRCommentService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<int> GetTotalCommentCount()
        {
            var resp = await _httpClient.GetAsync("comments/GetTotalCommentCount");
            var value = await resp.Content.ReadFromJsonAsync<int>();
            return value;
        }
    }
}
