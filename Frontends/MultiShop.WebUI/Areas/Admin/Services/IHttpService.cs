namespace MultiShop.WebUI.Areas.Admin.Services
{
    public interface IHttpService
    {
        Task<List<T>?> Get<T>(string endpoint);
        Task<bool> Create<T>(string endpoint, T dto);
        Task<bool> DeleteById(string endpoint, string id);
        Task<T?> GetById<T>(string endpoint, string id);
        Task<bool> Update<T>(string endpoint, T dto);
        Task<T?> GetOne<T>(string endpoint);
        void setUrl(string url);

    }
}
