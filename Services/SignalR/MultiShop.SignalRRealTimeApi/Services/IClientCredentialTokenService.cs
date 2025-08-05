namespace MultiShop.SignalRRealTimeApi.Services
{
    public interface IClientCredentialTokenService
    {
        Task<string> GetToken();

    }
}
