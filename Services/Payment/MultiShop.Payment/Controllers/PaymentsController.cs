using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MultiShop.Payment.DTOs;
using MultiShop.Payment.Services;

namespace MultiShop.Payment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet("GetPaymentByOrderingId/{id}")]
        [Authorize(Policy = "PaymentReadPolicy")]
        public async Task<IActionResult> GetPaymentByOrderingId(int id)
        {
            var values = await _paymentService.GetPaymentByOrderingId(id);
            return Ok(values);
        }

        [HttpGet("GetPaymentsByUserId/{id}")]
        [Authorize(Policy = "PaymentReadPolicy")]
        public async Task<IActionResult> GetPaymentsByUserId(string id)
        {
            var values = await _paymentService.GetAllPaymentByUserId(id);
            return Ok(values);
        }


        [HttpPost]
        [Authorize(Policy = "PaymentCreatePolicy")]
        public async Task<IActionResult> AddPayment(CreatePaymentDto createPaymentDto)
        {
            var add = await _paymentService.AddPayment(createPaymentDto);
            return Ok(new {success = add });
        }

        [HttpDelete("CancelPaymentByOrderingId/{id}")]
        [Authorize(Policy = "PaymentDeletePolicy")]
        public async Task<IActionResult> CancelPaymentByOrderingId(int id)
        {
            var delete = await _paymentService.CancelPaymentByOrderingId(id);
            return Ok(new {success = delete});
        }
    }
}
