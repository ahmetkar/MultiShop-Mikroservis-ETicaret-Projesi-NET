using Duende.IdentityModel.Client;
using IdentityModel.AspNetCore.AccessTokenManagement;
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

        public ClientCredentialTokenService(IOptions<ClientSettings> clientSettings,IOptions<ServiceApiSettings> serviceApiSettings,HttpClient httpClient,IClientAccessTokenCache clientAccessTokenCache)
        {
            _clientSettings = clientSettings.Value;
            _serviceApiSettings = serviceApiSettings.Value;
            _httpClient = httpClient;
            _clientAccessTokenCache = clientAccessTokenCache;
        }

        public async Task<string> GetToken()
        {
            var token1 = await _clientAccessTokenCache.GetAsync("multishoptoken",new ClientAccessTokenParameters { });
            if (token1!=null)
            {
                return token1.AccessToken;
            }
            var discoveryEndpoint = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest { 
                Address = _serviceApiSettings.IdentityServerUrl,
                Policy = new DiscoveryPolicy
                {
                    RequireHttps = false
                }
            });

            var clientCredentialTokenRequest = new ClientCredentialsTokenRequest
            {
                    ClientId = _clientSettings.MultiShopVisitorClient.ClientId,
                    ClientSecret = _clientSettings.MultiShopVisitorClient.ClientSecret,
                    Address = discoveryEndpoint.TokenEndpoint
            };
            var token2 = await _httpClient.RequestClientCredentialsTokenAsync(clientCredentialTokenRequest);
            await _clientAccessTokenCache.SetAsync("multishoptoken",token2.AccessToken,token2.ExpiresIn, new ClientAccessTokenParameters { });

            return token2.AccessToken;
        }
    }
}
