using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.CommentDtos;
using MultiShop.WebUI.Services.Interfaces;

namespace MultiShop.WebUI.ViewComponents.ProductDetailViewComponents
{
    public class _ProductDetailReviewComponentPartial : ViewComponent
    {
      

        private readonly IHttpService _httpService;
        public _ProductDetailReviewComponentPartial(IHttpService httpService)
        {
            _httpService = httpService;

            _httpService.setUrl("CommentApi");
        }


        public async Task<IViewComponentResult> InvokeAsync(string id)
        {
            var result = await _httpService.Get<ResultCommentDto>("Comments/CommentsByProductId?id=" + id);
            if (result != null) return View(result);
            return View();
        }
    }
}
