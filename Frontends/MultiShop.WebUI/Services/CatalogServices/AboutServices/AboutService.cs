﻿using MultiShop.DtoLayer.CatalogDtos.AboutDtos;

namespace MultiShop.WebUI.Services.CatalogServices.AboutServices
{
    public class AboutService : IAboutService
    {

        private readonly HttpClient _httpClient;

        public AboutService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task CreateAboutAsync(CreateAboutDto createAboutDto)
        {
            await _httpClient.PostAsJsonAsync<CreateAboutDto>("abouts", createAboutDto);

        }

        public async Task DeleteAboutAsync(string id)
        {
            await _httpClient.DeleteAsync("abouts?id=" + id);
        }

        public async Task<List<ResultAboutDto>> GetAllAboutAsync()
        {
            var resp = await _httpClient.GetAsync("abouts/AboutList");
            var values = await resp.Content.ReadFromJsonAsync<List<ResultAboutDto>>();
            return values;
        }

        public async Task<UpdateAboutDto> GetByIdAbout(string id)
        {
            var resp = await _httpClient.GetAsync("abouts/" + id);
            var values = await resp.Content.ReadFromJsonAsync<UpdateAboutDto>();
            return values;

        }

        public async Task<ResultAboutDto> GetLastAboutAsync()
        {
            var resp = await _httpClient.GetAsync("abouts/GetLastAbout");
            var values = await resp.Content.ReadFromJsonAsync<ResultAboutDto>();
            return values;
        }

        public async Task UpdateAboutAsync(UpdateAboutDto updateAboutDto)
        {
            await _httpClient.PutAsJsonAsync<UpdateAboutDto>("abouts", updateAboutDto);

        }

    }
}
