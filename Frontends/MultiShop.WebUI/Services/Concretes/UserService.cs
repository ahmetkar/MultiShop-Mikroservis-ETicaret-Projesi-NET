using MultiShop.WebUI.Models;
using MultiShop.WebUI.Services.Interfaces;
using System.Text.Json;

namespace MultiShop.WebUI.Services.Concretes
{
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;

        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<UserDetailViewModel> GetUserInfo()
        {
            var result = await _httpClient.GetFromJsonAsync<UserDetailViewModel>("/api/users/getuserinfo");
            return result ?? new UserDetailViewModel();
        }

        public async Task<string> GetUserId()
        {
            return await _httpClient.GetStringAsync("/api/users/getuserid");
        }

        public async Task<bool> UpdateUserInfo(UserDetailViewModel userDetailViewModel)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/users/updateuserinfo", userDetailViewModel);
            return response.IsSuccessStatusCode;
        }

        public async Task<(bool Success, string Message)> ChangePassword(string currentPassword, string newPassword)
        {
            var payload = new
            {
                CurrentPassword = currentPassword,
                NewPassword = newPassword
            };

            var response = await _httpClient.PostAsJsonAsync("/api/users/changepassword", payload);
            if (response.IsSuccessStatusCode)
            {
                return (true, "Şifreniz başarıyla değiştirildi.");
            }

            var content = await response.Content.ReadAsStringAsync();
            try
            {
                using var doc = JsonDocument.Parse(content);
                if (doc.RootElement.ValueKind == JsonValueKind.Array)
                {
                    var errors = new List<string>();
                    foreach (var err in doc.RootElement.EnumerateArray())
                    {
                        var desc = err.TryGetProperty("description", out var dp) ? dp.GetString() : (err.TryGetProperty("Description", out var d2) ? d2.GetString() : "");
                        if (!string.IsNullOrWhiteSpace(desc))
                        {
                            if (desc.Contains("Incorrect password", StringComparison.OrdinalIgnoreCase) || desc.Contains("PasswordMismatch", StringComparison.OrdinalIgnoreCase))
                            {
                                errors.Add("Mevcut şifreniz hatalı.");
                            }
                            else
                            {
                                errors.Add(desc);
                            }
                        }
                    }
                    if (errors.Count > 0)
                    {
                        return (false, string.Join(" ", errors));
                    }
                }
            }
            catch { }

            return (false, "Şifre değiştirilemedi. Lütfen mevcut şifrenizi kontrol ediniz.");
        }
    }
}
