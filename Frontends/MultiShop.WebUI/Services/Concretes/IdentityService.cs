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
            try
            {
                var httpContext = _httpContextAccessor.HttpContext;
                if (httpContext == null)
                {
                    _logger.LogWarning("GetRefreshToken: HttpContext bulunamadı.");
                    return null;
                }

                var discoveryEndpoint = await GetDiscoveryDocumentOrThrowAsync();

                var refreshToken = await httpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);

                if (string.IsNullOrWhiteSpace(refreshToken))
                {
                    _logger.LogWarning("GetRefreshToken: Refresh token bulunamadı.");
                    return null;
                }

                // Determine client credentials dynamically based on user role (Admin, Manager, User)
                var clientCreds = GetClientCredentialsForUser(httpContext.User);

                var refreshTokenRequest = new RefreshTokenRequest
                {
                    ClientId = clientCreds.ClientId,
                    ClientSecret = clientCreds.ClientSecret,
                    RefreshToken = refreshToken,
                    Address = discoveryEndpoint.TokenEndpoint
                };

                var token = await _httpClient.RequestRefreshTokenAsync(refreshTokenRequest);

                if (token.IsError)
                {
                    _logger.LogWarning("Refresh token yenileme hatası: {Error} - {Description}", token.Error, token.ErrorDescription);
                    return null;
                }

                if (string.IsNullOrWhiteSpace(token.AccessToken))
                {
                    _logger.LogWarning("Refresh sonrası access token boş geldi.");
                    return null;
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
                        Value = DateTime.UtcNow.AddSeconds(token.ExpiresIn).ToString("O")
                    }
                };

                var result = await httpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                if (result.Succeeded && result.Principal != null && result.Properties != null)
                {
                    result.Properties.StoreTokens(authToken);

                    await httpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        result.Principal,
                        result.Properties
                    );
                }

                return token.AccessToken;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetRefreshToken işlemi sırasında hata oluştu.");
                return null;
            }
        }

        public async Task<bool> SignIn(SignInDto signInDto)
        {
            try
            {
                if (signInDto == null)
                {
                    _logger.LogWarning("SignIn: Giriş bilgileri boş.");
                    return false;
                }

                if (string.IsNullOrWhiteSpace(signInDto.Username) || string.IsNullOrWhiteSpace(signInDto.Password))
                {
                    _logger.LogWarning("SignIn: Kullanıcı adı veya şifre boş.");
                    return false;
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

                // Password grant client: use Manager/Admin client (or configured client) for full scope acquisition across all user roles
                var clientCreds = _clientSettings.MultiShopManagerId ?? _clientSettings.MultiShopAdminId ?? _clientSettings.MultiShopUserId;
                if (clientCreds == null || string.IsNullOrWhiteSpace(clientCreds.ClientId))
                {
                    throw new InvalidOperationException("ResourceOwnerPassword Client ayarı bulunamadı.");
                }

                var passwordTokenRequest = new PasswordTokenRequest
                {
                    ClientId = clientCreds.ClientId,
                    ClientSecret = clientCreds.ClientSecret,
                    UserName = signInDto.Username,
                    Password = signInDto.Password,
                    Address = discoveryEndpoint.TokenEndpoint,
                    Scope = "openid profile email roles offline_access IdentityServerApi BasketFullPermission OcelotFullPermission CatalogFullPermission DiscountFullPermission OrderFullPermission CargoFullPermission PaymentFullPermission PaymentCreatePermission PaymentReadPermission PaymentUpdatePermission PaymentDeletePermission ImagesFullPermission MessageFullPermission CommentFullPermission"
                };

                var token = await _httpClient.RequestPasswordTokenAsync(passwordTokenRequest);

                if (token.IsError)
                {
                    _logger.LogWarning("SignIn başarısız: {Error} - {Description}", token.Error, token.ErrorDescription);
                    return false;
                }

                if (string.IsNullOrWhiteSpace(token.AccessToken))
                {
                    _logger.LogWarning("SignIn: Access token boş geldi.");
                    return false;
                }

                var userInfo = new UserInfoRequest
                {
                    Address = discoveryEndpoint.UserInfoEndpoint,
                    Token = token.AccessToken
                };

                var userValues = await _httpClient.GetUserInfoAsync(userInfo);

                var claims = userValues?.Claims?.ToList() ?? new List<Claim>();

                // Parse and map role claims from AccessToken JWT as well
                var jwtHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
                if (jwtHandler.CanReadToken(token.AccessToken))
                {
                    var jwtToken = jwtHandler.ReadJwtToken(token.AccessToken);
                    foreach (var c in jwtToken.Claims)
                    {
                        if (c.Type == "role" || c.Type == ClaimTypes.Role || c.Type == "roles")
                        {
                            if (!claims.Any(x => (x.Type == "role" || x.Type == ClaimTypes.Role) && x.Value == c.Value))
                            {
                                claims.Add(new Claim(ClaimTypes.Role, c.Value));
                            }
                        }
                    }
                }

                // If sub / NameIdentifier is not present, extract from token
                if (!claims.Any(x => x.Type == ClaimTypes.NameIdentifier || x.Type == "sub"))
                {
                    if (jwtHandler.CanReadToken(token.AccessToken))
                    {
                        var jwtToken = jwtHandler.ReadJwtToken(token.AccessToken);
                        var sub = jwtToken.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;
                        if (!string.IsNullOrWhiteSpace(sub))
                        {
                            claims.Add(new Claim(ClaimTypes.NameIdentifier, sub));
                        }
                    }
                }

                var claimsIdentity = new ClaimsIdentity(
                    claims,
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    "name",
                    ClaimTypes.Role
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

        private (string ClientId, string ClientSecret) GetClientCredentialsForUser(ClaimsPrincipal? user)
        {
            if (user != null && user.Identity != null && user.Identity.IsAuthenticated)
            {
                if (user.IsInRole("Admin") && _clientSettings.MultiShopAdminId != null)
                {
                    return (_clientSettings.MultiShopAdminId.ClientId, _clientSettings.MultiShopAdminId.ClientSecret);
                }

                if (user.IsInRole("Manager") && _clientSettings.MultiShopManagerId != null)
                {
                    return (_clientSettings.MultiShopManagerId.ClientId, _clientSettings.MultiShopManagerId.ClientSecret);
                }

                if (user.IsInRole("User") && _clientSettings.MultiShopUserId != null)
                {
                    return (_clientSettings.MultiShopUserId.ClientId, _clientSettings.MultiShopUserId.ClientSecret);
                }
            }

            // Default fallback
            var defaultClient = _clientSettings.MultiShopManagerId ?? _clientSettings.MultiShopUserId ?? _clientSettings.MultiShopAdminId;
            return (defaultClient?.ClientId ?? "MultiShopManagerId", defaultClient?.ClientSecret ?? "multishopsecret");
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
                    Value = DateTime.UtcNow.AddSeconds(expiresIn).ToString("O")
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