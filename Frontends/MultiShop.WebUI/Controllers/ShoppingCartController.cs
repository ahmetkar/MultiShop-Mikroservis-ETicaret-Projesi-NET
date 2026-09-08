using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.BasketDtos;
using MultiShop.DtoLayer.OrderDtos.OrderOrderingDtos;
using MultiShop.WebUI.Helpers;
using MultiShop.WebUI.Services.BasketServices;
using MultiShop.WebUI.Services.CargoServices.CargoCompanyServices;
using MultiShop.WebUI.Services.CatalogServices.FilterServices;
using MultiShop.WebUI.Services.CatalogServices.ProductServices;
using MultiShop.WebUI.Services.Interfaces;
using MultiShop.WebUI.Services.OrderServices.OrderOderingServices;
using Newtonsoft.Json.Linq;
using System.Security.Claims;

namespace MultiShop.WebUI.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly IProductService _productService;
        private readonly IBasketService _basketService;
        private readonly IFilterService _filterService;
        private readonly ICargoCompanyService _cargoCompanyService;
        private readonly IOrderOderingService _orderOderingService;
        private readonly IUserService _userService;
        private readonly IDataProtector _protector;

        public ShoppingCartController(
            IBasketService basketService,
            IProductService productService,
            IFilterService filterService,
            ICargoCompanyService cargoCompanyService,
            IOrderOderingService orderOderingService,
            IUserService userService,
            IDataProtectionProvider provider)
        {
            _basketService = basketService;
            _productService = productService;
            _filterService = filterService;
            _cargoCompanyService = cargoCompanyService;
            _orderOderingService = orderOderingService;
            _userService = userService;
            _protector = provider.CreateProtector("ActiveOrderingId_Protector");
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Directory1 = "Ana Sayfa";
            ViewBag.Directory2 = "Ürünler";
            ViewBag.Directory3 = "Sepetim";



          

            var basketitems = new BasketTotalDto();

            if (User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirst("sub")?.Value
                  ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                  ?? User.FindFirst("nameidentifier")?.Value;

                var activeOrdering = await _orderOderingService.GetActiveOrderingByUserId(userId);

                if (activeOrdering != null)
                {
                    if (activeOrdering.Status != OrderStatus.PaymentCompleted && activeOrdering.Status != OrderStatus.CargoFailed &&
                        activeOrdering.Status != OrderStatus.CargoCreated && activeOrdering.Status != OrderStatus.OrderNotCreated
                        )
                    {
                       
                        var encryptedId = _protector.Protect(activeOrdering.OrderingId.ToString());
                        return RedirectToAction("Index", "Payment", new { ActiveOrderingId = encryptedId });
                    }
                }
                

                int? isAdded = HttpContext.Session.GetInt32("IsCookiesAdded");
                if (isAdded != 1)
                {
                    isAdded = await _basketService.AddCookieDataToDatabase();
                }


                basketitems = await _basketService.GetBasketFromDatabase();
            }
            else
            {
                basketitems = await _basketService.GetBasketFromCookies();
            }




            int count = 0;
            count = basketitems.BasketItems.Count;

            if (count > 0)
            {
                ViewBag.TotalPriceWithoutKDV = basketitems.TotalPriceWithoutKDV;

                ViewBag.KDV = basketitems.KDVPrice;

                ViewBag.TotalPrice = basketitems.TotalPrice;

                ViewBag.TotalPriceWithoutDiscount = basketitems.TotalPriceWithoutDiscount;
                ViewBag.DiscountRate = basketitems.DiscountRate;
                ViewBag.DiscountCode = basketitems.DiscountCode;
                
            }


            return View(count);
        }

        [HttpPost]
        public async Task<IActionResult> AddBasketToItem(string ProductId, int Quantity = 1, string? selectedFilter = null)
        {
            var product = await _productService.GetByIdProduct(ProductId);
            if (product == null)
            {
                return RedirectToAction("Index");
            }

            var allFilters = await _filterService.GetAllFilterAsync();
            bool isOutOfStock = ProductStockHelper.IsProductOutOfStock(product.FilterIds, product.FilterStocks, allFilters);
            if (isOutOfStock)
            {
                return RedirectToAction("Index");
            }

            if (product.FilterIds != null && product.FilterIds.Count > 0)
            {
                var prodFilters = allFilters.Where(f => product.FilterIds.Contains(f.FilterId)).ToList();
                if (prodFilters.Count == 0)
                {
                    return RedirectToAction("Index");
                }

                if (string.IsNullOrWhiteSpace(selectedFilter))
                {
                    var defaultParts = new List<string>();
                    var grouped = prodFilters.GroupBy(f => f.FilterTitle);
                    foreach (var group in grouped)
                    {
                        var opt = group.FirstOrDefault(f => product.FilterStocks != null && product.FilterStocks.TryGetValue(f.FilterId, out int s) && s > 0);
                        if (opt == null)
                        {
                            return RedirectToAction("Index");
                        }
                        defaultParts.Add($"{group.Key}: {opt.FilterName}");
                    }
                    selectedFilter = string.Join(", ", defaultParts);
                }
                else
                {
                    var parts = selectedFilter.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var part in parts)
                    {
                        var trimmed = part.Trim();
                        var matchedFilter = prodFilters.FirstOrDefault(f =>
                            trimmed.Equals(f.FilterName, StringComparison.OrdinalIgnoreCase) ||
                            trimmed.Contains($"{f.FilterTitle}: {f.FilterName}", StringComparison.OrdinalIgnoreCase) ||
                            trimmed.Contains($"{f.FilterTitle}:{f.FilterName}", StringComparison.OrdinalIgnoreCase));

                        if (matchedFilter != null)
                        {
                            if (product.FilterStocks == null || !product.FilterStocks.TryGetValue(matchedFilter.FilterId, out int s) || s <= 0)
                            {
                                return RedirectToAction("Index");
                            }
                        }
                    }
                }
            }

            if (User.Identity.IsAuthenticated)
            {
                await _basketService.AddBasketItemToDatabase(ProductId, Quantity, selectedFilter);
            }
            else
            {
                await _basketService.AddBasketItemToCookies(ProductId, Quantity, selectedFilter);
            }

            return RedirectToAction("Index");
        }



        [HttpPost]
        public async Task<IActionResult> DeleteBasket()
        {
            if (User.Identity.IsAuthenticated)
            {
                await _basketService.DeleteBasketFromDatabase();
            }
            else
            {
                await _basketService.DeleteBasketFromCookies();
            }

            return RedirectToAction("Index");
        }


        public async Task<IActionResult> AddBasketItem(string id, string? selectedFilter = null)
        {
            var product = await _productService.GetByIdProduct(id);
            if (product == null)
            {
                return RedirectToAction("Index");
            }

            var allFilters = await _filterService.GetAllFilterAsync();
            bool isOutOfStock = ProductStockHelper.IsProductOutOfStock(product.FilterIds, product.FilterStocks, allFilters);
            if (isOutOfStock)
            {
                return RedirectToAction("Index");
            }

            if (product.FilterIds != null && product.FilterIds.Count > 0)
            {
                var prodFilters = allFilters.Where(f => product.FilterIds.Contains(f.FilterId)).ToList();
                if (prodFilters.Count == 0)
                {
                    return RedirectToAction("Index");
                }

                if (string.IsNullOrWhiteSpace(selectedFilter))
                {
                    var defaultParts = new List<string>();
                    var grouped = prodFilters.GroupBy(f => f.FilterTitle);
                    foreach (var group in grouped)
                    {
                        var opt = group.FirstOrDefault(f => product.FilterStocks != null && product.FilterStocks.TryGetValue(f.FilterId, out int s) && s > 0);
                        if (opt == null)
                        {
                            return RedirectToAction("Index");
                        }
                        defaultParts.Add($"{group.Key}: {opt.FilterName}");
                    }
                    selectedFilter = string.Join(", ", defaultParts);
                }
            }

            if (User.Identity.IsAuthenticated)
            {
                await _basketService.AddBasketItemToDatabase(id, 1, selectedFilter);
            }
            else
            {
                await _basketService.AddBasketItemToCookies(id, 1, selectedFilter);
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> RemoveBasketItem(string id, string? selectedFilter = null)
        {
            if (User.Identity.IsAuthenticated)
            {
                await _basketService.RemoveBasketItemFromDatabase(id, selectedFilter);
            }
            else
            {
                await _basketService.RemoveBasketItemFromCookies(id, selectedFilter);
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DecrementBasketItem(string id, string? selectedFilter = null)
        {
            if (User.Identity.IsAuthenticated)
            {
                await _basketService.DecrementBasketItemFromDatabase(id, selectedFilter);
            }
            else
            {
                await _basketService.DecrementBasketItemFromCookies(id, selectedFilter);
            }

            return RedirectToAction("Index");
        }
    }
}