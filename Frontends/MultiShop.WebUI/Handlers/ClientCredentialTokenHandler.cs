
using MultiShop.WebUI.Services.Interfaces;
using System.Net;
using System.Net.Http.Headers;

namespace MultiShop.WebUI.Handlers
{
    public class ClientCredentialTokenHandler : DelegatingHandler
    {
        private readonly IClientCredentialTokenService _clientCredentialTokenService;
        private readonly ILogger<ClientCredentialTokenHandler> _logger;

        public ClientCredentialTokenHandler(
            IClientCredentialTokenService clientCredentialTokenService,
            ILogger<ClientCredentialTokenHandler> logger)
        {
            _clientCredentialTokenService = clientCredentialTokenService;
            _logger = logger;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                var token = await _clientCredentialTokenService.GetToken();
                if (!string.IsNullOrWhiteSpace(token))
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Client credential token alınırken hata oluştu.");
            }

            var response = await base.SendAsync(request, cancellationToken);
            return response;
        }
    }
}
