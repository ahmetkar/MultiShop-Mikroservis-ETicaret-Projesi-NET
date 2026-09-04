using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.OrderDtos.OrderDetailDtos;
using MultiShop.DtoLayer.OrderDtos.OrderOrderingDtos;
using MultiShop.WebUI.Services.CatalogServices.ProductServices;
using MultiShop.WebUI.Services.OrderServices.OrderAddressServices;
using MultiShop.WebUI.Services.OrderServices.OrderDetailServices;
using MultiShop.WebUI.Services.OrderServices.OrderOderingServices;

namespace MultiShop.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Order")]
    public class OrderController : Controller
    {
        private readonly IOrderOderingService _orderOderingService;
        private readonly IOrderDetailService _orderDetailService;
        private readonly IOrderAddressService _orderAddressService;
        private readonly IProductService _productService;

        public OrderController(
            IOrderOderingService orderOderingService,
            IOrderDetailService orderDetailService,
            IOrderAddressService orderAddressService,
            IProductService productService)
        {
            _orderOderingService = orderOderingService;
            _orderDetailService = orderDetailService;
            _orderAddressService = orderAddressService;
            _productService = productService;
        }

        void OrderViewBag(string pagename)
        {
            ViewBag.v0 = "Sipariş İşlemleri";
            ViewBag.v1 = "Ana Sayfa";
            ViewBag.v2 = "Siparişler";
            ViewBag.v3 = pagename;
        }

        [Route("Index")]
        [Route("")]
        public async Task<IActionResult> Index()
        {
            OrderViewBag("Sipariş Listesi");
            var orders = await _orderOderingService.GetAllOrderingsAsync();
            return View(orders);
        }

        [HttpGet]
        [Route("OrderDetail/{id}")]
        public async Task<IActionResult> OrderDetail(int id)
        {
            OrderViewBag("Sipariş Detayları");
            var ordering = await _orderOderingService.GetOrderingById(id);
            var orderDetails = await _orderDetailService.GetOrderDetailsByOrderingId(id);

            ViewBag.Ordering = ordering;
            ViewBag.OrderDetails = orderDetails;

            return View(orderDetails);
        }

        [HttpPost]
        [Route("UpdateOrderStatus")]
        public async Task<IActionResult> UpdateOrderStatus(int orderingId, OrderStatus status)
        {
            var ordering = await _orderOderingService.GetOrderingById(orderingId);
            if (ordering != null)
            {
                var updateDto = new UpdateOrderingDto
                {
                    OrderingId = orderingId,
                    OrderDate = ordering.OrderDate,
                    BillingAddressId = ordering.BillingAddressId,
                    ShippingAdressId = ordering.ShippingAdressId,
                    TotalPrice = ordering.TotalPrice,
                    UserId = ordering.UserId,
                    Status = status
                };
            }
            return RedirectToAction("OrderDetail", new { id = orderingId });
        }

        [HttpGet]
        [Route("DeleteOrder/{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            await _orderOderingService.DeleteOrdering(id);
            return RedirectToAction("Index");
        }
    }
}
