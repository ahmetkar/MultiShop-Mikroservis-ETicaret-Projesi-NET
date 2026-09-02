using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.CommentDtos;
using MultiShop.WebUI.Services.CatalogServices.ProductServices;
using MultiShop.WebUI.Services.CommentServices;
using MultiShop.WebUI.Services.DiscountServices;

namespace MultiShop.WebUI.Controllers
{
    public class ProductListController : Controller
    {
        private readonly ICommentService _commentService;
        private readonly IProductService _productService;
        private readonly IDiscountService _discountService;

        public ProductListController(ICommentService commentService, IProductService productService, IDiscountService discountService)
        {
            _commentService = commentService;
            _productService = productService;
            _discountService = discountService;
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
    }
}
