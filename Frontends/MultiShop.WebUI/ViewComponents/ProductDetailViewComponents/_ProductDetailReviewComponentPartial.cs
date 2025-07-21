using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.CommentDtos;
using MultiShop.WebUI.Services.CommentServices;
using MultiShop.WebUI.Services.Interfaces;

namespace MultiShop.WebUI.ViewComponents.ProductDetailViewComponents
{
    public class _ProductDetailReviewComponentPartial : ViewComponent
    {
      
        private readonly ICommentService _commentService;
        public _ProductDetailReviewComponentPartial(ICommentService commentService)
        {
            _commentService = commentService;
        }


        public async Task<IViewComponentResult> InvokeAsync(string id)
        {
            var result = await _commentService.GetCommentsByProductId(id);
            return View(result);
        }
    }
}
