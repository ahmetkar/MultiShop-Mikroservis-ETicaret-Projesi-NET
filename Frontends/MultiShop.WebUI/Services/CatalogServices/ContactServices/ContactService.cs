using MultiShop.DtoLayer.CatalogDtos.ContactDTOs;

namespace MultiShop.WebUI.Services.CatalogServices.ContactServices
{
    public class ContactService : IContactService
    {
        private readonly HttpClient _httpClient;

        public ContactService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task CreateContactAsync(CreateContactDto createContactDto)
        {
            await _httpClient.PostAsJsonAsync<CreateContactDto>("contacts", createContactDto);

        }

        public async Task DeleteContactAsync(string id)
        {
            await _httpClient.DeleteAsync("contacts?id=" + id);
        }

        public async Task<List<ResultContactDto>> GetAllContactAsync()
        {
            var resp = await _httpClient.GetAsync("contacts");
            var values = await resp.Content.ReadFromJsonAsync<List<ResultContactDto>>();
            return values;
        }

        public async Task<GetByIdContactDto> GetByIdContact(string id)
        {
            var resp = await _httpClient.GetAsync("contacts/" + id);
            var values = await resp.Content.ReadFromJsonAsync<GetByIdContactDto>();
            return values;

        }

       
    }
}
