using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MultiShop.WebUI.Filters
{
    public class OrderAuthorizeFilter : IAsyncAuthorizationFilter
    {

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new RedirectToActionResult("Login", "Auth",new {reasonorder = "yes"});

            }
            await Task.CompletedTask;
        }
    }
}
