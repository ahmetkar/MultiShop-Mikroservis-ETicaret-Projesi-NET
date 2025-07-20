using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.CommentDtos;
using MultiShop.WebUI.Services.Interfaces;

namespace MultiShop.WebUI.Controllers
{
    public class ProductListController : Controller
    {
        private readonly IHttpService _httpService;
        public ProductListController(IHttpService httpService)
        {
            _httpService = httpService;
            _httpService.setUrl("CommentApi");
        }
        public IActionResult Index(string id)
        {
            ViewBag.Directory1 = "Ana Sayfa";
            ViewBag.Directory2 = "Ürün Listesi";
            ViewBag.Directory3 = "";
            ViewBag.i = id;
            return View();
        }

        public IActionResult ProductDetail(string id)
        {
            ViewBag.Directory1 = "Ana Sayfa";
            ViewBag.Directory2 = "Ürün Listesi";
            ViewBag.Directory3 = "Ürün Detayları";
            ViewBag.id = id;
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
            //Rating ayarlanacak ui tarafında
            createCommentDto.ImageUrl = "~/img/user.png";
            createCommentDto.Rating = 1;
            createCommentDto.CreatedDate = DateTime.Parse(DateTime.Now.ToShortDateString());
            createCommentDto.Status = false;
           
            var create = await _httpService.Create<CreateCommentDto>("Comments", createCommentDto);
            if(create) return RedirectToAction("ProductDetail", "ProductList", new { id = createCommentDto.ProductId });
            return RedirectToAction("Index", "Default");
        }
    }
}
