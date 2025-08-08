using MultiShop.DtoLayer.BasketDtos;

namespace MultiShop.WebUI.Services.BasketServices
{
    public interface IBasketService
    {
        Task<BasketTotalDto?> GetBasket();
        Task SaveBasket(BasketTotalDto basket);
        Task DeleteBasket(string userId);
        Task AddBasketItem(string id);
        Task<bool> RemoveBasketItem(string productId);
        Task<bool> DecrementBasketItem(string productId);
    }
}
