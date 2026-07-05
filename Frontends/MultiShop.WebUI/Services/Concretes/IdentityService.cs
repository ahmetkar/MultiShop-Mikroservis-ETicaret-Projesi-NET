using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using MultiShop.DtoLayer.IdentityDtos.LoginDtos;
using MultiShop.WebUI.Services.Interfaces;
using MultiShop.WebUI.Settings;
using System.Security.Claims;

namespace MultiShop.WebUI.Services.Concretes
{
    public class IdentityService : IIdentityService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ClientSettings _clientSettings;
        private readonly ServiceApiSettings _serviceApiSettings;
        private readonly ILogger<IdentityService> _logger;

        public IdentityService(
            HttpClient httpClient,
            IHttpContextAccessor httpContextAccessor,
            IOptions<ClientSettings> clientSettings,
            IOptions<ServiceApiSettings> serviceApiSettings,
            ILogger<IdentityService> logger)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            _clientSettings = clientSettings.Value;
            _serviceApiSettings = serviceApiSettings.Value;
            _logger = logger;
        }

        public async Task<string?> GetRefreshToken()
        {
            var discoveryEndpoint = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = _serviceApiSettings.IdentityServerUrl,
                Policy = new DiscoveryPolicy
                {
                    RequireHttps = false
                }
            });

            if (discoveryEndpoint.IsError)
            {
                throw new Exception($"Discovery error: {discoveryEndpoint.Error}");
            }

            var refreshToken = await _httpContextAccessor.HttpContext!
                .GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);

            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                throw new Exception("Refresh token bulunamadı.");
            }

            var refreshTokenRequest = new RefreshTokenRequest
            {
                ClientId = _clientSettings.MultiShopManagerId.ClientId,
                ClientSecret = _clientSettings.MultiShopManagerId.ClientSecret,
                RefreshToken = refreshToken,
                Address = discoveryEndpoint.TokenEndpoint
            };

            var token = await _httpClient.RequestRefreshTokenAsync(refreshTokenRequest);

            if (token.IsError)
            {
                throw new Exception($"Refresh token error: {token.Error} - {token.ErrorDescription}");
            }

            if (string.IsNullOrWhiteSpace(token.AccessToken))
            {
                throw new Exception("Refresh sonrası access token boş geldi.");
            }

            var newRefreshToken = !string.IsNullOrWhiteSpace(token.RefreshToken)
                ? token.RefreshToken
                : refreshToken;

            var authToken = new List<AuthenticationToken>
    {
        new AuthenticationToken
        {
            Name = OpenIdConnectParameterNames.AccessToken,
            Value = token.AccessToken
        },
        new AuthenticationToken
        {
            Name = OpenIdConnectParameterNames.RefreshToken,
            Value = newRefreshToken
        },
        new AuthenticationToken
        {
            Name = OpenIdConnectParameterNames.ExpiresIn,
            Value = DateTime.Now.AddSeconds(token.ExpiresIn).ToString("O")
        }
    };

            var result = await _httpContextAccessor.HttpContext.AuthenticateAsync(
                CookieAuthenticationDefaults.AuthenticationScheme
            );

            if (!result.Succeeded || result.Principal == null || result.Properties == null)
            {
                throw new Exception("Mevcut authentication bilgisi okunamadı.");
            }

            result.Properties.StoreTokens(authToken);

            await _httpContextAccessor.HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                result.Principal,
                result.Properties
            );

            return token.AccessToken;
        }

        public async Task<bool> SignIn(SignInDto signInDto)
        {
            try
            {
                if (signInDto == null)
                {
                    throw new ArgumentNullException(nameof(signInDto), "Giriş bilgileri boş olamaz.");
                }

                if (string.IsNullOrWhiteSpace(signInDto.Username))
                {
                    throw new ArgumentException("Kullanıcı adı boş olamaz.");
                }

                if (string.IsNullOrWhiteSpace(signInDto.Password))
                {
                    throw new ArgumentException("Şifre boş olamaz.");
                }

                var httpContext = _httpContextAccessor.HttpContext;

                if (httpContext == null)
                {
                    throw new InvalidOperationException("HttpContext bulunamadı.");
                }

                var discoveryEndpoint = await GetDiscoveryDocumentOrThrowAsync();

                if (string.IsNullOrWhiteSpace(discoveryEndpoint.UserInfoEndpoint))
                {
                    throw new InvalidOperationException("UserInfoEndpoint bulunamadı.");
                }

                var passwordTokenRequest = new PasswordTokenRequest
                {
                    ClientId = GetClientId(),
                    ClientSecret = GetClientSecret(),
                    UserName = signInDto.Username,
                    Password = signInDto.Password,
                    Address = discoveryEndpoint.TokenEndpoint,

                    // Refresh token alabilmek için offline_access gerekir.
                    // IdentityServer tarafında client AllowOfflineAccess = true olmalı.
                    Scope = "openid profile email offline_access IdentityServerApi BasketFullPermission OcelotFullPermission CatalogFullPermission DiscountFullPermission OrderFullPermission PaymentFullPermission"
                };

                var token = await _httpClient.RequestPasswordTokenAsync(passwordTokenRequest);

                if (token.IsError)
                {
                    throw new InvalidOperationException(
                        $"Token error: {token.Error} - {token.ErrorDescription}"
                    );
                }

                if (string.IsNullOrWhiteSpace(token.AccessToken))
                {
                    throw new InvalidOperationException("Access token boş geldi.");
                }

                if (string.IsNullOrWhiteSpace(token.RefreshToken))
                {
                    _logger.LogWarning(
                        "Refresh token boş geldi. offline_access scope veya IdentityServer client ayarları eksik olabilir."
                    );
                }

                var userInfo = new UserInfoRequest
                {
                    Address = discoveryEndpoint.UserInfoEndpoint,
                    Token = token.AccessToken
                };

                var userValues = await _httpClient.GetUserInfoAsync(userInfo);

                if (userValues.IsError)
                {
                    throw new InvalidOperationException(
                        $"UserInfo error: {userValues.Error}"
                    );
                }

                var claims = userValues.Claims?.ToList();

                if (claims == null || claims.Count == 0)
                {
                    throw new InvalidOperationException("UserInfo endpointinden claim bilgisi gelmedi.");
                }

                var claimsIdentity = new ClaimsIdentity(
                    claims,
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    "name",
                    "role"
                );

                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = signInDto.RememberMe
                };

                authProperties.StoreTokens(CreateAuthenticationTokens(
                    token.AccessToken,
                    token.RefreshToken,
                    token.ExpiresIn
                ));

                await httpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    claimsPrincipal,
                    authProperties
                );

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SignIn işlemi sırasında hata oluştu.");
                throw;
            }
        }

        public async Task SignOut()
        {
            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext == null)
            {
                _logger.LogWarning("SignOut sırasında HttpContext bulunamadı.");
                return;
            }

            await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        private async Task<DiscoveryDocumentResponse> GetDiscoveryDocumentOrThrowAsync()
        {
            if (string.IsNullOrWhiteSpace(_serviceApiSettings.IdentityServerUrl))
            {
                throw new InvalidOperationException("IdentityServerUrl ayarı boş.");
            }

            var discoveryEndpoint = await _httpClient.GetDiscoveryDocumentAsync(
                new DiscoveryDocumentRequest
                {
                    Address = _serviceApiSettings.IdentityServerUrl,
                    Policy = new DiscoveryPolicy
                    {
                        RequireHttps = false
                    }
                });

            if (discoveryEndpoint.IsError)
            {
                throw new InvalidOperationException(
                    $"Discovery error: {discoveryEndpoint.Error}"
                );
            }

            if (string.IsNullOrWhiteSpace(discoveryEndpoint.TokenEndpoint))
            {
                throw new InvalidOperationException("TokenEndpoint bulunamadı.");
            }

            return discoveryEndpoint;
        }

        private string GetClientId()
        {
            var clientId = _clientSettings.MultiShopManagerId.ClientId;

            if (string.IsNullOrWhiteSpace(clientId))
            {
                throw new InvalidOperationException("ClientId ayarı boş.");
            }

            return clientId;
        }

        private string GetClientSecret()
        {
            var clientSecret = _clientSettings.MultiShopManagerId.ClientSecret;

            if (string.IsNullOrWhiteSpace(clientSecret))
            {
                throw new InvalidOperationException("ClientSecret ayarı boş.");
            }

            return clientSecret;
        }

        private static List<AuthenticationToken> CreateAuthenticationTokens(
            string accessToken,
            string? refreshToken,
            int expiresIn)
        {
            var tokens = new List<AuthenticationToken>
            {
                new AuthenticationToken
                {
                    Name = OpenIdConnectParameterNames.AccessToken,
                    Value = accessToken
                },
                new AuthenticationToken
                {
                    Name = OpenIdConnectParameterNames.ExpiresIn,
                    Value = DateTime.Now.AddSeconds(expiresIn).ToString("O")
                }
            };

            if (!string.IsNullOrWhiteSpace(refreshToken))
            {
                tokens.Add(new AuthenticationToken
                {
                    Name = OpenIdConnectParameterNames.RefreshToken,
                    Value = refreshToken
                });
            }

            return tokens;
        }
    }
}