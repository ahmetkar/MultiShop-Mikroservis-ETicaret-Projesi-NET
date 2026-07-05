using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using MultiShop.WebUI.Services.Interfaces;
using System.Net;
using System.Net.Http.Headers;

namespace MultiShop.WebUI.Handlers
{
    public class ResourceOwnerPasswordTokenHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IIdentityService _identityService;

        public ResourceOwnerPasswordTokenHandler(IHttpContextAccessor httpContextAccessor, IIdentityService identityService)
        {
            _httpContextAccessor = httpContextAccessor;
            _identityService = identityService;
        }



        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {

            var accessToken = await _httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer",accessToken);
            var response = await base.SendAsync(request,cancellationToken);
            if(response.StatusCode == HttpStatusCode.Unauthorized)
            {
                var tokenResponseNewAccessToken = await _identityService.GetRefreshToken();

                if (!string.IsNullOrWhiteSpace(tokenResponseNewAccessToken))
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponseNewAccessToken);
                    response = await base.SendAsync(request, cancellationToken);
                    if (response.StatusCode == HttpStatusCode.Unauthorized) {
                        var errorBody = await response.Content.ReadAsStringAsync(cancellationToken);

                        throw new Exception($@"
                                    Refresh tokenden sonra yeniden yetkilendirme yapılamadı.

                                    Method: {request.Method}
                                    Url: {request.RequestUri}
                                    Response Body: {errorBody}
                                    ");
                   
                }else
                    {
                        throw new Exception("Refresh sonrası yeni access token alınamadı.");
                    }
                }
            }

            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                var errorBody = await response.Content.ReadAsStringAsync(cancellationToken);

                throw new Exception($@"
                        Bad Request döndü.
                        Method: {request.Method}
                        Url: {request.RequestUri}
                        Response Body: {errorBody}
                        ");
                                    }

            return response;
            
        }   



    }
}
