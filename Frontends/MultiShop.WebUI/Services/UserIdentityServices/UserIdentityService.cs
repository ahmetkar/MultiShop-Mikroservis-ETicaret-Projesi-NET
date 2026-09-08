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
            if (resp.IsSuccessStatusCode)
            {
                var values = await resp.Content.ReadFromJsonAsync<List<ResultUserDto>>();
                return values ?? new List<ResultUserDto>();
            }
            return new List<ResultUserDto>();
        }

        public async Task<ResultUserDto?> GetUserByIdAsync(string id)
        {
            var resp = await _httpClient.GetAsync($"api/users/GetUserById/{id}");
            if (resp.IsSuccessStatusCode)
            {
                return await resp.Content.ReadFromJsonAsync<ResultUserDto>();
            }
            return null;
        }

        public async Task<bool> UpdateUserAsync(AdminUpdateUserDto dto)
        {
            var resp = await _httpClient.PostAsJsonAsync("api/users/AdminUpdateUser", dto);
            return resp.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteUserAsync(string id)
        {
            var resp = await _httpClient.DeleteAsync($"api/users/DeleteUser/{id}");
            return resp.IsSuccessStatusCode;
        }
    }
}
