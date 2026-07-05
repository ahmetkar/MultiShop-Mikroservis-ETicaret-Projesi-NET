using MultiShop.DtoLayer.CatalogDtos.AboutDtos;
using System.Text.Json;

namespace MultiShop.WebUI.Services.CatalogServices.AboutServices
{
    public class AboutService : IAboutService
    {
       

        private readonly HttpClient _httpClient;

        public AboutService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

     

        public async Task<ResultAboutDto> GetAbout()
        {
            var resp = await _httpClient.GetAsync("abouts");

            if (resp.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return new ResultAboutDto();
            }

            if (!resp.IsSuccessStatusCode)
            {
                return new ResultAboutDto();
            }


            var values = await resp.Content.ReadFromJsonAsync<ResultAboutDto>();

            if(values == null)
            {
                return new ResultAboutDto();
            }
           
            return values;

        }

        
        public async Task UpdateAboutAsync(UpdateAboutDto updateAboutDto)
        {
            await _httpClient.PostAsJsonAsync<UpdateAboutDto>("abouts", updateAboutDto);

        }

    }
}
