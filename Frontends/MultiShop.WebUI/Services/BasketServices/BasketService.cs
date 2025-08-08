using MultiShop.DtoLayer.BasketDtos;
using MultiShop.WebUI.Services.CatalogServices.ProductServices;

namespace MultiShop.WebUI.Services.BasketServices
{
    public class BasketService : IBasketService
    {
        private readonly IProductService _productService;
        private readonly HttpClient _httpClient;

        public BasketService(HttpClient httpClient, IProductService productService)
        {
            _httpClient = httpClient;
            _productService = productService;
        }

        public async Task AddBasketItem(string id)
        {
            
            var values = await GetBasket();

            var product = await _productService.GetByIdProduct(id);

            var item = new BasketItemDto
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                Price = product.ProductPrice,
                Quantity = 1,
                ProductImageUrl = product.ProductImageUrl
            };

            if (values != null)
            {
                    if (values.BasketItems.Count(x => x.ProductId == id) == 0)
                    {
                    values.BasketItems.Add(item);

                    }
                    else if (values.BasketItems.Count(x => x.ProductId == id) > 0)
                    {
                        var addedItem = values.BasketItems.FirstOrDefault(x => x.ProductId == id);
                        int index = values.BasketItems.IndexOf(addedItem);
                        values.BasketItems[index].Quantity += 1;
                    }
                }
                else
                {
                    values = new BasketTotalDto();
                    values.BasketItems.Add(item);
                }

            await SaveBasket(values);
        }

        public Task DeleteBasket(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<BasketTotalDto?> GetBasket()
        {
            var responseMessage = await _httpClient.GetAsync("baskets");
            var values = await responseMessage.Content.ReadFromJsonAsync<BasketTotalDto>();
            return values;
        }

        public async Task<bool> RemoveBasketItem(string productId)
        {
            var values = await GetBasket();
            if (values != null)
            {
                var deletedItem = values.BasketItems.FirstOrDefault(x => x.ProductId == productId);
                if (deletedItem != null)
                {
                    if (values.BasketItems.Count(x => x.ProductId == productId) > 0)
                    {
                        var result = values.BasketItems.Remove(deletedItem);
                        await SaveBasket(values);
                        return true;
                    }
                 
                }
            }
            return false;
        }

        public async Task<bool> DecrementBasketItem(string productId)
        {
            var values = await GetBasket();
            if (values != null)
            {
                var item = values.BasketItems.FirstOrDefault(x => x.ProductId == productId);
                int index = values.BasketItems.IndexOf(item);
                if (values.BasketItems[index].Quantity > 1)
                {
                    values.BasketItems[index].Quantity -= 1;
                    await SaveBasket(values);
                    return true;
                }else
                {
                    var result = values.BasketItems.Remove(item);
                    await SaveBasket(values);
                    return true;
                }
            }

            return false;
        }

      

        public async Task SaveBasket(BasketTotalDto basketTotalDto)
        {
            await _httpClient.PostAsJsonAsync("baskets",basketTotalDto);

        }
    }
}
