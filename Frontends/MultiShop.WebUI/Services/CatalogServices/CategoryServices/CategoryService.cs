using MultiShop.DtoLayer.CatalogDtos.CategoryDtos;
using Newtonsoft.Json;

namespace MultiShop.WebUI.Services.CatalogServices.CategoryServices
{
    public class CategoryService : ICategoryService
    {

        private readonly HttpClient _httpClient;

        public CategoryService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task CreateCatagoryAsync(CreateCategoryDto createCategoryDto)
        {
            await _httpClient.PostAsJsonAsync<CreateCategoryDto>("categories",createCategoryDto);
            
        }

        public async Task DeleteCategoryAsync(string id)
        {
            await _httpClient.DeleteAsync("categories?id="+id);
        }

        public async Task<List<ResultCategoryDto>> GetAllCategoryAsync()
        {
           var resp =  await _httpClient.GetAsync("categories");
           var values = await resp.Content.ReadFromJsonAsync<List<ResultCategoryDto>>();  
           return values;
        }

        public async Task<GetByIdCategoryDto> GetByIdCategory(string id)
        {
            var resp = await _httpClient.GetAsync("categories/"+id);
            var values = await resp.Content.ReadFromJsonAsync<GetByIdCategoryDto>();
            return values;

        }

        public async Task UpdateCategoryAsync(UpdateCategoryDto updateCategoryDto)
        {
            await _httpClient.PutAsJsonAsync<UpdateCategoryDto>("categories",updateCategoryDto);

        }
    }
}
