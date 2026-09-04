using Microsoft.AspNetCore.Http;
using MultiShop.DtoLayer.BasketDtos;
using MultiShop.WebUI.Services.CatalogServices.ProductServices;
using MultiShop.WebUI.Services.DiscountServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace MultiShop.WebUI.Services.BasketServices
{
    public class BasketService : IBasketService
    {
        private readonly IProductService _productService;
        private readonly IDiscountService _discountService;
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BasketService(
            HttpClient httpClient,
            IProductService productService,
            IDiscountService discountService,
            IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _productService = productService;
            _discountService = discountService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<int> AddCookieDataToDatabase()
        {
            var context = _httpContextAccessor.HttpContext;
            if (context != null && context.Request.Cookies.ContainsKey("basket"))
            {
                var basketcookie = context.Request.Cookies["basket"];
                if (!string.IsNullOrEmpty(basketcookie))
                {
                    var basketTotalFromCookie = JsonConvert.DeserializeObject<BasketTotalDto>(basketcookie);
                    if (basketTotalFromCookie != null && basketTotalFromCookie.BasketItems.Count > 0)
                    {
                        var basketTotalFromDatabase = await GetBasketFromDatabase() ?? new BasketTotalDto();

                        foreach (var item in basketTotalFromCookie.BasketItems)
                        {
                            var existingInDb = basketTotalFromDatabase.BasketItems.FirstOrDefault(x =>
                                x.ProductId == item.ProductId && (x.SelectedFilter ?? "").Trim() == (item.SelectedFilter ?? "").Trim());

                            if (existingInDb != null)
                            {
                                existingInDb.Quantity += item.Quantity;
                            }
                            else
                            {
                                basketTotalFromDatabase.BasketItems.Add(item);
                            }
                        }

                        if (!string.IsNullOrEmpty(basketTotalFromCookie.DiscountCode))
                        {
                            basketTotalFromDatabase.DiscountCode = basketTotalFromCookie.DiscountCode;
                        }
                        if (basketTotalFromCookie.DiscountRate.HasValue && basketTotalFromCookie.DiscountRate.Value > 0)
                        {
                            basketTotalFromDatabase.DiscountRate = basketTotalFromCookie.DiscountRate;
                        }

                        await SaveBasketToDatabase(basketTotalFromDatabase);
                        _httpContextAccessor.HttpContext?.Session.SetInt32("IsCookiesAdded", 1);
                        await SaveBasketToCookies(new BasketTotalDto());
                        return 1;
                    }
                }
            }
            return 0;
        }

        public async Task<BasketTotalDto> GetBasketFromCookies()
        {
            var context = _httpContextAccessor.HttpContext;
            if (context != null && context.Request.Cookies.ContainsKey("basket"))
            {
                var basket = context.Request.Cookies["basket"];
                if (!string.IsNullOrEmpty(basket))
                {
                    var basketTotal = JsonConvert.DeserializeObject<BasketTotalDto>(basket);
                    return CalculateKDVAndTotal(basketTotal ?? new BasketTotalDto());
                }
            }
            return new BasketTotalDto();
        }

        public async Task<BasketTotalDto?> GetBasketFromDatabase()
        {
            var responseMessage = await _httpClient.GetAsync("baskets");
            if (responseMessage.StatusCode == HttpStatusCode.NoContent)
            {
                return new BasketTotalDto();
            }
            if (responseMessage.IsSuccessStatusCode)
            {
                var values = await responseMessage.Content.ReadFromJsonAsync<BasketTotalDto>();
                return CalculateKDVAndTotal(values ?? new BasketTotalDto());
            }
            return new BasketTotalDto();
        }

        public BasketTotalDto CalculateKDVAndTotal(BasketTotalDto basketTotalDto)
        {
            if (basketTotalDto == null) return new BasketTotalDto();

            double totalkdvprice = 0;
            double totalpricewithoutkdv = 0;
            foreach (var i in basketTotalDto.BasketItems)
            {
                totalkdvprice += i.KDVPrice * i.Quantity;
                totalpricewithoutkdv += (double)i.Price * i.Quantity;
            }
            basketTotalDto.TotalPriceWithoutKDV = totalpricewithoutkdv;
            basketTotalDto.KDVPrice = totalkdvprice;
            double rawTotal = totalpricewithoutkdv + totalkdvprice;

            if (basketTotalDto.DiscountRate.HasValue && basketTotalDto.DiscountRate.Value > 0)
            {
                basketTotalDto.TotalPriceWithoutDiscount = rawTotal;
                basketTotalDto.TotalPrice = rawTotal - (rawTotal * basketTotalDto.DiscountRate.Value / 100.0);
            }
            else
            {
                basketTotalDto.TotalPriceWithoutDiscount = rawTotal;
                basketTotalDto.TotalPrice = rawTotal;
            }

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

        public async Task SaveBasketToCookies(BasketTotalDto basketTotalDto, string discountCode, int discountRate)
        {
            basketTotalDto.DiscountCode = discountCode;
            basketTotalDto.DiscountRate = discountRate;
            basketTotalDto = CalculateKDVAndTotal(basketTotalDto);

            var context = _httpContextAccessor.HttpContext;
            if (context != null)
            {
                var json = JsonConvert.SerializeObject(basketTotalDto);
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

        public async Task SaveBasketToDatabase(BasketTotalDto basketTotalDto, string discountCode, int discountRate)
        {
            basketTotalDto.DiscountCode = discountCode;
            basketTotalDto.DiscountRate = discountRate;
            basketTotalDto = CalculateKDVAndTotal(basketTotalDto);

            await _httpClient.PostAsJsonAsync("baskets", basketTotalDto);
        }

        public async Task AddBasketItemToCookies(string id)
        {
            await AddBasketItemToCookies(id, 1, null);
        }

        public async Task AddBasketItemToCookies(string id, int quantity = 1, string? selectedFilter = null)
        {
            var basket = await GetBasketFromCookies();
            var newBasket = await AddBasketItem(basket, id, quantity, selectedFilter);
            await SaveBasketToCookies(newBasket);
        }

        public async Task AddBasketItemToDatabase(string id)
        {
            await AddBasketItemToDatabase(id, 1, null);
        }

        public async Task AddBasketItemToDatabase(string id, int quantity = 1, string? selectedFilter = null)
        {
            var basket = await GetBasketFromDatabase() ?? new BasketTotalDto();
            var newBasket = await AddBasketItem(basket, id, quantity, selectedFilter);
            await SaveBasketToDatabase(newBasket);
        }

        public async Task RemoveBasketItemFromCookies(string productId, string? selectedFilter = null)
        {
            var basket = await GetBasketFromCookies();
            await RemoveBasketItem(basket, productId, selectedFilter, SaveBasketToCookies);
        }

        public async Task DeleteBasketFromCookies()
        {
            var context = _httpContextAccessor.HttpContext;
            if (context != null)
            {
                context.Response.Cookies.Delete("basket");
            }
        }

        public async Task DeleteBasketFromDatabase()
        {
            await _httpClient.DeleteAsync("baskets");
        }

        public async Task RemoveBasketItemFromDatabase(string productId, string? selectedFilter = null)
        {
            var basket = await GetBasketFromDatabase() ?? new BasketTotalDto();
            await RemoveBasketItem(basket, productId, selectedFilter, SaveBasketToDatabase);
        }

        public async Task DecrementBasketItemFromCookies(string productId, string? selectedFilter = null)
        {
            var basket = await GetBasketFromCookies();
            await DecrementBasketItem(basket, productId, selectedFilter, SaveBasketToCookies);
        }

        public async Task DecrementBasketItemFromDatabase(string productId, string? selectedFilter = null)
        {
            var basket = await GetBasketFromDatabase() ?? new BasketTotalDto();
            await DecrementBasketItem(basket, productId, selectedFilter, SaveBasketToDatabase);
        }

        public async Task<BasketTotalDto> AddBasketItem(BasketTotalDto values, string id)
        {
            return await AddBasketItem(values, id, 1, null);
        }

        public async Task<BasketTotalDto> AddBasketItem(BasketTotalDto values, string id, int quantity = 1, string? selectedFilter = null)
        {
            if (values == null) values = new BasketTotalDto();

            var normalizedFilter = string.IsNullOrWhiteSpace(selectedFilter) ? null : selectedFilter.Trim();

            var existingItem = values.BasketItems.FirstOrDefault(x =>
                x.ProductId == id && (string.IsNullOrWhiteSpace(x.SelectedFilter) ? null : x.SelectedFilter.Trim()) == normalizedFilter);

            if (existingItem != null)
            {
                existingItem.Quantity += (quantity > 0 ? quantity : 1);
            }
            else
            {
                var product = await _productService.GetByIdProduct(id);
                if (product != null)
                {
                    decimal unitPrice = product.ProductPrice;

                    // Apply active product discount if available from admin coupon/discount
                    try
                    {
                        var productDiscount = await _discountService.GetDiscountByProductIdAsync(id);
                        if (productDiscount != null && productDiscount.IsActive && productDiscount.ValidDate >= DateTime.UtcNow && productDiscount.Rate > 0)
                        {
                            unitPrice = unitPrice - (unitPrice * productDiscount.Rate / 100m);
                        }
                    }
                    catch { }

                    var kdvPercent = product.KDVPercent > 0 ? product.KDVPercent : 20m;
                    var item = new BasketItemDto
                    {
                        ProductId = id,
                        ProductName = product.ProductName,
                        Price = unitPrice,
                        KDVPrice = (double)(unitPrice * kdvPercent / 100m),
                        KDVPercent = (double)kdvPercent,
                        Quantity = quantity > 0 ? quantity : 1,
                        ProductImageUrl = product.ProductImageUrl,
                        SelectedFilter = normalizedFilter
                    };
                    values.BasketItems.Add(item);
                }
            }

            return values;
        }

        public Task DeleteBasket(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> RemoveBasketItem(BasketTotalDto values, string productId, string? selectedFilter, Func<BasketTotalDto, Task> SaveBasket)
        {
            if (values != null)
            {
                var normalizedFilter = string.IsNullOrWhiteSpace(selectedFilter) ? null : selectedFilter.Trim();
                var item = values.BasketItems.FirstOrDefault(x =>
                    x.ProductId == productId && (string.IsNullOrWhiteSpace(x.SelectedFilter) ? null : x.SelectedFilter.Trim()) == normalizedFilter);

                if (item != null)
                {
                    values.BasketItems.Remove(item);
                    await SaveBasket(values);
                    return true;
                }
            }
            return false;
        }

        public async Task<bool> DecrementBasketItem(BasketTotalDto values, string productId, string? selectedFilter, Func<BasketTotalDto, Task> SaveBasket)
        {
            if (values != null)
            {
                var normalizedFilter = string.IsNullOrWhiteSpace(selectedFilter) ? null : selectedFilter.Trim();
                var item = values.BasketItems.FirstOrDefault(x =>
                    x.ProductId == productId && (string.IsNullOrWhiteSpace(x.SelectedFilter) ? null : x.SelectedFilter.Trim()) == normalizedFilter);

                if (item != null)
                {
                    if (item.Quantity > 1)
                    {
                        item.Quantity -= 1;
                    }
                    else
                    {
                        values.BasketItems.Remove(item);
                    }
                    await SaveBasket(values);
                    return true;
                }
            }
            return false;
        }
    }
}
