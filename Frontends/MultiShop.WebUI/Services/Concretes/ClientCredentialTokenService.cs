using IdentityModel.AspNetCore.AccessTokenManagement;
using IdentityModel.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MultiShop.WebUI.Services.Interfaces;
using MultiShop.WebUI.Settings;

namespace MultiShop.WebUI.Services.Concretes
{
    public class ClientCredentialTokenService : IClientCredentialTokenService
    {
        private readonly ServiceApiSettings _serviceApiSettings;
        private readonly HttpClient _httpClient;
        private readonly IClientAccessTokenCache _clientAccessTokenCache;
        private readonly ClientSettings _clientSettings;
        private readonly ILogger<ClientCredentialTokenService> _logger;

        public ClientCredentialTokenService(
            IOptions<ClientSettings> clientSettings,
            IOptions<ServiceApiSettings> serviceApiSettings,
            HttpClient httpClient,
            IClientAccessTokenCache clientAccessTokenCache,
            ILogger<ClientCredentialTokenService> logger)
        {
            _clientSettings = clientSettings.Value;
            _serviceApiSettings = serviceApiSettings.Value;
            _httpClient = httpClient;
            _clientAccessTokenCache = clientAccessTokenCache;
            _logger = logger;
        }

        public async Task<string> GetToken()
        {
            try
            {
                var cachedToken = await _clientAccessTokenCache.GetAsync("multishoptoken", new ClientAccessTokenParameters());
                if (cachedToken != null && !string.IsNullOrWhiteSpace(cachedToken.AccessToken))
                {
                    return cachedToken.AccessToken;
                }

                if (string.IsNullOrWhiteSpace(_serviceApiSettings.IdentityServerUrl))
                {
                    _logger.LogError("IdentityServerUrl boş.");
                    return string.Empty;
                }

                var discoveryEndpoint = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
                {
                    Address = _serviceApiSettings.IdentityServerUrl,
                    Policy = new DiscoveryPolicy
                    {
                        RequireHttps = false
                    }
                });

                if (discoveryEndpoint.IsError || string.IsNullOrWhiteSpace(discoveryEndpoint.TokenEndpoint))
                {
                    _logger.LogError("Discovery belgesi alınamadı: {Error}", discoveryEndpoint.Error);
                    return string.Empty;
                }

                var visitorClient = _clientSettings.MultiShopVisitorClient;
                if (visitorClient == null || string.IsNullOrWhiteSpace(visitorClient.ClientId))
                {
                    _logger.LogError("MultiShopVisitorClient ClientSettings içinde bulunamadı.");
                    return string.Empty;
                }

                var clientCredentialTokenRequest = new ClientCredentialsTokenRequest
                {
                    ClientId = visitorClient.ClientId,
                    ClientSecret = visitorClient.ClientSecret,
                    Address = discoveryEndpoint.TokenEndpoint,
                    Scope = "CatalogReadPermission CatalogFullPermission DiscountFullPermission OrderFullPermission CargoFullPermission CommentFullPermission PaymentReadPermission PaymentCreatePermission PaymentDeletePermission PaymentFullPermission OcelotFullPermission IdentityServerApi"
                };

                var tokenResponse = await _httpClient.RequestClientCredentialsTokenAsync(clientCredentialTokenRequest);

                if (tokenResponse.IsError || string.IsNullOrWhiteSpace(tokenResponse.AccessToken))
                {
                    _logger.LogError("Client credentials token isteği başarısız: {Error} - {Description}", tokenResponse.Error, tokenResponse.ErrorDescription);
                    return string.Empty;
                }

                await _clientAccessTokenCache.SetAsync("multishoptoken", tokenResponse.AccessToken, tokenResponse.ExpiresIn, new ClientAccessTokenParameters());

                return tokenResponse.AccessToken;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Client credentials token alınırken hata oluştu.");
                return string.Empty;
            }
        }
    }
}
