using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.CommentDtos;
using MultiShop.DtoLayer.CatalogDtos.ProductDtos;
using MultiShop.WebUI.Services.CatalogServices.FeatureSliderServices;
using MultiShop.WebUI.Services.CatalogServices.OfferDiscountServices;
using MultiShop.WebUI.Services.CatalogServices.ProductServices;
using MultiShop.WebUI.Services.CatalogServices.SpecialOfferServices;
using MultiShop.WebUI.Services.CommentServices;
using MultiShop.WebUI.Services.DiscountServices;

namespace MultiShop.WebUI.Controllers
{
    public class ProductListController : Controller
    {
        private readonly ICommentService _commentService;
        private readonly IProductService _productService;
        private readonly IDiscountService _discountService;
        private readonly IFeatureSliderService _featureSliderService;
        private readonly ISpecialOfferService _specialOfferService;
        private readonly IOfferDiscountService _offerDiscountService;

        public ProductListController(
            ICommentService commentService,
            IProductService productService,
            IDiscountService discountService,
            IFeatureSliderService featureSliderService,
            ISpecialOfferService specialOfferService,
            IOfferDiscountService offerDiscountService)
        {
            _commentService = commentService;
            _productService = productService;
            _discountService = discountService;
            _featureSliderService = featureSliderService;
            _specialOfferService = specialOfferService;
            _offerDiscountService = offerDiscountService;
        }

        public IActionResult Index(string id, List<string>? filterIds, int page = 1)
        {
            ViewBag.Directory1 = "Ana Sayfa";
            ViewBag.Directory2 = "Ürün Listesi";
            ViewBag.Directory3 = "";
            ViewBag.i = id;
            ViewBag.FilterIds = filterIds ?? new List<string>();
            ViewBag.CurrentPage = page;
            return View();
        }

        public async Task<IActionResult> ProductDetail(string id)
        {
            ViewBag.Directory1 = "Ana Sayfa";
            ViewBag.Directory2 = "Ürün Listesi";
            ViewBag.Directory3 = "Ürün Detayları";
            ViewBag.id = id;

            var comments = await _commentService.GetCommentsByProductId(id);
            ViewBag.commentCount = comments != null ? comments.Count : 0;

            return View();
        }

        [HttpGet]
        public PartialViewResult AddComment()
        {
            return PartialView();   
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(CreateCommentDto createCommentDto)
        {
            createCommentDto.ImageUrl = "~/img/user.png";
            createCommentDto.Rating = 5;
            createCommentDto.CreatedDate = DateTime.Now;
            createCommentDto.Status = true;

            await _commentService.CreateCommentAsync(createCommentDto);
            return RedirectToAction("ProductDetail", "ProductList", new { id = createCommentDto.ProductId });
        }

        [HttpGet]
        public async Task<IActionResult> Search(string query, int page = 1)
        {
            ViewBag.Directory1 = "Ana Sayfa";
            ViewBag.Directory2 = "Arama Sonuçları";
            ViewBag.Directory3 = query;
            ViewBag.Query = query;
            ViewBag.CurrentPage = page;

            int pageSize = 9;
            var products = await _productService.SearchProductsAsync(query, page, pageSize);
            var totalCount = await _productService.GetSearchProductCountAsync(query);

            var discounts = await _discountService.GetActiveProductDiscountsAsync();
            var discountDict = discounts.GroupBy(x => x.ProductId).ToDictionary(g => g.Key, g => g.First().Rate);
            ViewBag.DiscountDict = discountDict;

            ViewBag.TotalCount = totalCount;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> Campaign(string type, string id)
        {
            ViewBag.Directory1 = "Ana Sayfa";
            ViewBag.Directory2 = "Kampanyalar";

            string bannerTitle = "Özel Kampanya";
            string bannerSubtitle = "Kampanyaya Özel Seçili Ürünler";
            string bannerImage = "";
            var productIds = new List<string>();

            if (type == "slider")
            {
                var slider = await _featureSliderService.GetByIdFeatureSlider(id);
                if (slider != null)
                {
                    bannerTitle = slider.Title;
                    bannerSubtitle = slider.Description;
                    bannerImage = slider.ImageUrl;
                    productIds = slider.ProductIds ?? new List<string>();
                }
            }
            else if (type == "special")
            {
                var special = await _specialOfferService.GetByIdSpecialOffer(id);
                if (special != null)
                {
                    bannerTitle = special.Title;
                    bannerSubtitle = special.Subtitle;
                    bannerImage = special.ImageUrl;
                    productIds = special.ProductIds ?? new List<string>();
                }
            }
            else if (type == "offer")
            {
                var offer = await _offerDiscountService.GetByIdOfferDiscount(id);
                if (offer != null)
                {
                    bannerTitle = offer.Title;
                    bannerSubtitle = offer.Subtitle;
                    bannerImage = offer.ImageUrl;
                    productIds = offer.ProductIds ?? new List<string>();
                }
            }

            ViewBag.BannerTitle = bannerTitle;
            ViewBag.BannerSubtitle = bannerSubtitle;
            ViewBag.BannerImage = bannerImage;
            ViewBag.Directory3 = bannerTitle;

            var products = await _productService.GetProductsByIdsAsync(productIds);

            var discounts = await _discountService.GetActiveProductDiscountsAsync();
            var discountDict = discounts.GroupBy(x => x.ProductId).ToDictionary(g => g.Key, g => g.First().Rate);
            ViewBag.DiscountDict = discountDict;

            return View(products);
        }
    }
}
