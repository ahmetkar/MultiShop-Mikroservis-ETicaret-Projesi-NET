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
        private readonly ILogger<ResourceOwnerPasswordTokenHandler> _logger;

        public ResourceOwnerPasswordTokenHandler(
            IHttpContextAccessor httpContextAccessor,
            IIdentityService identityService,
            ILogger<ResourceOwnerPasswordTokenHandler> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _identityService = identityService;
            _logger = logger;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null)
            {
                var accessToken = await httpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
                if (!string.IsNullOrWhiteSpace(accessToken))
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                }
            }

            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.Unauthorized && httpContext != null)
            {
                try
                {
                    var tokenResponseNewAccessToken = await _identityService.GetRefreshToken();

                    if (!string.IsNullOrWhiteSpace(tokenResponseNewAccessToken))
                    {
                        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponseNewAccessToken);
                        response = await base.SendAsync(request, cancellationToken);
                    }
                    else
                    {
                        _logger.LogWarning("Refresh token sonrasında yeni access token alınamadı. İstek yetkisiz (401) kaldı.");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Token yenileme (refresh token) sırasında hata oluştu.");
                }
            }

            return response;
        }
    }
}
