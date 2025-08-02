using MultiShop.DtoLayer.IdentityDtos.UserDtos;

namespace MultiShop.WebUI.Services.UserIdentityServices
{
    public class UserIdentityService : IUserIdentityService
    {
        private readonly HttpClient _httpClient;

        public UserIdentityService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ResultUserDto>> GetAllUserListAsync()
        {   
            var resp = await _httpClient.GetAsync("api/users/GetAllUserList");
            var values = await resp.Content.ReadFromJsonAsync<List<ResultUserDto>>();
            return values;
        }
    }
}
