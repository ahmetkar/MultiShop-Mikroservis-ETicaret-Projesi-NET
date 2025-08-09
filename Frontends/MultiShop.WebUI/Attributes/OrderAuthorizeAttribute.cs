using Microsoft.AspNetCore.Mvc;
using MultiShop.WebUI.Filters;

namespace MultiShop.WebUI.Attributes
{
    public class OrderAuthorizeAttribute : TypeFilterAttribute
    {
        public OrderAuthorizeAttribute() : base(typeof(OrderAuthorizeFilter))
        {
        }
    }
}
