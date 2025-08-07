using Microsoft.AspNetCore.Mvc;
using MultiShop.RapidApi.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text.Json.Serialization;

namespace MultiShop.RapidApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RapidApiController : Controller
    {
        private IConfiguration Configuration { get; }
        public string ApiKey;

        public RapidApiController(IConfiguration configuration)
        {
            Configuration = configuration;

            ApiKey = Configuration["RapidApiKey"]!;
        }

        string replaceQueryStr(string query)
        {
            return query.Replace(" ", "%20").Replace("ı", "i").Replace("ğ", "g").Replace("ç", "c").Replace("ş", "s");
        }


        [HttpGet("WeatherDetail/{query}")]
        public async Task<IActionResult> WeatherDetail(string query)
        {

            string replacedQuery = replaceQueryStr(query);

            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://open-weather13.p.rapidapi.com/city?city="+replacedQuery+"&lang=TR"),
                Headers =
                {
                        { "x-rapidapi-key", ApiKey },
                        { "x-rapidapi-host", "open-weather13.p.rapidapi.com" },
                },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<WeatherViewModel>(body);
                //ViewBag.citytempr = values.main.temp;

                return Ok(values);
            }
        }

        public async Task<IActionResult> Exchange([FromQuery]string from,[FromQuery]string to)
        {
            string queryfrom = replaceQueryStr(from);
            string queryto = replaceQueryStr(to);
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://real-time-finance-data.p.rapidapi.com/currency-exchange-rate?from_symbol="+queryfrom+"&to_symbol="+queryto+"&language=en"),
                Headers =
                {
                    { "x-rapidapi-key", ApiKey },
                    { "x-rapidapi-host", "real-time-finance-data.p.rapidapi.com" },
                },
              };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<ExchangeViewModel>(body);
                //ViewBag.exchgRate = values.data.exchange_rate;
                //ViewBag.prevClose = values.data.previous_close;
                return Ok(values);
            }
        }



        [HttpGet("ProductSearch/{query}")]
        public async Task<IActionResult> ProductSearch(string query)
        {
            string replacedQuery = replaceQueryStr(query);

            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://real-time-product-search.p.rapidapi.com/search-light-v2?q="+replacedQuery+"&country=tr&language=tr&page=1&limit=10&sort_by=BEST_MATCH&product_condition=ANY&return_filters=false"),
                Headers =
                    {
                        { "x-rapidapi-key", ApiKey },
                        { "x-rapidapi-host", "real-time-product-search.p.rapidapi.com" },
                    },
                            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<ProductViewModel>(body);
                return Ok(values);
            }
            
        }

        }
    }
