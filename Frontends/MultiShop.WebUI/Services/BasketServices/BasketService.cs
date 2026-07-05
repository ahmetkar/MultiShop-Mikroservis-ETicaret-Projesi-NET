using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using MultiShop.DtoLayer.BasketDtos;
using MultiShop.WebUI.Services.CatalogServices.ProductServices;
using Newtonsoft.Json;
using NuGet.Protocol;
using System;
using System.Linq;
using System.Net;

namespace MultiShop.WebUI.Services.BasketServices
{
    public class BasketService : IBasketService
    {
        private readonly IProductService _productService;
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
       
        public BasketService(HttpClient httpClient, IProductService productService, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _productService = productService;
            _httpContextAccessor = httpContextAccessor;
   
        }

        public async Task<int> AddCookieDataToDatabase()
        {
            var context = _httpContextAccessor.HttpContext;
            if (context != null && context.Request.Cookies.ContainsKey("basket"))
            {
                var basketcookie = context.Request.Cookies["basket"];
                var basketTotalFromCookie = JsonConvert.DeserializeObject<BasketTotalDto>(basketcookie);

                var basketTotalFromDatabase = await GetBasketFromDatabase();

                if (basketTotalFromDatabase.BasketItems.Count > 0)
                {

                    for(int i = 0; i < basketTotalFromCookie.BasketItems.Count; i++)
                    {
                        var item = basketTotalFromCookie.BasketItems[i];
                        if (basketTotalFromDatabase.BasketItems.Any(x=>x.ProductId == item.ProductId))
                        {
                            var getItem = basketTotalFromDatabase.BasketItems.FirstOrDefault(x=>x.ProductId == item.ProductId);
                            int index = basketTotalFromDatabase.BasketItems.IndexOf(getItem);
                            basketTotalFromDatabase.BasketItems[index].Quantity += getItem.Quantity;
                            basketTotalFromCookie.BasketItems.Remove(item);
                        }
                    }

                    var items = basketTotalFromDatabase.BasketItems.Concat(basketTotalFromCookie.BasketItems);
                    basketTotalFromDatabase.BasketItems = items.ToList();

                    
                    
                    


                    if (basketTotalFromCookie.DiscountCode != null)
                    {
                        basketTotalFromDatabase.DiscountCode = basketTotalFromCookie.DiscountCode;
                    }
                    if (basketTotalFromCookie.DiscountRate != null)
                    {
                        basketTotalFromDatabase.DiscountRate = basketTotalFromCookie.DiscountRate;
                    }

                    await SaveBasketToDatabase(basketTotalFromDatabase);
                    _httpContextAccessor.HttpContext.Session.SetInt32("IsCookiesAdded", 1);
                    await SaveBasketToCookies(new BasketTotalDto());
                    return 1;
                }
                else
                {
                    await SaveBasketToDatabase(basketTotalFromCookie);
                    _httpContextAccessor.HttpContext.Session.SetInt32("IsCookiesAdded", 1);
                    await SaveBasketToCookies(new BasketTotalDto());
                    return 1;
                }
                
                

            }
            return 0;

        }

        public async Task<BasketTotalDto> GetBasketFromCookies()
        {
            var context = _httpContextAccessor.HttpContext;
            if (context != null && context.Request.Cookies.ContainsKey("basket")) {

                var basket =  context.Request.Cookies["basket"];
                var basketTotal = JsonConvert.DeserializeObject<BasketTotalDto>(basket);
                return basketTotal;
            }
            return new BasketTotalDto();
            
        }

        public async Task<BasketTotalDto?> GetBasketFromDatabase()
        {
            var responseMessage = await _httpClient.GetAsync("baskets");
           if(responseMessage.StatusCode == HttpStatusCode.NoContent)
            {
                return new BasketTotalDto();
            }
            if (responseMessage.IsSuccessStatusCode)
            {
                var values = await responseMessage.Content.ReadFromJsonAsync<BasketTotalDto>();
                return values;
            }
            return new BasketTotalDto();
        }

        public BasketTotalDto CalculateKDVAndTotal(BasketTotalDto basketTotalDto) {

            double totalkdvprice = 0;
            double totalpricewithoutkdv = 0;
            foreach (var i in basketTotalDto.BasketItems)
            {
                totalkdvprice += i.KDVPrice * i.Quantity;
                totalpricewithoutkdv += (double)i.Price * i.Quantity;

            }
            basketTotalDto.TotalPriceWithoutKDV = totalpricewithoutkdv;
            basketTotalDto.TotalPrice = totalpricewithoutkdv + totalkdvprice;
            basketTotalDto.KDVPrice = totalkdvprice;

            return basketTotalDto;

        }

        public async Task SaveBasketToCookies(BasketTotalDto basket)
        {
            var context = _httpContextAccessor.HttpContext;
            if (context != null)
            {
                basket = CalculateKDVAndTotal(basket);

                var json = JsonConvert.SerializeObject(basket);
                context.Response.Cookies.Append("basket", json, new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddDays(1),
                    HttpOnly = false
                });
            }
        }

        public async Task SaveBasketToDatabase(BasketTotalDto basketTotalDto)
        {
            basketTotalDto = CalculateKDVAndTotal(basketTotalDto);

            await _httpClient.PostAsJsonAsync("baskets", basketTotalDto);

        }

        public async Task AddBasketItemToCookies(string id)
        {
              var basket = await GetBasketFromCookies();
              var newBasket = await AddBasketItem(basket, id);
              await SaveBasketToCookies(newBasket);
            
        }

        public async Task AddBasketItemToDatabase(string id)
        {
            var basket = await GetBasketFromDatabase();
         
            var newBasket = await AddBasketItem(basket, id);

            await SaveBasketToDatabase(newBasket);

        }




        public async Task RemoveBasketItemFromCookies(string productId)
        {
            var basket = await GetBasketFromCookies();
            await RemoveBasketItem(basket, productId, SaveBasketToCookies);
        }



        public async Task DeleteBasketFromCookies()
        {
          
        }

        public async Task DeleteBasketFromDatabase()
        {

        }

        

        public async Task RemoveBasketItemFromDatabase(string productId)
        {
            var basket = await GetBasketFromDatabase();
            await RemoveBasketItem(basket, productId, SaveBasketToDatabase);
        }


        public async Task DecrementBasketItemFromCookies(string productId)
        {
            var basket = await GetBasketFromCookies();
            await DecrementBasketItem(basket,productId,SaveBasketToCookies);

        }

        public async Task DecrementBasketItemFromDatabase(string productId)
        {
            var basket = await GetBasketFromDatabase();
            await DecrementBasketItem(basket, productId, SaveBasketToDatabase);

        }




        public async Task<BasketTotalDto> AddBasketItem(BasketTotalDto values,string id)
        {
           
            var product = await _productService.GetByIdProduct(id);

            var item = new BasketItemDto
            {
               
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                Price = product.ProductPrice,
                KDVPrice = (double)product.KDVPrice,
                KDVPercent = (double)product.KDVPercent,
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

            return values;
            
        }

        public Task DeleteBasket(string userId)
        {
            throw new NotImplementedException();
        }


       

        public async Task<bool> RemoveBasketItem(BasketTotalDto values ,string productId,Func<BasketTotalDto,Task> SaveBasket)
        {
            
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

        public async Task<bool> DecrementBasketItem(BasketTotalDto values,string productId,Func<BasketTotalDto,Task> SaveBasket)
        {
       
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



       
    }
}
