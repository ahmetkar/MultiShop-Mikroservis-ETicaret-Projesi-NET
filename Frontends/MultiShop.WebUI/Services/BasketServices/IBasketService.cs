using MultiShop.DtoLayer.BasketDtos;

namespace MultiShop.WebUI.Services.BasketServices
{
    public interface IBasketService
    {
        Task<BasketTotalDto> AddBasketItem(BasketTotalDto values, string id);
        Task<bool> RemoveBasketItem(BasketTotalDto values, string productId, Func<BasketTotalDto, Task> SaveBasket);
        Task<bool> DecrementBasketItem(BasketTotalDto values, string productId, Func<BasketTotalDto, Task> SaveBasket);
        Task DeleteBasket(string userId);
        Task<int> AddCookieDataToDatabase();
        Task<BasketTotalDto> GetBasketFromCookies();
        Task<BasketTotalDto?> GetBasketFromDatabase();
        Task SaveBasketToCookies(BasketTotalDto basket);
        Task SaveBasketToDatabase(BasketTotalDto basketTotalDto);
        Task AddBasketItemToCookies(string id);
        Task AddBasketItemToDatabase(string id);
        Task RemoveBasketItemFromCookies(string productId);
        Task DeleteBasketFromCookies();
        Task DeleteBasketFromDatabase();
        Task RemoveBasketItemFromDatabase(string productId);
        Task DecrementBasketItemFromCookies(string productId);
        Task DecrementBasketItemFromDatabase(string productId);
    }
}