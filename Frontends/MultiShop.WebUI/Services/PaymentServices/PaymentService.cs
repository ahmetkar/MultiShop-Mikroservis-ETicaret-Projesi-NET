using MultiShop.DtoLayer.PaymentDtos;
using System.Net.Http;

namespace MultiShop.WebUI.Services.PaymentServices
{
    public class PaymentResultModel
    {
        public bool success { get; set; }
    }


    public class PaymentService : IPaymentService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpClient _httpClient;
        public PaymentService(
       IHttpClientFactory httpClientFactory,
       IHttpContextAccessor httpContextAccessor, HttpClient httpClient)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
            _httpClient = httpClient;
        }

        private HttpClient GetPaymentClient()
        {
            var isAuthenticated =
                _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated == true;

            if (isAuthenticated)
            {
                return _httpClientFactory.CreateClient("PaymentUserClient");
            }

            return _httpClientFactory.CreateClient("PaymentClientCredentialClient");
        }

        public async Task<bool> AddPayment(CreatePaymentDto createPaymentDto)
        {
           var client = GetPaymentClient();
            
           var result =  await client.PostAsJsonAsync<CreatePaymentDto>("Payments", createPaymentDto);
            if (result.IsSuccessStatusCode)
            {
                var res = await result.Content.ReadFromJsonAsync<PaymentResultModel>();

                if (res.success)
                {
                    return true;
                }else
                {
                    return false;
                }
            }else
                {
                throw new Exception($"Payment API Error: {result.StatusCode} - {result.Content}");
               
            }
               
        }

        public async Task<bool> CancelPaymentByOrderingId(int id)
        {
            var result = await _httpClient.DeleteAsync($"Payments/CancelPaymentByOrderingId/{id}");
            if (result.IsSuccessStatusCode)
            {
                var res = await result.Content.ReadFromJsonAsync<PaymentResultModel>();

                if (res.success)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<ResultPaymentDto> GetPaymentByOrderingId(int id)
        {
            var resp = await _httpClient.GetAsync($"Payments/GetPaymentByOrderingId/{id}");
            var values = await resp.Content.ReadFromJsonAsync<ResultPaymentDto>();
            return values;
        }

        public async Task<List<ResultPaymentDto>> GetPaymentsByUserId(string id)
        {
            var resp = await _httpClient.GetAsync($"Payments/GetPaymentsByUserId/{id}");
            var values = await resp.Content.ReadFromJsonAsync<List<ResultPaymentDto>>();
            return values;
        }
    }
}
