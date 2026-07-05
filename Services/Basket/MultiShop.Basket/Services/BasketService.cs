using MultiShop.Basket.Dtos;
using MultiShop.Basket.Settings;
using System.Text.Json;

namespace MultiShop.Basket.Services
{
    public class BasketService : IBasketService
    {
        private readonly RedisService _redisService;

        public BasketService(RedisService redisService)
        {
            _redisService = redisService;
       
        }
        public async Task DeleteBasket(string userId)
        {
            await _redisService.GetDb().KeyDeleteAsync(userId);

        }

        public async Task<BasketTotalDto?> GetBasket(string userId)
        {
            var existsBasket = await _redisService.GetDb().StringGetAsync(userId);

            if (existsBasket.IsNull)
            {
                return null;
            }

            var baskettotal = JsonSerializer.Deserialize<BasketTotalDto>(existsBasket);

            if (baskettotal != null)
            {
                return baskettotal;
            }
            return null;
            
        }

        public async Task SaveBasket(BasketTotalDto basketTotalDto)
        {
            
            
            await _redisService.GetDb().StringSetAsync(basketTotalDto.UserId, JsonSerializer.Serialize(basketTotalDto));
        }
    }
}
