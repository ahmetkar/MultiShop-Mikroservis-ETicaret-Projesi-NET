using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultiShop.Payment.DAL.Context;
using MultiShop.Payment.DTOs;
using MultiShop.Payment.Services;
using MultiShop.SharedLayer.Events;
using MultiShop.SharedLayer.Kafka;

namespace MultiShop.Payment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly PaymentContext _paymentContext;
        private readonly IKafkaProducer _kafkaProducer;

        public PaymentsController(IPaymentService paymentService,IKafkaProducer kafkaProducer,PaymentContext paymentContext)
        {
            _paymentService = paymentService;
            _kafkaProducer = kafkaProducer;
            _paymentContext = paymentContext;
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
        public async Task<IActionResult> AddPayment(CreatePaymentDto createPaymentDto,CancellationToken cancellationToken)
        {
            var orderSnapshot = await _paymentContext.PaymentOrderSnapshots.FirstOrDefaultAsync(x => x.OrderingId == createPaymentDto.OrderingId, cancellationToken);

            if (orderSnapshot is null)
            {
                return Ok(new { success = false });

            }


            try
            {
                var add = await _paymentService.AddPayment(createPaymentDto, cancellationToken);

                if (add is not null)
                {
                    var paymentCompletedEvent = new PaymentCompletedEvent
                    {
                        OrderingId = add.OrderingId,
                        PaymentTotal = add.PaymentTotal,
                        UserId = add.UserId,
                        PaymentId = add.PaymentId,
                        CardInfoId = add.CardInfoId,
                        CorrrelationId = orderSnapshot.CorrelationId
                    };

                    await _kafkaProducer.PublishAsync(KafkaTopics.PaymentCompleted, paymentCompletedEvent, add.OrderingId.ToString(), cancellationToken);


                    return Ok(new { success = true });


                }

            }
            catch (Exception ex) {
                var paymentFailedEvent = new PaymentFailedEvent
                {
                    OrderingId = orderSnapshot.OrderingId,
                    Reason = ex.Message,
                    CorrrelationId = orderSnapshot.CorrelationId
                };

                await _kafkaProducer.PublishAsync(KafkaTopics.PaymentFailed,paymentFailedEvent,orderSnapshot.OrderingId.ToString(), cancellationToken);
                
                return Ok(new { success = false });

            }

            return Ok(new { success = false });
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
