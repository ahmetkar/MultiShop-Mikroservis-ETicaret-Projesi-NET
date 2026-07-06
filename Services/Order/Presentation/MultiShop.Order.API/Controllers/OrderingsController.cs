using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MultiShop.Order.API.Dtos;
using MultiShop.Order.Application.Features.Mediator.Commands.OrderingCommands;
using MultiShop.Order.Application.Features.Mediator.Queries.OrderingQueries;
using MultiShop.SharedLayer.Kafka;
using MultiShop.SharedLayer.Events;
using MultiShop.Order.Application.Features.Mediator.Results.OrderingResult;

namespace MultiShop.Order.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderingsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IKafkaProducer _kafkaProducer;

        public OrderingsController(IMediator mediator,IKafkaProducer kafkaProducer)
        {
            _mediator = mediator;
            _kafkaProducer = kafkaProducer;
        }

        [HttpGet("OrderingList")]
        public async Task<IActionResult> OrderingList()
        {
            var values = await _mediator.Send(new GetOrderingQuery());
            return Ok(values);
        }

        [HttpGet("GetOrderingById/{id}")]
        public async Task<IActionResult> GetOrderingById(int id)
        {
            var values = await _mediator.Send(new GetOrderingByIdQuery(id));
            return Ok(values);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrdering(CreateOrderingCommand command,CancellationToken cancellationToken = default)
        {
            var correlationId = Guid.NewGuid();
            CreateOrderingResult res = await _mediator.Send(command);

            if(res is not null)
            {
                var orderCreatedEvent = new OrderCreatedEvent
                {
                    UserId = res.UserId,
                    OrderingId = res.OrderingId,
                    TotalPrice = res.TotalPrice,
                    ShippingAdressId = res.ShippingAdressId,
                    BillingAddressId = res.BillingAddressId,
                    OrderDate = res.OrderDate,
                    CorrrelationId = correlationId,
                    CreatedDate = DateTime.UtcNow
                };

                await _kafkaProducer.PublishAsync(KafkaTopics.OrderCreated, orderCreatedEvent, res.OrderingId.ToString(),cancellationToken);
                
                return Ok(new CreateOrderingResultDto() { OrderingId = res.OrderingId });
            }
            else
            {
                var orderNotCreatedEvent = new OrderNotCreatedEvent
                {
                    UserId = command.UserId,
                    OrderDate = command.OrderDate,
                    CorrrelationId = correlationId,
                    CreatedDate = DateTime.UtcNow
                };
                await _kafkaProducer.PublishAsync(KafkaTopics.OrderNotCreated, orderNotCreatedEvent, command.UserId.ToString(), cancellationToken);

                return Ok(new CreateOrderingResultDto() { });
            }

                
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveOrdering(int id)
        {
            await _mediator.Send(new RemoveOrderingCommand(id));
            return Ok("Sipariş başarıyla silindi.");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateOrdering(UpdateOrderingCommand updateOrderingRequest)
        {
            await _mediator.Send(updateOrderingRequest);
            return Ok("Sipariş başarıyla güncellendi.");
        }

        [HttpGet("GetOrderingsByUserId/{id}")]
        public async Task<IActionResult> GetOrderingsByUserId(string id)
        {
            var values = await _mediator.Send(new GetOrderingByUserIdQuery(id));
            return Ok(values);
        }

        [HttpGet("GetActiveOrderingByUserId/{id}")]
        public async Task<IActionResult> GetActiveOrderingByUserId(string id)
        {
            var values = await _mediator.Send(new GetOrderingByUserIdQuery(id));
            var activeOrder = values.FirstOrDefault(x=>!x.IsOrderCompleted && !x.IsOrderDelivered);
            return Ok(activeOrder);
        }


    }
}
