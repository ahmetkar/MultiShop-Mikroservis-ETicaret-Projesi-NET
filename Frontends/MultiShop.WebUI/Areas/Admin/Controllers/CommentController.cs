using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.CommentDtos;
using MultiShop.WebUI.Services.Interfaces;

namespace MultiShop.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AllowAnonymous]
    [Route("Admin/Comment")]
    public class CommentController : Controller
    {

        private readonly IHttpService _httpService;
        public CommentController(IHttpService httpService)
        {
            _httpService = httpService;

            _httpService.setUrl("CommentApi");
        }


        public async Task<IActionResult> Index()
        {
            ViewBag.v0 = "Yorum İşlemleri";
            ViewBag.v1 = "Ana Sayfa";
            ViewBag.v2 = "Yorumler";
            ViewBag.v3 = "Yorum Listesi";


            var result = await _httpService.Get<ResultCommentDto>("Comments");
            if (result != null) return View(result);
            return View();
        }



        [Route("DeleteComment/{id}")]
        public async Task<IActionResult> DeleteComment(string id)
        {


            var result = await _httpService.DeleteById("Comments", id);
            if (result) { return RedirectToAction("Index", new { area = "Admin" }); }
            return View();
        }

        [Route("UpdateComment/{id}")]
        [HttpGet]
        public async Task<IActionResult> UpdateComment(string id)
        {
            ViewBag.v0 = "Yorum İşlemleri";
            ViewBag.v1 = "Ana Sayfa";
            ViewBag.v2 = "Yorumler";
            ViewBag.v3 = "Yorum Güncelle";

            var result = await _httpService.GetById<UpdateCommentDto>("Comments", id);
            if (result != null) return View(result);
            return View();
        }

        [Route("UpdateComment/{id}")]
        [HttpPost]
        public async Task<IActionResult> UpdateComment(UpdateCommentDto updateCommentDto)
        {
            
            var result = await _httpService.Update<UpdateCommentDto>("Comments", updateCommentDto);
            if (result) { return RedirectToAction("Index", "Comment", new { area = "Admin" }); }
            return View();
        }
    }
}
