using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MultiShop.WebUI.Services
{
    public class HttpService : IHttpService
    {

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private string _url = "";
        private IConfigurationSection urls;

        public HttpService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            urls = _configuration.GetSection("ApiUrls");
        }

        public void setUrl(string kind)
        {
            var url = urls.GetValue<string>(kind);
            if (_url != null)
            {
                _url = url;
            }
        }


        private async Task<string> GetTokenForVisitor()
        {

            string token = "";
            using (var httpClient = new HttpClient())
            {
                var request = new HttpRequestMessage
                {
                    RequestUri = new Uri("http://localhost:5001/connect/token"),
                    Method = HttpMethod.Post,
                    Content = new FormUrlEncodedContent(new Dictionary<string, string>
                    {
                        {"client_id","MultiShopVisitorId" },
                        {"client_secret","multishopsecret" },
                        { "grant_type", "client_credentials" }

                    })
                };


                using (var res = await httpClient.SendAsync(request))
                {
                    if (res.IsSuccessStatusCode)
                    {
                        var content = await res.Content.ReadAsStringAsync();
                        var tokenResponse = JObject.Parse(content);
                        token = tokenResponse["access_token"]?.ToString() ?? string.Empty;
                    }
                }
            }

            return token;
        }

        public async Task<List<T>?> Get<T>(string endpoint)
        {

        
            var token = await GetTokenForVisitor();



            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync(_url + endpoint);
            if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<T>>(jsonData);
                return values;
            }

            return null;

        }

        public async Task<T?> GetOne<T>(string endpoint)
        {

            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync(_url + endpoint);
            if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<T>(jsonData);
                return values;
            }

            return default;
        }




        public async Task<bool> Create<T>(string endpoint, T dto)
        {
            var client = _httpClientFactory.CreateClient();
            var jsonData = JsonConvert.SerializeObject(dto);
            StringContent stringContent = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");
            var response = await client.PostAsync(_url + endpoint, stringContent);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            return false;

        }



        public async Task<bool> DeleteById(string endpoint, string id)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.DeleteAsync($"{_url}{endpoint}?id={id}");
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;

        }


        public async Task<T?> GetById<T>(string endpoint, string id)
        {

            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"{_url}{endpoint}/{id}");
            if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<T>(jsonData);
                return values;
            }
            return default;

        }


        public async Task<bool> Update<T>(string endpoint, T dto)
        {
            var client = _httpClientFactory.CreateClient();
            var jsonData = JsonConvert.SerializeObject(dto);
            StringContent stringContent = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");
            var response = await client.PutAsync(_url + endpoint, stringContent);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            return false;


        }
    }
}
