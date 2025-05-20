using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;

namespace MultiShop.WebUI.Areas.Admin.Services
{
    public class HttpService : IHttpService
    {

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private string _url = "";

        public HttpService(IHttpClientFactory httpClientFactory,IConfiguration configuration) {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public void setUrl(string kind)
        {
            var urls = _configuration.GetSection("ApiUrls");
            if (kind == "CatalogApi")
            {
                var url = urls.GetValue<string>("CatalogApiUrl");
                if(_url != null)
                {
                    _url = url;
                }
            }
        }
           
           public async Task<List<T>?> Get<T>(string endpoint)
           {
         
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync(_url+endpoint);
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

                return default(T);
            }


         

            public async Task<bool> Create<T>(string endpoint,T dto)
            {
                var client = _httpClientFactory.CreateClient();
                var jsonData = JsonConvert.SerializeObject(dto);
                StringContent stringContent = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync(_url+endpoint, stringContent);
                if (response.IsSuccessStatusCode)
                {
                return true;  
                }

            return false;
            
            }


         
            public async Task<bool> DeleteById(string endpoint,string id)
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.DeleteAsync($"{_url}{endpoint}?id={id}");
                if (response.IsSuccessStatusCode)
                {
                return true;
                }
                return false;
            
            }


            public async Task<T?> GetById<T>(string endpoint,string id)
            {
             
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync($"{_url}{endpoint}/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var jsonData = await response.Content.ReadAsStringAsync();
                    var values = JsonConvert.DeserializeObject<T>(jsonData);
                    return values;
                }
                 return default(T);
             
            }

         
            public async Task<bool> Update<T>(string endpoint,T dto)
            {
                var client = _httpClientFactory.CreateClient();
                var jsonData = JsonConvert.SerializeObject(dto);
                StringContent stringContent = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");
                var response = await client.PutAsync(_url+endpoint, stringContent);

                if (response.IsSuccessStatusCode)
                {
                return true;  
                }

                return false;

              
            }
        }
}
