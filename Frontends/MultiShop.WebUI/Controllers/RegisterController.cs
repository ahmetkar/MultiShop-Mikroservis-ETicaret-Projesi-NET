using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MultiShop.DtoLayer.IdentityDtos.RegisterDtos;
using MultiShop.WebUI.Settings;
using System.Text.Json;

namespace MultiShop.WebUI.Controllers
{
    public class RegisterController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ServiceApiSettings _serviceApiSettings;
        private readonly ILogger<RegisterController> _logger;

        public RegisterController(
            IHttpClientFactory httpClientFactory,
            IOptions<ServiceApiSettings> serviceApiSettings,
            ILogger<RegisterController> logger)
        {
            _httpClientFactory = httpClientFactory;
            _serviceApiSettings = serviceApiSettings.Value;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(CreateRegisterDto createRegisterDto)
        {
            if (!ModelState.IsValid)
            {
                return View(createRegisterDto);
            }

            if (createRegisterDto.Password != createRegisterDto.ConfirmPassword)
            {
                ModelState.AddModelError("ConfirmPassword", "Girdiğiniz şifreler birbiriyle uyuşmuyor.");
                return View(createRegisterDto);
            }

            try
            {
                var client = _httpClientFactory.CreateClient();
                var registerUrl = $"{_serviceApiSettings.IdentityServerUrl}/api/Registers";

                var payload = new
                {
                    UserName = createRegisterDto.UserName?.Trim(),
                    Email = createRegisterDto.Email?.Trim(),
                    Name = createRegisterDto.Name?.Trim(),
                    Surname = createRegisterDto.Surname?.Trim(),
                    Password = createRegisterDto.Password
                };

                var response = await client.PostAsJsonAsync(registerUrl, payload);

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Kayıt işleminiz başarıyla tamamlandı. Şimdi giriş yapabilirsiniz.";
                    return RedirectToAction("Login", "Auth", new { registered = "true" });
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogWarning("Kayıt başarısız: {Status} - {Content}", response.StatusCode, errorContent);

                try
                {
                    using var doc = JsonDocument.Parse(errorContent);
                    if (doc.RootElement.ValueKind == JsonValueKind.Array)
                    {
                        foreach (var error in doc.RootElement.EnumerateArray())
                        {
                            var code = error.TryGetProperty("code", out var cp) ? cp.GetString() : (error.TryGetProperty("Code", out var c2) ? c2.GetString() : "");
                            var desc = error.TryGetProperty("description", out var dp) ? dp.GetString() : (error.TryGetProperty("Description", out var d2) ? d2.GetString() : "");
                            ModelState.AddModelError("", TranslateIdentityError(code, desc));
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Kayıt işlemi gerçekleştirilemedi.");
                    }
                }
                catch
                {
                    ModelState.AddModelError("", "Kayıt işlemi gerçekleştirilemedi: " + (string.IsNullOrWhiteSpace(errorContent) ? "Bilinmeyen hata" : errorContent));
                }

                return View(createRegisterDto);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Kayıt sırasında IdentityServer bağlantı hatası oluştu.");
                ModelState.AddModelError("", "Kimlik doğrulama sunucusuna bağlanılamadı.");
                return View(createRegisterDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kayıt sırasında hata oluştu.");
                ModelState.AddModelError("", "Kayıt sırasında beklenmeyen bir hata oluştu: " + ex.Message);
                return View(createRegisterDto);
            }
        }

        private static string TranslateIdentityError(string? code, string? defaultDescription)
        {
            return code switch
            {
                "DuplicateUserName" => "Bu kullanıcı adı zaten başka bir kullanıcı tarafından kullanılıyor.",
                "DuplicateEmail" => "Bu e-posta adresi ile kayıtlı başka bir hesap bulunmaktadır.",
                "InvalidUserName" => "Kullanıcı adı yalnızca harf ve rakam içerebilir.",
                "InvalidEmail" => "Lütfen geçerli bir e-posta adresi formatı giriniz.",
                "PasswordTooShort" => "Şifreniz en az 6 karakter uzunluğunda olmalıdır.",
                "PasswordRequiresNonAlphanumeric" => "Şifreniz en az bir özel karakter (*, -, @, ! vb.) içermelidir.",
                "PasswordRequiresDigit" => "Şifreniz en az bir rakam (0-9) içermelidir.",
                "PasswordRequiresLower" => "Şifreniz en az bir küçük harf (a-z) içermelidir.",
                "PasswordRequiresUpper" => "Şifreniz en az bir büyük harf (A-Z) içermelidir.",
                _ => !string.IsNullOrWhiteSpace(defaultDescription) ? defaultDescription : "Kayıt sırasında bir hata oluştu."
            };
        }
    }
}
