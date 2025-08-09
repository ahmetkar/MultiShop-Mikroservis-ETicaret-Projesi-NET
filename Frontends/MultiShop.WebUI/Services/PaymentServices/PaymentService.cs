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
        

        private readonly HttpClient _httpClient;

        public PaymentService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public async Task<bool> AddPayment(CreatePaymentDto createPaymentDto)
        {
           var result =  await _httpClient.PostAsJsonAsync<CreatePaymentDto>("Payments", createPaymentDto);
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
