
using Duende.IdentityModel.Client;
using IdentityModel.AspNetCore.AccessTokenManagement;
using Microsoft.Extensions.Options;
using MultiShop.SignalRRealTimeApi.Services;


namespace MultiShop.SignalRRealTimeApi.Services
{
    public class ClientCredentialTokenService : IClientCredentialTokenService
    {
        private readonly HttpClient _httpClient;
        private readonly IClientAccessTokenCache _clientAccessTokenCache;


        public ClientCredentialTokenService(HttpClient httpClient,IClientAccessTokenCache clientAccessTokenCache)
        {
 
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
                Address = "http://localhost:5001/",
                Policy = new DiscoveryPolicy
                {
                    RequireHttps = false
                }
            });

            var clientCredentialTokenRequest = new ClientCredentialsTokenRequest
            {
                    ClientId = "MultiShopVisitorId",
                    ClientSecret = "multishopsecret",
                    Address = discoveryEndpoint.TokenEndpoint
            };
            var token2 = await _httpClient.RequestClientCredentialsTokenAsync(clientCredentialTokenRequest);
            await _clientAccessTokenCache.SetAsync("multishoptoken",token2.AccessToken,token2.ExpiresIn, new ClientAccessTokenParameters { });

            return token2.AccessToken;
        }
    }
}
